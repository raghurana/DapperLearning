using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DapperLearning.ConsoleApp.Data.Entities;

namespace DapperLearning.ConsoleApp.Data.Repositories
{
    public class ProgramRepository
    {
        private readonly IDbConnectionProvider factory;

        public ProgramRepository(IDbConnectionProvider factory)
        {
            this.factory = factory;
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

                return true;
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