using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.Utility
{
    public static class Constants
    {
        /// <summary>Enumeration for expanded month names of an year</summary>
        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        };

        /// <summary>Enumeration for date entity. It could be a month, a year, a day, a hour, a minute, a second, etc.</summary>
        public enum DateTimeEntityType
        {
            Year,
            Month,
            Week,
            Day,
            Hour,
            Minute,
            Second,
            MilliSecond,
            DayOfMonth,
            DayOfWeek,
            DayOfYear,
            HourOfDay
        };

        public const string StatusCode_UnPaid = "UNPD";

        public const string TypeCode_OUUU = "OUUU";
       
        
    }
}
