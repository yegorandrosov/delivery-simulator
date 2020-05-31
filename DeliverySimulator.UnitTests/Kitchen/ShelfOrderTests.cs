using DeliverySimulator.Kitchen.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.UnitTests.Kitchen
{
    [TestFixture]
    public class ShelfOrderTests
    {

        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/1/1 14:01:00", true })]
        [TestCase(new object[] { "2000/5/3 10:05:10", "2000/5/3 10:05:40", true })]
        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/2/1 14:10:00", false })]
        [TestCase(new object[] { "2000/5/3 9:00:10", "2000/5/3 10:05:40", false })]
        public void GetValue_Order1(string addedAtStr, string currentTimeStr, bool expectedValueGreaterThanZero)
        {
            var addedAt = DateTime.Parse(addedAtStr);
            var currentTime = DateTime.Parse(currentTimeStr);

            var shelfOrder = new ShelfOrder(new Shared.Models.Order()
            {
                DecayRate = 0.5,
                ShelfLife = 100,
            }, addedAt);

            var value = shelfOrder.GetValue(currentTime, 1);
            if (expectedValueGreaterThanZero)
            {
                Assert.Greater(value, 0);
                Assert.Less(value, 1);
            }
            else
            {
                Assert.LessOrEqual(value, 0);
            }
        }

        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/1/1 14:01:00", 0.5, 400, 1, 0.9375 })]
        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/1/1 14:01:00", 0.4, 320, 2, 0.875 })]
        public void GetValue_ValueMatchesExpected(string addedAtStr, string currentTimeStr, double decayRate, int shelfLife, int shelfDecay, double expected)
        {
            var addedAt = DateTime.Parse(addedAtStr);
            var currentTime = DateTime.Parse(currentTimeStr);

            var shelfOrder = new ShelfOrder(new Shared.Models.Order()
            {
                DecayRate = decayRate,
                ShelfLife = shelfLife,
            }, addedAt);

            var value = shelfOrder.GetValue(currentTime, shelfDecay);

            Assert.AreEqual(expected, value);
        }

        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/1/1 14:01:00", 0.5, 400, 1, 800 })]
        [TestCase(new object[] { "1970/1/1 14:00:10", "1970/1/1 14:01:00", 0.4, 320, 2, 400 })]
        public void GetMaxAge_ValueMatchesExpected(string addedAtStr, string currentTimeStr, double decayRate, int shelfLife, int shelfDecay, double expected)
        {
            var addedAt = DateTime.Parse(addedAtStr);
            var currentTime = DateTime.Parse(currentTimeStr);

            var shelfOrder = new ShelfOrder(new Shared.Models.Order()
            {
                DecayRate = decayRate,
                ShelfLife = shelfLife,
            }, addedAt);

            var maxAge = shelfOrder.GetMaxOrderAge(shelfDecay);

            Assert.AreEqual(expected, maxAge);
        }
    }
}
