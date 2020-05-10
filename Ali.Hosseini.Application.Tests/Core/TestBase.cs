using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Net.Http;

namespace Ali.Hosseini.Application.Tests.Core
{
    public class TestBase : IDisposable
    {
        protected AppDbContext GetDbContext() => new AppDbContext(GetDbContextOptions());

        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        #region Ctor
        public TestBase()
        {
            var integrationTestsPath = PlatformServices.Default.Application.ApplicationBasePath;
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../../../../../Ali.Hosseini.Application/Ali.Hosseini.Application.Tests"));

            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseContentRoot(applicationPath)
                .UseEnvironment("Development"));
            _client = _server.CreateClient();

        }
        #endregion
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
        protected DbContextOptions<AppDbContext> GetDbContextOptions() => new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(databaseName: "DbTest")
         .Options;
        protected T GetService<T>() => ServiceProviderHandler.GetService<T>();
        protected T Get<T>(ActionResult<T> actionResult) => actionResult.Value ?? (T)((ObjectResult)actionResult.Result).Value;
    }
}
