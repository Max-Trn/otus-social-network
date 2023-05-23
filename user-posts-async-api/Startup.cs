using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using user_posts_async_api.Configuration;
using user_posts_async_api.Services;
using user_posts_async_api.Services.Infrastructure;
using user_posts_async_api.Services.Interfaces;

namespace user_posts_async_api;

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
        services.AddMvc();
        
        services.AddTransient<IUserService, UserService>();
        services.AddSingleton<PostConsumer>();
        services.AddHostedService<PostConsumer>(provider => provider.GetService<PostConsumer>());
        services.AddSingleton<PostWebSocketService>();
    }

    private ApplicationConfiguration GetConfiguration()
    {   
        var myConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        var applicationKey = myConfig.GetValue<string>("Application:ApplicationKey");

        return new ApplicationConfiguration()
        {
            ApplicationKey = applicationKey
        };
    }
}