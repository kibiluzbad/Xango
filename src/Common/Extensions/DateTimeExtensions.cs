using System;

namespace Xango.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsToday(this System.DateTime dateTime)
        {
            return System.DateTime.Today == dateTime.Date;
        }

        public static System.DateTime AddWorkingDays(this System.DateTime specificDate,
                                      int workingDaysToAdd)
        {
            int completeWeeks = workingDaysToAdd / 5;
            System.DateTime date = specificDate.AddDays(completeWeeks * 7);
            workingDaysToAdd = workingDaysToAdd % 5;
            for (int i = 0; i < workingDaysToAdd; i++)
            {
                date = date.AddDays(1);
                while (!IsWeekDay(date))
                {
                    date = date.AddDays(1);
                }
            }
            return date;
        }

        public static bool IsWeekDay(this System.DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            return day != DayOfWeek.Saturday && day != DayOfWeek.Sunday;
        }
    }
}