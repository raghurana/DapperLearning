using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperLearning.ConsoleApp.Data
{
    public interface IDbConnectionProvider
    {
        Task<SqlConnection> GetOpenReadConnection();

        Task<SqlConnection> GetOpenWriteConnection();
    }

    public class DefaultDbConnectionProvider : IDbConnectionProvider
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public DefaultDbConnectionProvider(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public Task<SqlConnection> GetOpenReadConnection()
        {
            var readConnectionString = connectionStringProvider.GetReadConnectionString();
            return GetOpenSqlConnection(readConnectionString);
        }

        public Task<SqlConnection> GetOpenWriteConnection()
        {
            var writeConnectionString = connectionStringProvider.GetWriteConnectionString();
            return GetOpenSqlConnection(writeConnectionString);
        }

        private static async Task<SqlConnection> GetOpenSqlConnection(string connectionString)
        {
            var readConnection = new SqlConnection(connectionString);
            await readConnection.OpenAsync();
            return readConnection;
        }
    }
}
