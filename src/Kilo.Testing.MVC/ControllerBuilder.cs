using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Kilo.Testing.Mvc
{
    public class ControllerBuilder<T> : TestSubjectBuilder<ControllerBuilder<T>, T>
        where T : Controller
    {
        private UrlHelper _urlHelper;
        private RouteCollection _routes;

        /// <summary>
        /// Gets the context.
        /// </summary>
        public Mock<HttpContextBase> Context { get; private set; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public Mock<HttpRequestBase> Request { get; private set; }

        /// <summary>
        /// Gets the response.
        /// </summary>
        public Mock<HttpResponseBase> Response { get; private set; }

        /// <summary>
        /// Gets the model state dictionary
        /// </summary>
        public Mock<ModelStateDictionary> ModelState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerConfigurator{T}" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public ControllerBuilder()
        {
            this.Request = new Mock<HttpRequestBase>();
            this.Response = new Mock<HttpResponseBase>();

            this.Context = new Mock<HttpContextBase>();
            this.Context.Setup(r => r.Request).Returns(this.Request.Object);
            this.Context.Setup(r => r.Response).Returns(this.Response.Object);

            this.Response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string url) => url);

            _routes = new RouteCollection();
        }

        /// <summary>
        /// Sets up the controller with a default route.
        /// </summary>
        public ControllerBuilder<T> WithDefaultRoutes()
        {
            _routes.MapRoute(
                    "Area",
                    "{area}/{controller}/{action}/{nodeGuid}",
                    new { controller = "Home", action = "Index", nodeGuid = UrlParameter.Optional },
                    new string[] { "Orchidnet.Web.Controllers" });

            _routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{nodeGuid}", // URL with parameters
                new { controller = "Home", action = "Index", nodeGuid = UrlParameter.Optional }, // Parameter defaults
                new string[] { "Orchidnet.Web.Controllers" }
            );

            return this;
        }

        /// <summary>
        /// Adds a route to the routes table
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        public ControllerBuilder<T> WithRoute(string url, object defaults = null)
        {
            var defaultsDictionary = new RouteValueDictionary(defaults);
            var route = new Route(url, defaultsDictionary, new MvcRouteHandler());

            _routes.Insert(0, route);

            return this;
        }

        /// <summary>
        /// Setups up the AppPathModifer function to return the output string, when given the input string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="output">The output string</param>
        public ControllerBuilder<T> WithAppPathModifier(string input, string output)
        {
            this.Response.Setup(x => x.ApplyAppPathModifier(input)).Returns(output);
            return this;
        }

        /// <summary>
        /// Sets up the controller with a Url Builder instance
        /// </summary>
        public ControllerBuilder<T> WithUrlHelper()
        {
            _urlHelper = new UrlHelper(new RequestContext(this.Context.Object, new RouteData()), _routes);
            return this;
        }

        /// <summary>
        /// Sets up the controller to return the specified user identity
        /// </summary>
        /// <param name="userIdentity">The user identity</param>
        public ControllerBuilder<T> WithIdentity(IIdentity userIdentity, string [] roles = null)
        {
            this.Context.SetupGet(c => c.User).Returns(new GenericPrincipal(userIdentity, roles));
            return this;
        }

        /// <summary>
        /// Sets the collection which should be returned when the headers are queried
        /// </summary>
        /// <param name="headers">The headers</param>
        public ControllerBuilder<T> WithHeaders(NameValueCollection headers)
        {
            this.Response.Setup(r => r.Headers).Returns(headers);
            return this;
        }

        /// <summary>
        /// Sets up the controller as the final step in the process.
        /// </summary>
        public override T Build()
        {
 	        var controller = base.Build();

            SetupController(controller);

            return controller;
        }

        /// <summary>
        /// Builds a mock of the controller.
        /// </summary>
        public override Mock<T> BuildMock()
        {
            var mock = base.BuildMock();
            mock.CallBase = true;

            SetupController(mock.Object);

            mock.CallBase = false;

            return mock;
        }

        /// <summary>
        /// Setups the controller with all of the properties which are needed.
        /// </summary>
        /// <param name="controller">The controller.</param>
        private void SetupController(T controller)
        {
            controller.ControllerContext = new ControllerContext(this.Context.Object, new RouteData(), controller);
            controller.Url = this._urlHelper;
        }
    }
}
