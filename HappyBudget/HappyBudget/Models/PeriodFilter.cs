using System;
using System.Collections.Generic;
using System.Text;

namespace HappyBudget.Models
{
    public class PeriodFilter
    {
        public string Name { get; set; }
        public int NumberOfDays { get; set; }


        public PeriodFilter(string name, int numberOfDays)
        {
            Name = name;
            NumberOfDays = numberOfDays;
        }

        public static List<PeriodFilter> GetPeriodFilters()
        {
            return new List<PeriodFilter>
            {
                new PeriodFilter("TODAY", 0),
                new PeriodFilter("LAST 7 DAYS", 7),
                new PeriodFilter("LAST 30 DAYS", 30),
                new PeriodFilter("LAST 1 YEAR", 365),
            };
        }
    }
}
