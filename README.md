![.NET Core](https://github.com/alihssini/WebAPI-with-NetCore3.1/workflows/.NET%20Core/badge.svg)
# WebAPI-with-NetCore3.1

A simple CRUD WebAPI with netcore 3.1

## Description

This project is a basic example of a microservice with some features such as [multilingual](https://www.nuget.org/packages/My.Extensions.Localization.Json/2.0.0), [serilog](https://www.nuget.org/packages/Serilog.Sinks.File/4.1.0), external web service [resiliency](https://www.nuget.org/packages/Polly/7.2.1), [validation](https://www.nuget.org/packages/FluentValidation.AspNetCore/8.6.2), [swagger](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Filters/5.1.1), [EFCoreInMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/3.1.3) (to be used for testing purposes).

## Requirements
```bash
Install-Package Swashbuckle.AspNetCore.Filters -Version 5.1.1 #for swagger with parameters
Install-Package Serilog.Sinks.File -Version 4.1.0 #write log to a file
Install-Package My.Extensions.Localization.Json -Version 2.0.0 #use string resources with multi language json files
Install-Package Microsoft.EntityFrameworkCore.InMemory -Version 3.1.3 #EntityFramework in Memory
Install-Package FluentValidation.AspNetCore -Version 8.6.2 #validation library for .NET
Install-Package Polly -Version 7.2.1 #resilience and transient fault handling policies
```

## Structure
This solution has 3 projects.

***Data***: is only for the persistence layer implementation(without aggregates or value object..) and you must define one repository per aggregate.

***Domain***: Define aggregates, business logic.

***API***: CRUD actions for aggregate and use services from the domain.

***Projects:***

* **Ali.Hosseini.Application.Data** - .NetStandard 2.1 Class Library

* **Ali.Hosseini.Application.Api** - .NetCore 3.1 Kestrel Host for an WebApi

* **Ali.Hosseini.Application.Domain** – .NetStandard 2.1 Class Library Containing Business Logic and Models
### Serilog in Startup.cs

```c#
  public virtual void ConfigureServices(IServiceCollection services){
...
    Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(Configuration) #define serilog configs in appsettings.json
                         .CreateLogger();
...
```
#### Serilog in Program.cs
```c#
  public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() #add this line
...
```

   
### Resiliency in Startup.cs

```c#
 public virtual void ConfigureServices(IServiceCollection services){
...
//Retry and Fallback policies
var policyWrap = Policy.WrapAsync(FallbackPolicy(), GetRetryPolicy());
services.AddHttpClient("HttpClient").AddPolicyHandler(policyWrap);
...
```
### MultiLingual in Startup.cs
```c#
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
...
```
### FluentValidation in Startup.cs
```c#
 public virtual void ConfigureServices(IServiceCollection services){
...
//Validation
services.AddTransient<IValidator<Applicant>, ApplicantValidation>();
services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
...
```
