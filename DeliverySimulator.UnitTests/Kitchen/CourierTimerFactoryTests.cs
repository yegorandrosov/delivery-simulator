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
    public class CourierTimerFactoryTests
    {
        [Test]
        public void Create_TimersHaveDifferentIntervals()
        {
            var factory = new CourierTimerFactory();
            var timer1 = factory.Create(new KitchenShelf(string.Empty, 10, 1), new ShelfOrder(new Order(), DateTime.Now));
            var timer2 = factory.Create(new KitchenShelf(string.Empty, 10, 1), new ShelfOrder(new Order(), DateTime.Now));
            var timer3 = factory.Create(new KitchenShelf(string.Empty, 10, 1), new ShelfOrder(new Order(), DateTime.Now));
            var timer4 = factory.Create(new KitchenShelf(string.Empty, 10, 1), new ShelfOrder(new Order(), DateTime.Now));

            Assert.IsTrue(
                timer1.Interval != timer2.Interval
                || timer1.Interval != timer3.Interval
                || timer1.Interval != timer4.Interval);
        }
    }
}
