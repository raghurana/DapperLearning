using System;
using DapperLearning.ConsoleApp.Utils;

namespace DapperLearning.ConsoleApp.Data.Queries
{
    public class VacancyQueries
    {
        private readonly Lazy<string> vacanciesByFilterQuery;

        public string VacanciesByFilterQuery => vacanciesByFilterQuery.Value;

        public VacancyQueries(QueryHelper queryHelper)
        {
            this.vacanciesByFilterQuery = new Lazy<string>(() => queryHelper.GetQuerySqlByFileName("VacanciesByFacility"));
        }
    }
}
