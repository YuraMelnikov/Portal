using System.Web.Http;
using WebActivatorEx;
using Wiki;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Wiki
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Wiki");
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
