namespace DapperLearning.ConsoleApp.Data.Repositories
{
    public class ProgramRepository
    {
        private IDbConnectionProvider factory;

        public ProgramRepository(IDbConnectionProvider factory)
        {
            this.factory = factory;
        }


    }
}