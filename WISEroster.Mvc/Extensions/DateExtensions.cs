using System;

namespace WISEroster.Mvc.Extensions
{
    public static class DateExtensions
    {
        public static short GetSchoolYear(this DateTime date)
        {
            if (date.Month > 6)
            {
                return (short)(date.Year + 1);
            }
            return (short)date.Year;
        }
    }
}