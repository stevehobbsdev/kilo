using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.Testing.Mvc
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Asserts that the controller has the specified key and value set into temp data
        /// </summary>
        /// <param name="controller">The controller to test</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void AssertHasTempData(this Controller controller, string key, object value)
        {
            Assert.IsNotNull(controller.TempData[key], "The TempData key " + key + " was not found");
            Assert.AreEqual(value, controller.TempData[key], "The value in TempData[" + key + "] does not match '" + value + "'");
        }
    }
}
