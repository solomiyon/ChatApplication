using ChatApp.DAL;
using ChatApp.DAL.Entity;
using ChatApp.DAL.UnitOfWork;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ChatApp.BLL.Infrastructure
{
    public static class BllServiceCollectionsExtentions
    {
        public static IServiceCollection AddMainContext(this IServiceCollection services, string connectionString, IConfiguration Configuration)
        {
            return services.AddDbContext<ChatAppDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString(connectionString)));
        }

        public static IServiceCollection AddIdentityFromBll(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                  .AddEntityFrameworkStores<ChatAppDbContext>();

            return services;
        }

        public static IServiceCollection AddAuthenticationFromBll(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = token.JwtIssuer,
                        ValidAudience = token.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.JwtKey)),
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true
                    };
                    //cfg.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var accessToken = context.Request.Query["access_token"];

                    //        // если запрос направлен хабу
                    //        var path = context.HttpContext.Request.Path;
                    //        if (!string.IsNullOrEmpty(accessToken) &&
                    //            (path.StartsWithSegments("/chatHub")))
                    //        {
                    //            // получаем токен из строки запроса
                    //            context.Token = accessToken;
                    //        }
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {

            return services.AddTransient<IUnitOfWork, UnitOfWork>(provider =>
                new UnitOfWork(provider.GetRequiredService<ChatAppDbContext>()));
        }
    }
}
