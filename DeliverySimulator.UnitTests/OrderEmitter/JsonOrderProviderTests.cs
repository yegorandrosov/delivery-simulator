using DeliverySimulator.OrderEmitter.OrderProviders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.UnitTests.OrderEmitter
{
    [TestFixture]
    public class JsonOrderProviderTests
    {
        [Test]
        public void Constructor_EmptyParameter()
        {
            Assert.Catch(typeof(ArgumentNullException), () =>
            {
                new JsonOrderProvider(null);
            });
        }

        [Test]
        public void Constructor_ParameterInvalidOrdersJSON()
        {
            Assert.Catch(() =>
            {
                new JsonOrderProvider("{}");
            });
        }

        [Test]
        public void Constructor_ValidJSON_SingleOrder()
        {
            var json = @"
[
  {
    ""id"": ""7f682ade-8375-4ef4-aa9b-45440f361268"",
    ""name"": ""Kebab"",
    ""temp"": ""hot"",
    ""shelfLife"": 200,
    ""decayRate"": 0.54
  }
]
";

            var op = new JsonOrderProvider(json);
            var orders = op.GetOrders();
            Assert.AreEqual(1, orders.Count);

            Assert.AreNotEqual(Guid.Empty, orders[0].Id);
            Assert.AreEqual("Kebab", orders[0].Name);
            Assert.AreEqual("hot", orders[0].Temp);
            Assert.AreEqual(200, orders[0].ShelfLife);
            Assert.AreEqual(0.54, orders[0].DecayRate);
        }
    }
}
