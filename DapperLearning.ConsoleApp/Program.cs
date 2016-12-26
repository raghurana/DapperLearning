using System;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using DapperLearning.ConsoleApp.Data;
using DapperLearning.ConsoleApp.Data.Repositories;
using DbUp;
using Ninject;

namespace DapperLearning.ConsoleApp
{
    public class Program
    {
        public static IKernel Container { get; } = new StandardKernel(new DependencyMap());

        public static void Main(string[] args)
        {
            SetupDatabase();

            try
            {
                MainAsync().Wait();
            }
            catch (Exception exception)
            {
                LogMessage(exception.Message, false);
            }
            
            TearDownDatabase();
        }

        private static async Task MainAsync()
        {
            var programRepo = Container.Get<ProgramRepository>();

            var insertSuccessful = await programRepo.InsertRecords();
            if (!insertSuccessful)
            {
                LogMessage("Failed to Insert records !", false);
                return;
            }

            var updateSuccessful = await programRepo.UpdateRecords();
            if (!updateSuccessful)
            {
                LogMessage("Failed to Update records !", false);
                return;
            }

            await programRepo.GetAllFacilitiesWithAreas();
            programRepo.GetVacanciesByFacility();
        }

        private static void SetupDatabase()
        {
            var connStringProvider = Container.Get<IConnectionStringProvider>();
            var writeConnectionString = connStringProvider.GetWriteConnectionString();

            EnsureDatabase.For.SqlDatabase(writeConnectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(writeConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), ScriptsFilter)
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                LogMessage(result.Error.Message, false);
                return;
            }

            LogMessage("Database Setup Successful.");
        }

        private static void TearDownDatabase()
        {
            const string dbName = "DapperLearn";
            var dropCommand     = $"USE master;ALTER DATABASE {dbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;Drop database {dbName};";

            using (var connectionAsync = Container.Get<IDbConnectionProvider>().GetOpenWriteConnection())
            {
                var count = connectionAsync.Result.Execute(dropCommand);

                if (count == 0)
                {
                    LogMessage("Db tearndown failed.", false);
                    return;
                }

                LogMessage("Database Tearndown Successful.");
            }
        }

        private static void LogMessage(string message, bool isSuccess = true)
        {
            Console.ForegroundColor = (isSuccess) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static bool ScriptsFilter(string fullyQualifiedFileName)
        {
            return fullyQualifiedFileName.Contains("Scripts.Script");
        }
    }
}
