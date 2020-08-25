using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;
//using Microsoft.Extensions.DependencyInjection;

namespace Platform
{
    public class WeatherEndpoint
    {
        //private IResponseFormatter formatter;
        //public WeatherEndpoint(IResponseFormatter responseFormatter)
        //{
        //    formatter = responseFormatter;
        //}
        //public async Task Endpoint(HttpContext context)        
        public async Task Endpoint(HttpContext context, IResponseFormatter formatter)
        {
            await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        }

        #region MyRegion
        //public static async Task Endpoint(HttpContext context, IResponseFormatter formatter)
        //{
        //    await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        //}
        #endregion    
    }

        #region MyRegion
        //public class WeatherEndpoint
        //{
        //    public static async Task Endpoint(HttpContext context)
        //    {
        //        IResponseFormatter formatter = context.RequestServices.GetRequiredService<IResponseFormatter>();
        //        await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        //        //            await context.Response.WriteAsync("Endpoint Class: It is cloudy in Milan");
        //    }
        //}
        #endregion

    }