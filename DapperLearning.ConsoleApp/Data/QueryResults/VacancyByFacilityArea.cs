using DapperLearning.ConsoleApp.Data.Entities;

namespace DapperLearning.ConsoleApp.Data.QueryResults
{
    public class VacancyByFacilityArea
    {
        public Vacancy Vacancy { get; }

        public Qualification Qualification { get; }

        public  Facility Facility { get; }

        public FacilityArea Area { get; }

        public VacancyByFacilityArea(Vacancy vacancy, Qualification qualification, Facility facility, FacilityArea area)
        {
            Vacancy       = vacancy;
            Qualification = qualification;
            Facility      = facility;
            Area          = area;
        }
    }
}