using System;
using System.Collections.Generic;
using System.Text;
using CSharpDuels;
using CSharpDuels.DataContext;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CSharpDuels
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string authKey = Environment.GetEnvironmentVariable("AuthKey");
            string endpoint = Environment.GetEnvironmentVariable("ServiceEndpoint");
            string dbName = Environment.GetEnvironmentVariable("DatabaseName");
            builder.Services.AddDbContext<CSharpDuelsDbContext>(
                options => options.UseCosmos(endpoint,authKey,dbName));
            builder.Services.AddHttpContextAccessor();
        }
    }
}
