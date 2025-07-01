namespace CleanArchitecture.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.Configure<IdentityOptions>(opts =>
                opts.SignIn.RequireConfirmedEmail = true);

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            #region Swagger
            services.AddSwaggerGen(swagger =>
               {
                   swagger.SwaggerDoc("v1", new OpenApiInfo
                   {
                       Version = "v1",
                       Title = "ApiTemplate",
                       Description = "Simple Template N-Tier"
                   });

                   swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "Bearer",
                       BearerFormat = "JWT",
                       In = ParameterLocation.Header,
                       Description = "Enter 'Bearer' [space] and then your valid token below.",
                   });

                   swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                   });
               });

            #endregion

            #region JWT
            // JWT config
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config["JWT:ValidIssuer"],
                    ValidAudience = config["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]))
                };
            });
            #endregion


            #region Email
            var emailconfig = config.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailconfig);
            services.AddScoped<IEmailService, EmailService>();
            services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);
            #endregion

            return services;
        }
    }
}
