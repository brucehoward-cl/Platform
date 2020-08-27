using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;

namespace Platform
{
    public class Startup
    {
        public Startup(IConfiguration configService)
        {
            Configuration = configService; //The configuration service is instantiated BEFORE Startup is instantiated
        }
        private IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MessageOptions>(Configuration.GetSection("Location")); //replaces the default values
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider($"{env.ContentRootPath}/staticfiles"), RequestPath = "/files"
            });

            app.UseRouting();
            app.UseMiddleware<LocationMiddleware>();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    logger.LogDebug("Response for / started");
                    await context.Response.WriteAsync("Hello World!");
                    logger.LogDebug("Response for / completed");
                });
            });
        }

        #region Through Listing 15-19
        ////        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration config)
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    app.UseRouting();

        //    app.UseMiddleware<LocationMiddleware>();

        //    app.Use(async (context, next) =>
        //    {
        //        //string defaultDebug = config["Logging:LogLevel:Default"]; //This is used (without the need for a Configuration property) if only using it to configure middleware
        //        string defaultDebug = Configuration["Logging:LogLevel:Default"];
        //        await context.Response.WriteAsync($"The config setting is: {defaultDebug}");
        //        string environ = Configuration["ASPNETCORE_ENVIRONMENT"];
        //        await context.Response.WriteAsync($"\nThe env setting is: {environ}");
        //        string wsID = Configuration["WebService:Id"];
        //        string wsKey = Configuration["WebService:Key"];
        //        await context.Response.WriteAsync($"\nThe secret ID is: {wsID}");
        //        await context.Response.WriteAsync($"\nThe secret Key is: {wsKey}");
        //    });
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapGet("/", async context =>
        //        {
        //            await context.Response.WriteAsync("Hello World!");
        //        });
        //    });
        //} 
        #endregion
    }
}

#region Chapter 14
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Routing;
//using Platform.Services;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace Platform
//{
//    public class Startup
//    {
//        public Startup(IConfiguration config)
//        {
//            Configuration = config;
//        }
//        private IConfiguration Configuration;

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSingleton(typeof(ICollection<>), typeof(List<>));

//            #region MyRegion
//            //services.AddScoped<ITimeStamper, DefaultTimeStamper>();
//            //services.AddScoped<IResponseFormatter, TextResponseFormatter>();
//            //services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
//            //services.AddScoped<IResponseFormatter, GuidService>(); 
//            #endregion

//            #region MyRegion
//            //services.AddScoped<IResponseFormatter>(serviceProvider =>
//            //{
//            //    string typeName = Configuration["services:IResponseFormatter"];
//            //    return (IResponseFormatter)ActivatorUtilities.CreateInstance(serviceProvider, typeName == null
//            //                                                            ? typeof(GuidService) : Type.GetType(typeName, true));
//            //});
//            //services.AddScoped<ITimeStamper, DefaultTimeStamper>(); 
//            #endregion

//            #region MyRegion
//            ////services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
//            ////services.AddTransient<IResponseFormatter, GuidService>();
//            ////services.AddScoped<IResponseFormatter, GuidService>();
//            //services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
//            //services.AddScoped<ITimeStamper, DefaultTimeStamper>(); 
//            #endregion
//        }
//        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResponseFormatter formatter)
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            app.UseDeveloperExceptionPage();
//            app.UseRouting();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapGet("/string", async context =>
//                {
//                    ICollection<string> collection = context.RequestServices.GetService<ICollection<string>>();
//                    collection.Add($"Request: { DateTime.Now.ToLongTimeString() }");
//                    foreach (string str in collection)
//                    {
//                        await context.Response.WriteAsync($"String: {str}\n");
//                    }
//                });
//                endpoints.MapGet("/int", async context =>
//                {
//                    ICollection<int> collection = context.RequestServices.GetService<ICollection<int>>();
//                    collection.Add(collection.Count() + 1);
//                    foreach (int val in collection)
//                    {
//                        await context.Response.WriteAsync($"Int: {val}\n");
//                    }
//                });
//            });

//            #region MyRegion
//            //app.UseDeveloperExceptionPage();
//            //app.UseRouting();
//            //app.UseEndpoints(endpoints =>
//            //{
//            //    endpoints.MapGet("/single", async context =>
//            //    {
//            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
//            //        await formatter.Format(context, "Single service");
//            //    });
//            //    endpoints.MapGet("/", async context =>
//            //    {
//            //        IResponseFormatter formatter = context.RequestServices.GetServices<IResponseFormatter>().First(f => f.RichOutput);
//            //        await formatter.Format(context, "Multiple services");
//            //    });
//            //}); 
//            #endregion

//            #region MyRegion
//            //app.UseDeveloperExceptionPage();
//            //app.UseRouting();
//            //app.UseMiddleware<WeatherMiddleware>();
//            //app.Use(async (context, next) =>
//            //{
//            //    if (context.Request.Path == "/middleware/function")
//            //    {
//            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
//            //        //                    await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
//            //        //await TextResponseFormatter.Singleton.Format(context, "Middleware Function: It is snowing in Chicago");
//            //        //await TypeBroker.Formatter.Format(context, "Middleware Function: It is snowing in Chicago");
//            //        await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
//            //        //IResponseFormatter formatter = app.ApplicationServices.GetService<IResponseFormatter>();
//            //    }
//            //    else
//            //    {
//            //        await next();
//            //    }
//            //});
//            //app.UseEndpoints(endpoints =>
//            //{
//            //    //endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);
//            //    endpoints.MapEndpoint<WeatherEndpoint>("/endpoint/class");
//            //    //endpoints.MapWeather("/endpoint/class");
//            //    endpoints.MapGet("/endpoint/function", async context =>
//            //    {
//            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
//            //        //await context.Response.WriteAsync("Endpoint Function: It is sunny in LA");
//            //        //await TextResponseFormatter.Singleton.Format(context,"Endpoint Function: It is sunny in LA");
//            //        //await TypeBroker.Formatter.Format(context,"Endpoint Function: It is sunny in LA");
//            //        await formatter.Format(context, "Endpoint Function: It is sunny in LA");
//            //        //IResponseFormatter formatter = app.ApplicationServices.GetService<IResponseFormatter>();
//            //    });
//            //}); 
//            #endregion
//        }
//    }

//    #region MyRegion
//    //public class Startup
//    //{
//    //    public void ConfigureServices(IServiceCollection services)
//    //    {
//    //        services.Configure<RouteOptions>(opts =>
//    //        {
//    //            opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
//    //        });
//    //    }

//    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    //    {
//    //        app.UseDeveloperExceptionPage();
//    //        app.UseRouting();
//    //        app.Use(async (context, next) =>
//    //        {
//    //            Endpoint end = context.GetEndpoint();
//    //            if (end != null)
//    //            {
//    //                await context.Response.WriteAsync($"{end.DisplayName} Selected \n");
//    //            }
//    //            else
//    //            {
//    //                await context.Response.WriteAsync("No Endpoint Selected \n");
//    //            }
//    //            await next();
//    //        });

//    //        app.UseEndpoints(endpoints =>
//    //        {
//    //            endpoints.Map("{number:int}", async context =>
//    //            {
//    //                await context.Response.WriteAsync("Routed to the int endpoint");
//    //            })
//    //                    .WithDisplayName("Int Endpoint")
//    //                    .Add(b => ((RouteEndpointBuilder)b).Order = 1);

//    //            endpoints.Map("{number:double}", async context =>
//    //            {
//    //                await context.Response.WriteAsync("Routed to the double endpoint");
//    //            })
//    //                    .WithDisplayName("Double Endpoint").Add(b => ((RouteEndpointBuilder)b).Order = 2);
//    //        });

//    //        app.Use(async (context, next) =>
//    //        {
//    //            await context.Response.WriteAsync("Terminal Middleware Reached");
//    //        });
//    //    }
//    //}

//    #endregion

//    #region MyRegion
//    //public class Startup
//    //{
//    //    public void ConfigureServices(IServiceCollection services)
//    //    {
//    //        services.Configure<RouteOptions>(opts =>
//    //        {
//    //            opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
//    //        });
//    //    }
//    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    //    {
//    //        app.UseDeveloperExceptionPage();
//    //        //app.UseMiddleware<Population>();
//    //        //app.UseMiddleware<Capital>();

//    //        app.UseRouting();
//    //        app.UseEndpoints(endpoints =>
//    //        {
//    //            endpoints.MapGet("{first:alpha:length(3)}/{second:bool}", async context =>
//    //            {
//    //                await context.Response.WriteAsync("Request Was Routed\n");
//    //                foreach (var kvp in context.Request.RouteValues)
//    //                {
//    //                    await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
//    //                }
//    //            });

//    //            endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);

//    //            endpoints.MapGet("capital/{country:regex(^uk|france|monaco$)}", Capital.Endpoint);
//    //            endpoints.MapGet("size/{city?}", Population.Endpoint)
//    //                .WithMetadata(new RouteNameMetadata("population"));

//    //            endpoints.MapFallback(async context =>
//    //            {
//    //                await context.Response.WriteAsync("Routed to fallback endpoint");
//    //            });
//    //        });

//    //        app.Use(async (context, next) =>
//    //        {
//    //            await context.Response.WriteAsync("Terminal Middleware Reached");
//    //        });
//    //    }
//    //} 
//    #endregion
//} 
#endregion

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