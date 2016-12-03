using System.Web.Http;
using CarRentalAPI.Data;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace CarRentalAPI.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            builder.EntitySet<Booking>("Bookings");
            builder.EntitySet<Car>("Cars");
            builder.EntitySet<CarType>("CarTypes");
            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Rent>("Rents");
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
