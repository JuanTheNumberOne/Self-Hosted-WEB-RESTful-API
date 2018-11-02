//Self Host references
using System.Web.Http;
using Owin;

namespace Self_Host_Web_API_REST
{
    class ServerStart
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.Formatting =
                Newtonsoft.Json.Formatting.Indented;

            //Converts the enum value to its 
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add
                (new Newtonsoft.Json.Converters.StringEnumConverter());

            //Delete the formater, return only JSON data
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Set JSON as default for browsers
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            //Load configuration
            app.UseWebApi(config);
        }
    }
}
