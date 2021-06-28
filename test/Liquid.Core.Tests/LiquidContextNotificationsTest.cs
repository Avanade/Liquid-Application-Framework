using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Liquid.Core.UnitTests
{
    public class LiquidContextNotificationsTest
    {
        private ILiquidContextNotifications _sut;

        public LiquidContextNotificationsTest()
        {
            _sut = new LiquidContextNotifications(new LiquidContext());
        }

        public void InitializeNotifications()
        {
            _sut.UpsertNotification("test", "initialize notifications");
        }

        [Fact]
        public void InsertNotificaton_WhenContextHasNoNotifications_Inserted()
        {
            _sut.UpsertNotification("case1", "test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count >= 1);
            Assert.True(result["case1"].ToString() == "test case");
        }

        [Fact]
        public void InsertNotification_WhenContextHasNotifications_Inserted()
        {
            InitializeNotifications();

            _sut.UpsertNotification("case2", "test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count > 1);
            Assert.True(result["case2"].ToString() == "test case");
        }

        [Fact]
        public void UpdateNotificaton_WhenNotificationKeyDoesntExist_Inserted()
        {
            _sut.UpsertNotification("case3", "test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count == 1);
            Assert.True(result["case3"].ToString() == "test case");
        }

        [Fact]
        public void UpdateNotification_WhenNotificationKeyExists_Updated()
        {
            InitializeNotifications();

            _sut.UpsertNotification("test", "test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count == 1);
            Assert.True(result["test"].ToString() == "test case");
        }

        [Fact]
        public void GetNotifications_WhenContexthasNone_ReturnNull()
        {
            var result = _sut.GetNotifications();

            Assert.Null(result);
        }

        [Fact]
        public void GetNotifications_WhenContexthasNotifications_ReturnNotifications()
        {
            InitializeNotifications();

            var result = _sut.GetNotifications();

            Assert.True(result.Count >= 1);
            Assert.NotNull(result);
        }

    }
}
