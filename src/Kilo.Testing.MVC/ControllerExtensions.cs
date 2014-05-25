using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.Testing.Mvc
{
    public static class ControllerExtensions
    {
        public static void HasTempData(this Controller controller, string key, string value)
        {
            Assert.IsNotNull(controller.TempData[key], "The TempData key " + key + " was not found");
            Assert.AreEqual(value, controller.TempData[key], "The value in TempData[" + key + "] does not match '" + value + "'");
        }
    }

}
