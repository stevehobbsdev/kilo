using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kilo.Expressions;

namespace Kilo.UnitTests
{
    [TestClass]
    public class Expressions
    {

        class TestNestedObject
        {
            public string Name { get; set; }

            public TestNestedObject Nested { get; set; }
        }

        [TestMethod]
        public void Can_get_expression_path()
        {
            ExpressionParser<TestNestedObject, object> parser = new ExpressionParser<TestNestedObject, object>();

            string path = parser.GetPropertyPathFromExpression(t => t.Name);

            Assert.AreEqual("Name", path);
        }

        [TestMethod]
        public void Can_get_complex_expression_path()
        {
            ExpressionParser<TestNestedObject, object> parser = new ExpressionParser<TestNestedObject, object>();

            string path = parser.GetPropertyPathFromExpression(t => t.Nested.Nested.Name);

            Assert.AreEqual("Nested.Nested.Name", path);
        }
    }
}
