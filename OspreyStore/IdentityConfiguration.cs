using ApplicationCore.Entities;
using ApplicationCore.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OspreyStore
{
    public static class IdentityConfiguration
    {
        public static AuthorizationPolicy AuthorizationPolicy
        {
            get
            {
                return new AuthorizationPolicyBuilder()
                                        .RequireAuthenticatedUser()
                                        .Build();
            }
        }
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Issuer"],
                ValidAudience = configuration["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"])),
                ClockSkew = TimeSpan.Zero,
            };
        }

        public static void ConfigureProduction(IServiceCollection services, IConfiguration configuration)
        {
            DbContextOptions<AppIdentityDbContext> identityContextOptions =
                   new DbContextOptionsBuilder<AppIdentityDbContext>()
                       .UseSqlServer(configuration.GetConnectionString("IdentityConnection")).Options;
            services.AddSingleton(identityContextOptions);
        }
        public static void ConfigureTesting(IServiceCollection services)
        {
            DbContextOptions<AppIdentityDbContext> storeContextOptions =
            new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase("AppIdentityDbContext").Options;

            services.AddSingleton(storeContextOptions);
        }
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ => GetTokenValidationParameters(configuration));
            services.AddDbContext<AppIdentityDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = GetTokenValidationParameters(configuration);
                    jwtBearerOptions.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthorizationHandler, ItemAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, BrandAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CategoryAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CharacteristicAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CharacteristicValueAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ItemVariantImageAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ItemVariantAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ItemPropertyAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, MeasurementUnitAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, StoreAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CartItemAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, OrderAuthorizationHandler>();

            services.AddScoped(typeof(IAuthorizationParameters<>), typeof(AuthorizationParameters<>));
            services.AddScoped<IAuthorizationParameters<CartItem>, CartItemAuthorizationParameters>();
            services.AddScoped<IAuthorizationParameters<Order>, OrderAuthorizationParameters>();
        }
    }
}
