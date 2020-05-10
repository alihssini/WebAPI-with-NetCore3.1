using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Data.Repository;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainService;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Ali.Hosseini.Application.Domain.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace Ali.Hosseini.Application.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; protected set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Swagger
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Applicant API",
                    Description = "A simple example ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Ali Hosseini",
                        Email = "ali.hssini@gmail.com",
                        Url = new Uri("https://github.com/alihssini"),
                    }
                });
                c.OperationFilter<AddHeaderOperationFilter>("Accept-Language", "Enter en-US/de-DE/it-IT for switch language...");//Add Header for demonstrate Swagger Filtes and use it for basic lang switch ;-)
                                                                                                                                 //Determine base path for the application.
                //Set the comments path for the swagger json and ui.
                c.IncludeXmlComments("Ali.Hosseini.Application.Api.xml");

            });
            //Log
            ConfigLog();
            //DbContext
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: Configuration.GetConnectionString("EFInMemDbName")));
            //Repo
            services.AddTransient<IApplicantRepository, ApplicantRepository>();
            //Svc
            services.AddTransient<IApplicantDomainService, ApplicantDomainService>();
            //Validation
            services.AddTransient<IValidator<Applicant>, ApplicantValidation>();
            services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            //Localization
            services.AddJsonLocalization(opts => { opts.ResourcesPath = "Resources"; });

            //Retry and Fallback policies
            var policyWrap = Policy.WrapAsync(FallbackPolicy(), GetRetryPolicy());
            services.AddHttpClient("HttpClient").AddPolicyHandler(policyWrap);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]
               {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("it-IT")
               };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var cultureInfo = new CultureInfo("de-DE");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture(culture: cultureInfo, uiCulture: cultureInfo)
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Applicant V1");
                });
            }
#if Debug
            app.UseHttpsRedirection();
#endif
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        #region Log Configuration
        private void ConfigLog()
        {
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(Configuration)
                         .CreateLogger();
        }
        #endregion
        #region Retry Policies
        /// <summary>
        /// Configuration for Webservice resiliency and policies such as Retry, Circuit Breaker, Timeout, and Fallback in Exceptions
        /// </summary>
        /// <returns></returns>
        protected static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              // What to do if any of the above erros occur:
              // Retry 3 times, each time wait 0.5, 1 and 1.5 seconds before retrying.
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt * 0.5));
        }
        #endregion
        #region Fallback Policies
        /// <summary>
        /// Configuration for Fallback policies and return default response for handle Exceptions without exception in our system
        /// </summary>
        /// <returns></returns>
        protected static IAsyncPolicy<HttpResponseMessage> FallbackPolicy()
        {
            return Policy
                    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != System.Net.HttpStatusCode.NotFound)//Fallback in any status except 200 and 404[not found status for country]
                    .FallbackAsync(FallbackAction, OnFallbackAsync);
        }

        protected static Task OnFallbackAsync(DelegateResult<HttpResponseMessage> arg)
        {
            Log.Logger.Error("Country webservice is down");
            return Task.CompletedTask;
        }

        protected static Task<HttpResponseMessage> FallbackAction(CancellationToken arg)
        {
            Log.Logger.Error("Fallback action is executing");
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StringContent($"The fallback executed!")
                ,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
            return Task.FromResult(httpResponseMessage);
        }
        #endregion
    }
}
