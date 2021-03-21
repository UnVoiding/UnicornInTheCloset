using System;

namespace RomenoCompany
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startDayOfWeek)
        {
            var diff = (7 + (dt.DayOfWeek - startDayOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }
    
        public static DateTime DayOfCurrentWeek(this DateTime dt, DayOfWeek day)
        {
            var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            return startOfWeek.AddDays(DayOfWeekIndex(day));
        }

        public static int DayOfWeekIndex(DayOfWeek dow)
        {
            var ret = (int)dow - 1;
        
            return ret < 0 ? 6 : ret;
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target < start)
                target += 7;
            return from.AddDays(target - start);
        }
    
        public static DateTime Next2(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static DateTime TruncateToDayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }
    }    
}
