using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.UnitTests
{
    enum TestEnum
    {
        Item1 = 0,

        [System.ComponentModel.Description("This is item 2")]
        Item2 = 1,

        Item3 = 2
    }

    [TestClass]
    public class Enum
    {
        [TestMethod]
        public void Can_get_description()
        {
            TestEnum testEnum = TestEnum.Item1;

            string description = EnumHelpers.GetDescription<TestEnum>(testEnum);

            Assert.AreEqual("Item1", description);
        }

        [TestMethod]
        public void Can_get_attribute_description()
        {
            TestEnum testEnum = TestEnum.Item2;

            string description = EnumHelpers.GetDescription<TestEnum>(testEnum);

            Assert.AreEqual("This is item 2", description);
        }

        [TestMethod]
        public void Can_get_enum_pairs()
        {
            var pairs = EnumHelpers.GetNameValues<TestEnum>().ToArray();

            Assert.IsNotNull(pairs);
            Assert.AreEqual(3, pairs.Length);

            Assert.AreEqual(TestEnum.Item1, pairs[0].Key);
            Assert.AreEqual("Item1", pairs[0].Value);

            Assert.AreEqual(TestEnum.Item2, pairs[1].Key);
            Assert.AreEqual("This is item 2", pairs[1].Value);

            Assert.AreEqual(TestEnum.Item3, pairs[2].Key);
            Assert.AreEqual("Item3", pairs[2].Value);
        }
    }
}
