namespace DapperLearning.ConsoleApp.Utils
{
    public class QueryHelper
    {
        private readonly IResourceReader resourceReader;

        public QueryHelper(IResourceReader resourceReader)
        {
            this.resourceReader = resourceReader;
        }

        public string GetQuerySqlByFileName(string fileName)
        {
            return resourceReader.GetResourceContents<string>("Data.Queries", $"{fileName}.sql");
        }
    }
}
