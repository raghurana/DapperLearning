using System.Collections.Generic;
using DapperLearning.ConsoleApp.Data.Entities;

namespace DapperLearning.ConsoleApp.Data.QueryResults
{
    public class FacilityWithArea 
    {
        public Facility Facility { get; }

        public FacilityArea Area { get; }

        public FacilityWithArea(Facility facility, FacilityArea area)
        {
            Facility = facility;
            Area     = area;
        }

        public override string ToString()
        {
            return Area != null 
                ? $"{Facility}{BaseEntity.FieldDelimeter}{Area}" 
                : $"{Facility}{BaseEntity.FieldDelimeter}No Areas";
        }
    }
}