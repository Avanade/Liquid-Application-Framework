using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Utils;
using NUnit.Framework;

namespace Liquid.Core.Tests.Utils
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DateTimeUtilsTest
    {
        /// <summary>
        /// Verifies to iso8601.
        /// </summary>
        [Test]
        public void Verify_ToIso8601()
        {
            var sut = new DateTime(1990, 01, 01);
            var result = sut.ToIso8601();
            Assert.AreEqual("1990-01-01T00:00:00.0000000", result);
        }

        /// <summary>
        /// Verifies to SQL.
        /// </summary>
        [Test]
        public void Verify_ToSql()
        {
            var sut = new DateTime(1990, 01, 01);
            var result = sut.ToSql();
            Assert.AreEqual("1990-01-01 00:00:00", result);
        }

        /// <summary>
        /// Verifies to SQL.
        /// </summary>
        [Test]
        public void Verify_ToOracleSql()
        {
            var sut = new DateTime(1990, 01, 01);
            var result = sut.ToOracleSql();
            Assert.AreEqual("to_date('01.01.1990 00:00:00','dd.mm.yyyy hh24.mi.ss')", result);
        }

        /// <summary>
        /// Verifies to unix.
        /// </summary>
        [Test]
        public void Verify_ToUnix()
        {
            var sut = new DateTime(1990, 01, 01);
            var result = sut.ToUnix();
            Assert.AreEqual(631152000, result);
        }

        /// <summary>
        /// Verifies from unix.
        /// </summary>
        [Test]
        public void Verify_FromUnix()
        {
            long sut = 631152000;
            var result = sut.FromUnix();
            Assert.AreEqual(1990, result.Year);
            Assert.AreEqual(1, result.Month);
            Assert.AreEqual(1, result.Day);
            Assert.AreEqual(0, result.Hour);
            Assert.AreEqual(0, result.Minute);
            Assert.AreEqual(0, result.Second);
        }

        /// <summary>
        /// Verifies the get age.
        /// </summary>
        [Test]
        public void Verify_GetAge()
        {
            var sut = DateTime.Today;
            var result = sut.GetAge();
            Assert.AreEqual(0, result);

            sut = new DateTime(1990, 01, 01);
            result = sut.GetAge();
            Assert.Greater(result, 18);

            sut = new DateTime(1990, 12, 01);
            result = sut.GetAge();
            Assert.Greater(result, 17);
        }

        /// <summary>
        /// Verifies to time zone.
        /// </summary>
        [Test]
        public void Verify_ToTimeZone()
        {
            var sut = new DateTimeOffset(1990, 1, 1, 12, 0, 0, new TimeSpan(0, 3, 0, 0, 0));
            DateTime result;
            try
            {
                result = sut.UtcDateTime.ToTimeZone("E. South America Standard Time");
            }
            catch 
            {
                try
                {
                    result = sut.UtcDateTime.ToTimeZone("America/Sao_Paulo");
                }
                catch
                {
                    result = sut.UtcDateTime;
                }
            }
            Assert.Greater(sut.UtcDateTime, result);
        }

        /// <summary>
        /// Verifies the is weekend.
        /// </summary>
        [Test]
        public void Verify_IsWeekend()
        {
            var sut = new DateTime(2019, 01, 05);
            var result = sut.IsWeekend();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies the is weekend.
        /// </summary>
        [Test]
        public void Verify_IsWeekday()
        {
            var sut = new DateTime(2019, 01, 04);
            var result = sut.IsWeekday();
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies the add workdays.
        /// </summary>
        [Test]
        public void Verify_AddWorkdays()
        {
            var sut = new DateTime(2019, 01, 04);
            var result = sut.AddWorkdays(15);
            Assert.AreEqual(new DateTime(2019, 01, 25), result);
        }

        /// <summary>
        /// Verifies the get last day of month.
        /// </summary>
        [Test]
        public void Verify_GetLastDayOfMonth()
        {
            var sut = new DateTime(2019, 01, 04);
            var result = sut.GetLastDayOfMonth();
            Assert.AreEqual(new DateTime(2019, 01, 31), result);
        }

        /// <summary>
        /// Verifies the get first day of month.
        /// </summary>
        [Test]
        public void Verify_GetFirstDayOfMonth()
        {
            var sut = new DateTime(2019, 01, 04);
            var result = sut.GetFirstDayOfMonth();
            Assert.AreEqual(new DateTime(2019, 01, 01), result);
        }

        /// <summary>
        /// Verifies the elapsed.
        /// </summary>
        [Test]
        public void Verify_Elapsed()
        {
            var sut = DateTime.Today;
            var result = sut.Elapsed();
            Assert.GreaterOrEqual(TimeSpan.FromDays(1), result);
        }
    }
}