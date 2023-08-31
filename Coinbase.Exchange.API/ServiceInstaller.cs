using Coinbase.Client.Websocket.Responses;
using Coinbase.Exchange.API.BackgroundServices;
using Coinbase.Exchange.API.Identity;
using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Logic.Processors;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Coinbase.Exchange.API
{
    public static class ServiceInstaller
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Coinbase API",
                    Description = "Coinbase API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            });

                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddSingleton<IEnumerable<ChannelProcessor>>(options => GetInstances<ChannelProcessor>(serviceProvider));
        }

        public static void UseException(this WebApplication app)
        {
            var logger = app.Services.GetService<ILogger<WebApplication>>();
            app.UseExceptionHandler(option => {
                option.Run(async context => {
                    context.Response.ContentType = "application/json";
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>();
                    logger?.LogError(exception?.Error.ToString());
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        StatusCode = System.Net.HttpStatusCode.InternalServerError,
                        ErrorMessage = "An Error occurred while processing your request. Kindly try again later"
                    }));
                });
            });
        }

        private static List<T> GetInstances<T>(IServiceProvider serviceProvider)
        {
            var instances = new List<T>();
            var foundInstances = Assembly.GetAssembly(typeof(T))?.GetTypes()
                                ?.Where(detector => detector.IsClass &&
                                !detector.IsAbstract && typeof(T).IsAssignableFrom(detector));

            if (foundInstances != null && foundInstances.Any())
            {
                foreach (var type in foundInstances)
                {
                    var typeDetector = (T?)serviceProvider.GetService(type);
                    if (typeDetector != null)
                        instances.Add(typeDetector);
                }
            }
            return instances;
        }
    }
}
