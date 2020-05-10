using Ali.Hosseini.Application.Api;
using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Data.Repository;
using Ali.Hosseini.Application.Domain;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainService;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Ali.Hosseini.Application.Domain.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Ali.Hosseini.Application.Tests.Core
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            //DbContext
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: Configuration.GetConnectionString("EFInMemDbName")));
            //Repo
            services.AddTransient<IApplicantRepository, ApplicantRepository>();
            //Svc
            services.AddTransient<IApplicantDomainService, ApplicantDomainService>();
            //Validation
            services.AddTransient<IValidator<Applicant>, ApplicantValidation>();
            services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());//Todo check Duplicate with upper lone

            //Localization
            services.AddJsonLocalization(opts => { opts.ResourcesPath = "Resources"; });

            //Retry and Fallback policies
            var policyWrap = Policy.WrapAsync(FallbackPolicy(), GetRetryPolicy());
            services.AddHttpClient("HttpClient").AddPolicyHandler(policyWrap);
            //ServiceProvider
            ServiceProviderHandler.Initialize(services.BuildServiceProvider());
        }
    }
}
