namespace DapperLearning.ConsoleApp.Data.Queries
{
    public interface ICustomQueryProvider
    {
        VacancyQueries VacancyQueries { get; }
    }

    public class EmbeddedCustomQueryProvider : ICustomQueryProvider
    {
        public VacancyQueries VacancyQueries { get; }

        public EmbeddedCustomQueryProvider(VacancyQueries vacancyQueries)
        {
            VacancyQueries = vacancyQueries;
        }
    }
}
