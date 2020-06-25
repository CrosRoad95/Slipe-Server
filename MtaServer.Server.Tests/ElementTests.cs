using FluentAssertions;
using MtaServer.Server.Elements;
using System;
using Xunit;

namespace MtaServer.Server.Tests
{
    public class ElementTests
    {
        [Fact]
        public void CreateTwoElements_CreatesUniqueElementIds()
        {
            var element1 = new Element();
            var element2 = new Element();

            element1.Id.Should().NotBe(element2.Id);
        }

        [Fact]
        public void GetAndIncrementTimeContext_ReturnsNewTimeContext()
        {
            var element = new Element();

            var context = element.TimeContext;
            var incrementContext = element.GetAndIncrementTimeContext();

            context.Should().NotBe(incrementContext);
        }

        [Fact]
        public void GetAndIncrementTimeContext_WrapsAroundSkippingZero()
        {
            var element = new Element();

            for (int i = 0; i < 256; i++)
            {
                element.GetAndIncrementTimeContext();
            }
            var context = element.TimeContext;

            context.Should().Be(1);
        }
    }
}
