using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Kitchen.Shelves.ShelfTimers;
using DeliverySimulator.Shared.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.UnitTests.Kitchen
{
    [TestFixture]
    public class OrderDeterriorationTimerFactoryTests
    {
        [Test]
        public void Create_IntervalChangesAccordingToProductParams()
        {
            var factory = new OrderDeterriorationTimerFactory();
            var kitchenShelf = new KitchenShelf("hot", 10, 1);
            var shelfOrder = new ShelfOrder(new Order()
            {
                ShelfLife = 300,
                DecayRate = 0.5
            }, DateTime.Now);

            var timer1 = factory.Create(kitchenShelf, shelfOrder);

            var shelfOrderWithLowerShelfLife = new ShelfOrder(new Order()
            {
                ShelfLife = 100,
                DecayRate = 0.5,
            }, DateTime.Now);

            var timer2 = factory.Create(kitchenShelf, shelfOrderWithLowerShelfLife);

            var shelfOrderWithHigherDecayRate = new ShelfOrder(new Order()
            {
                ShelfLife = 300,
                DecayRate = 1,
            }, DateTime.Now);

            var timer3 = factory.Create(kitchenShelf, shelfOrderWithHigherDecayRate);

            Assert.IsTrue(timer1.Interval > timer2.Interval);
            Assert.IsTrue(timer1.Interval > timer3.Interval);
        }
    }
}
