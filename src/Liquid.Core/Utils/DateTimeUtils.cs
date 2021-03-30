using System;
using System.Globalization;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Date time extensions class.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Converts datetime to Iso 8601 format.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToIso8601(this DateTime date)
        {
            return date.ToString("o", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts datetime to SQL invariant format.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToSql(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// To the Oracle SQL date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string ToOracleSql(this DateTime dateTime)
        {
            return $"to_date('{dateTime:dd.MM.yyyy HH:mm:ss}','dd.mm.yyyy hh24.mi.ss')";
        }

        /// <summary>
        /// Converts datetime to Unix format.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static long ToUnix(this DateTime date)
        {
            var timeSpan = date - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Gets date time from unix format.
        /// </summary>
        /// <param name="unix">The unix.</param>
        /// <returns></returns>
        public static DateTime FromUnix(this long unix)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unix);
        }

        /// <summary>
        /// Gets the age from the birth date.
        /// </summary>
        /// <param name="birthDate">The birth date.</param>
        /// <returns></returns>
        public static int GetAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var result = today.Year - birthDate.Year;

            if (today.DayOfYear < birthDate.DayOfYear)
            {
                result--;
            }

            return result;
        }

        /// <summary>
        /// Converts the datetime to a specific time zone.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        /// <returns></returns>
        public static DateTime ToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            var universalDateTime = dateTime.ToUniversalTime();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
            return convertedDateTime;
        }

        /// <summary>
        /// Determines whether this date is weekend.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>true</c> if the specified d is weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime date)
        {
            return !date.IsWeekday();
        }

        /// <summary>
        /// Determines whether this instance is weekday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>true</c> if the specified date is weekday; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekday(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Adds the workdays.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime AddWorkdays(this DateTime date, int days)
        {
            while (date.IsWeekend()) date = date.AddDays(1.0);
            for (var i = 0; i < days; ++i)
            {
                date = date.AddDays(1.0);
                while (date.IsWeekend()) date = date.AddDays(1.0);
            }
            return date;
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="date">The date time.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Gets the first day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Get the elapsed time from date time now since the input DateTime
        /// </summary>
        /// <param name="date">Input DateTime</param>
        /// <returns>Returns a TimeSpan value with the elapsed time since the input DateTime</returns>
        /// <example>
        /// TimeSpan elapsed = dtStart.Elapsed();
        /// </example>
        public static TimeSpan Elapsed(this DateTime date)
        {
            return DateTime.Now.Subtract(date);
        }
    }
}