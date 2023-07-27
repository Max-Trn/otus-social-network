
using System.Text;
using dialogs_api.Configuration;
using dialogs_api.DAL.Infrastructure;
using dialogs_api.DAL.Repositories;
using dialogs_api.Services;
using dialogs_api.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace dialogs_api;

public class Startup
{
    public IConfigurationRoot Configuration { get; set; }

    public Startup(IHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseWebSockets();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.EnableAnnotations();
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        });
        services.AddTransient<ILogger>(s => s.GetService<ILogger<Program>>());
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient<DialogRepository>();
        
        var config = GetConfiguration();
        services.AddSingleton(config);

        services.AddTransient<ConnectionFactory>((sp) => new ConnectionFactory(config));

        services.AddMvc();
    }

    private ApplicationConfiguration GetConfiguration()
    {   
        var myConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        var connectionString = myConfig.GetValue<string>("Application:ConnectionString");

        return new ApplicationConfiguration()
        {
            ConnectionString = connectionString,
        };
    }
}