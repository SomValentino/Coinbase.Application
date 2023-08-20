using Coinbase.Exchange.API.BackgroundServices;
using Coinbase.Exchange.API.Identity;
using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Coinbase.Exchange.API
{
    public static class ServiceInstaller
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApiConfiguration>>().Value;
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

            services.AddTransient<IAuthorizationHandler,ValidTokenAuthorizationHandler>();
            services.AddAuthorization();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = apiConfiguration.JwtAudience,
                        ValidIssuer = apiConfiguration.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiConfiguration.JwtKey)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context => {
                            var accessToken = context.Request.Query["access_token"];

                            logger.LogInformation("Obtained access token from query string: {token}", accessToken);

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/exchangesubscription")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;

                                logger.LogInformation("Setting token in http request context");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientRegistration", policy =>
                {
                    policy.RequireRole(apiConfiguration.Role);
                    policy.AddRequirements(new ValidTokenRequirement());
                });
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient("coinbaseapp", options => {
                var baseAddress = apiConfiguration.ApiBaseUrl;
                options.BaseAddress = new Uri(baseAddress);
            });

            services.AddHostedService<ExchangeWorker>();
        }
    }
}
