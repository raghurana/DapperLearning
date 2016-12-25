using System.Configuration;

namespace DapperLearning.ConsoleApp.Data
{
    public interface IConnectionStringProvider
    {
        string GetReadConnectionString();

        string GetWriteConnectionString();
    }

    public class AppSettingConnectionStringProvider : IConnectionStringProvider
    {
        private const string ReadConnectionStringName = "ReadConnectionString";
        private const string WriteConnectionStringName = "WriteConnectionString";

        public string GetReadConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[ReadConnectionStringName].ConnectionString;
        }

        public string GetWriteConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[WriteConnectionStringName].ConnectionString;
        }
    }
}
