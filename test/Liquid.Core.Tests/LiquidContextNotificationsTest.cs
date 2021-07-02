using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
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

        private void InitializeNotifications()
        {
            _sut.InsertNotification("initialize notifications");
        }

        [Fact]
        public void InsertNotificaton_WhenContextHasNoNotifications_Inserted()
        {
            _sut.InsertNotification("test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count == 1);
            Assert.True(result.Contains("test case"));
        }

        [Fact]
        public void InsertNotification_WhenContextHasNotifications_Inserted()
        {
            InitializeNotifications();

            _sut.InsertNotification("test case 2");

            var result = _sut.GetNotifications();

            Assert.True(result.Count > 1);
            Assert.True(result.Contains("test case 2"));
        }

        [Fact]
        public void UpdateNotification_WhenNotificationTextAlredyExists_Inserted()
        {
            InitializeNotifications();

            _sut.InsertNotification("test case");

            var result = _sut.GetNotifications();

            Assert.True(result.Count > 1);
            Assert.True(result.Contains("test case"));
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
