using System;

namespace DapperLearning.ConsoleApp.Data.Entities
{
    public abstract class BaseEntity
    {
        public const string FieldDelimeter = ", ";

        public long Id { get; set; }
    }

    public class Facility : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"FacilityId={Id}{FieldDelimeter}Name={Name}";
        }
    }

    public class FacilityArea : BaseEntity
    {
        public long FacilityId { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"AreaId={Id}{FieldDelimeter}Name={Name}";
        }
    }

    public class Qualification : BaseEntity
    {
        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Id}{FieldDelimeter}{Abbreviation}";
        }
    }

    public class Vacancy : BaseEntity
    {
        public long FacilityId { get; set; }

        public long? FacilityAreaId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public override string ToString()
        {
            return $"{Id}{FieldDelimeter}FacilityId={FacilityId}{FieldDelimeter}AreaId={FacilityAreaId}{FieldDelimeter}CreateDateUtc={CreatedDateUtc}";
        }
    }

    public class VacancyQualificationMapping : BaseEntity
    {
        public long VacancyId { get; set; }

        public long QualificationId { get; set; }

        public override string ToString()
        {
            return $"VacancyId={VacancyId}{FieldDelimeter}QualId={QualificationId}";
        }
    }
}