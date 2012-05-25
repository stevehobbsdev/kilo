using System;
using System.Collections.Generic;
using Kilo.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.UnitTests
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void Can_build_dictionary_from_object()
        {
            DateTime now = DateTime.Now;

            var testObject = new { Name = "Test", Now = now };

            IDictionary<string, object> result = DictionaryBuilder.FromObject(testObject);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("Name"));
            Assert.IsTrue(result.ContainsKey("Now"));
            Assert.AreEqual("Test", result["Name"]);
            Assert.AreEqual(now, result["Now"]);
        }
    }
}
