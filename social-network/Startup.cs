
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using social_network.Configuration;
using social_network.DAL.Infrastructure;
using social_network.DAL.Repositories;
using social_network.HttpClients.Interfaces;
using social_network.Services;
using social_network.Services.Interfaces;

namespace social_network;

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

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
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
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Description = "Please insert JWT token into field"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
        services.AddTransient<UserRepository>();
        services.AddTransient<PostRepository>();
        services.AddTransient<FriendRepository>();
        
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddSingleton<IMessageService, MessageService>();
        
        services.AddSingleton<IDialogClient, DialogClient>();
        
        var config = GetConfiguration();
        services.AddSingleton(config);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "test",
                ValidAudience = "test",
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(
                    $"{config.ApplicationKey}")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = false
            };
        });
        services.AddAuthorization();

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
        
        var applicationKey = myConfig.GetValue<string>("Application:ApplicationKey");
        var connectionString = myConfig.GetValue<string>("Application:ConnectionString");
        var connectionStringReplica = myConfig.GetValue<string>("Application:ConnectionStringReplica");

        return new ApplicationConfiguration()
        {
            ConnectionString = connectionString,
            ConnectionStringReplica = connectionStringReplica,
            ApplicationKey = applicationKey
        };
    }
}