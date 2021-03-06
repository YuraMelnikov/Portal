﻿using System;
using System.Linq;

namespace Wiki.Models
{
    public class WorkingDays
    {
        public int GetWorkingDays(DateTime from, DateTime to)
        {
            try
            {
                var dayDifference = (int)to.Subtract(from).TotalDays;
                return Enumerable
                    .Range(1, dayDifference)
                    .Select(x => from.AddDays(x))
                    .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
            }
            catch
            {
                return 0;
            }
        }
    }
}