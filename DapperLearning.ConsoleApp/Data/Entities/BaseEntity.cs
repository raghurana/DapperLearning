using System;

namespace DapperLearning.ConsoleApp.Data.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
    }

    public class Facility : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class FacilityArea : BaseEntity
    {
        public long FacilityId { get; set; }

        public string Name { get; set; }
    }

    public class Qualification : BaseEntity
    {
        public string Abbreviation { get; set; }

        public string Description { get; set; }
    }

    public class Vacancy : BaseEntity
    {
        public long FacilityId { get; set; }

        public long? FacilityAreaId { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }

    public class VacancyQualificationMapping : BaseEntity
    {
        public long VacancyId { get; set; }

        public long QualificationId { get; set; }
    }
}