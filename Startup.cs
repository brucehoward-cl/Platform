using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Platform.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Platform
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITimeStamper, DefaultTimeStamper>();
            services.AddScoped<IResponseFormatter, TextResponseFormatter>();
            services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
            services.AddScoped<IResponseFormatter, GuidService>();

            #region MyRegion
            //services.AddScoped<IResponseFormatter>(serviceProvider =>
            //{
            //    string typeName = Configuration["services:IResponseFormatter"];
            //    return (IResponseFormatter)ActivatorUtilities.CreateInstance(serviceProvider, typeName == null
            //                                                            ? typeof(GuidService) : Type.GetType(typeName, true));
            //});
            //services.AddScoped<ITimeStamper, DefaultTimeStamper>(); 
            #endregion

            #region MyRegion
            ////services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
            ////services.AddTransient<IResponseFormatter, GuidService>();
            ////services.AddScoped<IResponseFormatter, GuidService>();
            //services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
            //services.AddScoped<ITimeStamper, DefaultTimeStamper>(); 
            #endregion
        }
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResponseFormatter formatter)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/single", async context => {
                    IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
                    await formatter.Format(context, "Single service");
                });
                endpoints.MapGet("/", async context => {
                    IResponseFormatter formatter = context.RequestServices.GetServices<IResponseFormatter>().First(f => f.RichOutput);
                    await formatter.Format(context, "Multiple services");
                });
            });

            #region MyRegion
            //app.UseDeveloperExceptionPage();
            //app.UseRouting();
            //app.UseMiddleware<WeatherMiddleware>();
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/middleware/function")
            //    {
            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
            //        //                    await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
            //        //await TextResponseFormatter.Singleton.Format(context, "Middleware Function: It is snowing in Chicago");
            //        //await TypeBroker.Formatter.Format(context, "Middleware Function: It is snowing in Chicago");
            //        await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
            //        //IResponseFormatter formatter = app.ApplicationServices.GetService<IResponseFormatter>();
            //    }
            //    else
            //    {
            //        await next();
            //    }
            //});
            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);
            //    endpoints.MapEndpoint<WeatherEndpoint>("/endpoint/class");
            //    //endpoints.MapWeather("/endpoint/class");
            //    endpoints.MapGet("/endpoint/function", async context =>
            //    {
            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
            //        //await context.Response.WriteAsync("Endpoint Function: It is sunny in LA");
            //        //await TextResponseFormatter.Singleton.Format(context,"Endpoint Function: It is sunny in LA");
            //        //await TypeBroker.Formatter.Format(context,"Endpoint Function: It is sunny in LA");
            //        await formatter.Format(context, "Endpoint Function: It is sunny in LA");
            //        //IResponseFormatter formatter = app.ApplicationServices.GetService<IResponseFormatter>();
            //    });
            //}); 
            #endregion
        }
    }

    #region MyRegion
    //public class Startup
    //{
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.Configure<RouteOptions>(opts =>
    //        {
    //            opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
    //        });
    //    }

    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        app.UseDeveloperExceptionPage();
    //        app.UseRouting();
    //        app.Use(async (context, next) =>
    //        {
    //            Endpoint end = context.GetEndpoint();
    //            if (end != null)
    //            {
    //                await context.Response.WriteAsync($"{end.DisplayName} Selected \n");
    //            }
    //            else
    //            {
    //                await context.Response.WriteAsync("No Endpoint Selected \n");
    //            }
    //            await next();
    //        });

    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.Map("{number:int}", async context =>
    //            {
    //                await context.Response.WriteAsync("Routed to the int endpoint");
    //            })
    //                    .WithDisplayName("Int Endpoint")
    //                    .Add(b => ((RouteEndpointBuilder)b).Order = 1);

    //            endpoints.Map("{number:double}", async context =>
    //            {
    //                await context.Response.WriteAsync("Routed to the double endpoint");
    //            })
    //                    .WithDisplayName("Double Endpoint").Add(b => ((RouteEndpointBuilder)b).Order = 2);
    //        });

    //        app.Use(async (context, next) =>
    //        {
    //            await context.Response.WriteAsync("Terminal Middleware Reached");
    //        });
    //    }
    //}

    #endregion

    #region MyRegion
    //public class Startup
    //{
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.Configure<RouteOptions>(opts =>
    //        {
    //            opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
    //        });
    //    }
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        app.UseDeveloperExceptionPage();
    //        //app.UseMiddleware<Population>();
    //        //app.UseMiddleware<Capital>();

    //        app.UseRouting();
    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapGet("{first:alpha:length(3)}/{second:bool}", async context =>
    //            {
    //                await context.Response.WriteAsync("Request Was Routed\n");
    //                foreach (var kvp in context.Request.RouteValues)
    //                {
    //                    await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
    //                }
    //            });

    //            endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);

    //            endpoints.MapGet("capital/{country:regex(^uk|france|monaco$)}", Capital.Endpoint);
    //            endpoints.MapGet("size/{city?}", Population.Endpoint)
    //                .WithMetadata(new RouteNameMetadata("population"));

    //            endpoints.MapFallback(async context =>
    //            {
    //                await context.Response.WriteAsync("Routed to fallback endpoint");
    //            });
    //        });

    //        app.Use(async (context, next) =>
    //        {
    //            await context.Response.WriteAsync("Terminal Middleware Reached");
    //        });
    //    }
    //} 
    #endregion
}

#region Old code prior to Listing 13-3

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;

//namespace Platform
//{
//    public class Startup
//    {
//        // This method gets called by the runtime. Use this method to add services to the container.
//        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.Configure<MessageOptions>(options => { options.CityName = "Albany"; });
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
////        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> msgOptions)
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseMiddleware<LocationMiddleware>();

//            #region msgOptions
//            //app.Use(async (context, next) =>
//            //{
//            //    if (context.Request.Path == "/location")
//            //    {
//            //        MessageOptions opts = msgOptions.Value;
//            //        await context.Response.WriteAsync($"{opts.CityName}, {opts.CountryName}");
//            //    }
//            //    else
//            //    {
//            //        await next();
//            //    }
//            //});
//            #endregion

//            #region /branch
//            //app.Map("/branch", branch =>
//            //{
//            //    branch.UseMiddleware<QueryStringMiddleWare>();
//            //    branch.Use(async (context, next) =>
//            //    {
//            //        await context.Response.WriteAsync($"Branch Middleware");
//            //    });
//            //});
//            #endregion

//            #region app.Use status code
//            //app.Use(async (context, next) =>
//            //{
//            //    await next();
//            //    await context.Response.WriteAsync($"\nStatus Code: { context.Response.StatusCode}");
//            //});
//            #endregion

//            #region app.Use /short
//            //app.Use(async (context, next) =>
//            //{
//            //    if (context.Request.Path == "/short")
//            //    {
//            //        await context.Response.WriteAsync($"Request Short Circuited");
//            //    }
//            //    else
//            //    {
//            //        await next();
//            //    }
//            //});
//            #endregion

//            #region app.Use Custom Middleware
//            //app.Use(async (context, next) =>
//            //{
//            //    if (context.Request.Method == HttpMethods.Get
//            //    && context.Request.Query["custom"] == "true")
//            //    {
//            //        await context.Response.WriteAsync("Custom Middleware \n");
//            //    }
//            //    await next();
//            //});
//            #endregion

//            //app.Map("/branch", branch => {
//            //    branch.Run(new QueryStringMiddleWare().Invoke);
//            //});

//            //app.UseMiddleware<QueryStringMiddleWare>();

//            app.UseRouting();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapGet("/", async context =>
//                {
//                    await context.Response.WriteAsync("Hello World!");
//                });
//            });
//        }
//    }
//}
// /**/

#endregion