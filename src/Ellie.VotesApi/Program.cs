using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ellie.VotesApi;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
