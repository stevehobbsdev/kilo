using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.Testing.Mvc
{
    public static class RedirectResultExtensions
    {
        /// <summary>
        /// Verifies that the result has redirected to the route dictated by the action, controller and area parameters.
        /// </summary>
        /// <param name="result">The result to verify.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="area">The area.</param>
        public static void VerifyRoute(this RedirectToRouteResult result, string action, string controller = null, string area = null)
        {
            Assert.AreEqual(action, result.RouteValues["action"],
                string.Format("The route action parameter '{0}' does not match '{1}' - '{2}' was found instead", "action", action, result.RouteValues["action"]));

            Assert.AreEqual(controller, result.RouteValues["controller"],
                string.Format("The route action parameter '{0}' does not match '{1}' - '{2}' was found instead", "controller", controller, result.RouteValues["controller"]));

            Assert.AreEqual(area, result.RouteValues["area"],
                string.Format("The route action parameter '{0}' does not match '{1}' - '{2}' was found instead", "area", area, result.RouteValues["area"]));
        }
    }
}
