using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperLearning.ConsoleApp.Data.Entities;
using DapperLearning.ConsoleApp.Data.Queries;
using DapperLearning.ConsoleApp.Data.QueryResults;
using DapperLearning.ConsoleApp.Utils;

namespace DapperLearning.ConsoleApp.Data.Repositories
{
    public class ProgramRepository
    {
        private readonly IDbConnectionProvider factory;
        private readonly ICustomQueryProvider customQueryProvider;

        public ProgramRepository(IDbConnectionProvider factory, ICustomQueryProvider customQueryProvider)
        {
            this.factory = factory;
            this.customQueryProvider = customQueryProvider;
        }

        public async Task<bool> InsertRecords()
        {
            using (var connection = await factory.GetOpenWriteConnection())
            {
                var facilityNoAreas = new Facility { Name = "Facility-1", IsActive = true };
                facilityNoAreas.Id  = await connection.InsertAsync<long>(facilityNoAreas);

                var facilityWithAreas = new Facility { Name = "Facility-2", IsActive = true };
                facilityWithAreas.Id  = await connection.InsertAsync<long>(facilityWithAreas);

                var area1 = new FacilityArea { FacilityId = facilityWithAreas.Id, Name = $"{facilityWithAreas.Name}-Area1" };
                var area2 = new FacilityArea { FacilityId = facilityWithAreas.Id, Name = $"{facilityWithAreas.Name}-Area2" };

                using (var txn = connection.BeginTransaction())
                {
                    area1.Id = await connection.InsertAsync<long>(area1, txn);
                    area2.Id = await connection.InsertAsync<long>(area2, txn);
                    txn.Commit();
                }

                var qualDivOne  = new Qualification {Abbreviation = "Div1", Description = "Division One Nurse"};
                var qualRn      = new Qualification {Abbreviation = "RN"  , Description = "Registered Nurse"};
                var qualPca     = new Qualification {Abbreviation = "PCA" , Description = "Personal Care Assistant"};

                using (var txn = connection.BeginTransaction())
                {
                    qualDivOne.Id = await connection.InsertAsync<long>(qualDivOne, txn);
                    qualRn.Id     = await connection.InsertAsync<long>(qualRn, txn);
                    qualPca.Id    = await connection.InsertAsync<long>(qualPca, txn);
                    txn.Commit();
                }

                var vacancy1 = new Vacancy {FacilityId = facilityNoAreas.Id, CreatedDateUtc = DateTime.UtcNow};
                var vacancy2 = new Vacancy {FacilityId = facilityWithAreas.Id, FacilityAreaId = area1.Id, CreatedDateUtc = DateTime.UtcNow};
                var vacancy3 = new Vacancy {FacilityId = facilityWithAreas.Id, FacilityAreaId = area2.Id, CreatedDateUtc = DateTime.UtcNow};

                using (var txn = connection.BeginTransaction())
                {
                    await AddVacancyWithQualification(vacancy1, qualDivOne, connection, txn);
                    await AddVacancyWithQualification(vacancy2, qualRn, connection, txn);
                    await AddVacancyWithQualification(vacancy3, qualPca, connection, txn);
                    txn.Commit();
                }
            }

            return true;
        }

        public async Task<bool> UpdateRecords()
        {
            using (var connection = await factory.GetOpenWriteConnection())
            {
                var facilities = (await 
                                    connection.GetListAsync<Facility>(
                                        $"where {nameof(Facility.Name)} = @FacilityName",
                                        new {FacilityName = "Facility-1"})
                                 ).ToList();

                if(!facilities.Any())
                    throw new Exception("No facilities found");

                var facility = facilities.First();
                facility.Name = "Facility-1-NoAreas";

                var updateCount = await connection.UpdateAsync(facility);
                if(updateCount == 0)
                    throw new Exception("Failed to update facility with no areas");

                updateCount = await connection.ExecuteAsync(
                                $"Update {nameof(Facility)} set {nameof(Facility.Name)} = @NewFacName where {nameof(Facility.Name)} = @OldFacName",
                                new { OldFacName = "Facility-1-NoAreas", NewFacName = "Facility-1"});

                if (updateCount == 0)
                    throw new Exception("Failed to update facility with no areas using Execute Async");
            }

            return true;
        }

        public async Task<IList<FacilityWithArea>> GetAllFacilitiesWithAreas()
        {
            using (var connection = await factory.GetOpenWriteConnection())
            {
                var query  = "select * from Facility f left join FacilityArea fa on f.Id = fa.FacilityId";
                var result = await connection.QueryAsync<Facility, FacilityArea, FacilityWithArea>(query, (f, fa) => new FacilityWithArea(f, fa));
                return result.ToList();
            }
        }

        public async Task<IList<VacancyByFacilityArea>> GetVacanciesByFacility()
        {
            using (var connection = await factory.GetOpenWriteConnection())
            {
                var sql    = customQueryProvider.VacancyQueries.VacanciesByFilterQuery;
                var result = await
                    connection.QueryAsync<Vacancy, Qualification, Facility, FacilityArea, VacancyByFacilityArea>(sql,
                        (v, q, f, fa) => new VacancyByFacilityArea(v, q, f, fa));

                return result.ToList();
            }
        }

        private static async Task<VacancyQualificationMapping> AddVacancyWithQualification(Vacancy vacancy, Qualification qualification, SqlConnection connection, SqlTransaction txn)
        {
            vacancy.Id  = await connection.InsertAsync<long>(vacancy, txn);
            var map = new VacancyQualificationMapping { VacancyId = vacancy.Id, QualificationId = qualification.Id };
            map.Id  = await connection.InsertAsync<long>(map, txn);
            return map;
        }
    }
}