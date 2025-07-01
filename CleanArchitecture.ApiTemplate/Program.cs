



namespace CleanArchitecture.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Inject external DI
            builder.Services
                .AddDataAccessServices(config)
                .AddServiceLayer()
                .AddApiLayer(config);

            // Identity config
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            // Redis
          

            //var redisOptions = new ConfigurationOptions
            //{
            //    EndPoints = { "redis-15356.c251.east-us-mz.azure.redns.redis-cloud.com:15356" },
            //    Password = "b4h7EzqUCTHgAT6NQ19Pa0jJeSWacnxr",
            //    User = "default", 
            //    Ssl = true,
            //    AbortOnConnectFail = false
            //};

            //redisOptions.CertificateValidation += (sender, cert, chain, errors) => true;

            //builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            //    ConnectionMultiplexer.Connect(redisOptions));




            var app = builder.Build();

            // Redis ImageHelper Config
            var accessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            ImageHelper.Configure(accessor, env);

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
