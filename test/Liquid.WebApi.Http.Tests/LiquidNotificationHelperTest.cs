using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Implementations;
using Liquid.WebApi.Http.Interfaces;
using Liquid.WebApi.Http.UnitTests.Mocks;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Liquid.WebApi.Http.UnitTests
{
    public class LiquidNotificationHelperTest
    {
        private ILiquidNotificationHelper _sut;
        private ILiquidContextNotifications _contextNotifications = Substitute.For<ILiquidContextNotifications>();
        public LiquidNotificationHelperTest()
        {
            
        }

        [Fact]
        public void IncludeMessages_WhenNotificationContextHasMessages_ResponseHasMessagesList()
        {
            _contextNotifications.GetNotifications().Returns(new List<string>()
            {
                "test",
                "case"
            });

            _sut = new LiquidNotificationHelper(_contextNotifications);

            var responseObject = new TestCaseResponse("With Messages");

            object response = _sut.IncludeMessages(responseObject);

            var messages = response.GetType().GetProperty("messages").GetValue(response);

            Assert.NotNull(response.GetType().GetProperty("messages").GetValue(response));
        }

        [Fact]
        public void IncludeMessages_WhenNotificationContextHasNoMessages_ResponseWithoutMessages()
        {
            IList<string> notifications = default;

            _contextNotifications.GetNotifications().Returns(notifications);

            _sut = new LiquidNotificationHelper(_contextNotifications);

            var responseObject = new TestCaseResponse("With Messages");

            object response = _sut.IncludeMessages(responseObject);

            Assert.Null(response.GetType().GetProperty("messages")?.GetValue(response));
        }

    }
}
