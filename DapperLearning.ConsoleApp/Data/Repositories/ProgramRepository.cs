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
                facilityNoAreas.Id = await connection.InsertAsync<long>(facilityNoAreas);

                var facilityWithAreas = new Facility { Name = "Facility-2", IsActive = true };
                facilityWithAreas.Id = await connection.InsertAsync<long>(facilityWithAreas);

                var area1 = new FacilityArea { FacilityId = facilityWithAreas.Id, Name = $"{facilityWithAreas.Name}-Area1" };
                var area2 = new FacilityArea { FacilityId = facilityWithAreas.Id, Name = $"{facilityWithAreas.Name}-Area2" };

                using (var txn = connection.BeginTransaction())
                {
                    area1.Id = await connection.InsertAsync<long>(area1, txn);
                    area2.Id = await connection.InsertAsync<long>(area2, txn);
                    txn.Commit();
                }
            }

            return true;
        }
    }
}