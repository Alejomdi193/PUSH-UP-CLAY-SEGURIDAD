using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Services;
using Aplicacion.UnitOfWork;
using AspNetCoreRateLimit;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
       {
           options.AddPolicy("CorsPolicy", builder =>
           builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
       });

    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>(); 
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
       {
           options.DefaultApiVersion = new ApiVersion(1, 0);
           options.AssumeDefaultVersionWhenUnspecified = true;

           options.ApiVersionReader = ApiVersionReader.Combine(
               new QueryStringApiVersionReader("ver"),
               new HeaderApiVersionReader("X-Version")
           );
       });
    }

    public static void ConfigureRatelimiting(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 15
                }
            };
        });
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWT>(configuration.GetSection("JWT"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
    }
    }
}