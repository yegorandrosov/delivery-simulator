using DeliverySimulator.OrderEmitter.OrderProviders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.UnitTests.OrderEmitter
{
    [TestFixture]
    public class JsonFileOrderProviderTests
    {
        [Test]
        public void Constructor_ValidPath_ValidOrderJson_HasOrders()
        {
            var op = new JsonFileOrderProvider(TestContext.CurrentContext.TestDirectory + "\\test-orders.json");

            Assert.IsTrue(op.GetOrders().Count > 0);
        }
    }
}
