using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;

namespace Platform
{
    public class WeatherMiddleware
    {
        private RequestDelegate next;
        //private IResponseFormatter formatter;

        //public WeatherMiddleware(RequestDelegate nextDelegate, IResponseFormatter respFormatter)
        public WeatherMiddleware(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
            //formatter = respFormatter;
        }

        //public async Task Invoke(HttpContext context)
        #region MyRegion
        //public async Task Invoke(HttpContext context, IResponseFormatter formatter)
        //{
        //    if (context.Request.Path == "/middleware/class")
        //    {
        //        await formatter.Format(context, "Middleware Class: It is raining in London");
        //    }
        //    else
        //    {
        //        await next(context);
        //    }
        //}

        #endregion    

        public async Task Invoke(HttpContext context, IResponseFormatter formatter1,IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if (context.Request.Path == "/middleware/class")
            {
                await formatter1.Format(context, string.Empty);
                await formatter2.Format(context, string.Empty);
                await formatter3.Format(context, string.Empty);
            }
            else
            {
                await next(context);
            }
        }
    }
}