using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NomadChat.WebAPI.Configs;
using NomadChat.WebAPI.Helpers.IoC;
using NomadChat.WebAPI.Hubs;
using NomadChat.WebAPI.Middlewares;
using NomadChat.WebAPI.Settings;
using System.Reflection;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.UtilityServices.Models.Common;
using WebPush;

namespace NomadChat.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterEncryption(Configuration);

            services.RegisterAllinjections();

            var vapidDetails = new VapidDetails(
                Configuration.GetValue<string>("VapidDetails:Subject"),
                Configuration.GetValue<string>("VapidDetails:PublicKey"),
                Configuration.GetValue<string>("VapidDetails:PrivateKey"));

            services.AddTransient(c => vapidDetails);

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfigurationModel>();

            services.AddSingleton(emailConfig);

            services.AddSignalR();

            services.AddCors();

            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

            services.SetupIdentity();

            services.RegisterJwt(Configuration);

            services.AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly);

            services.AddControllersWithViews().AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "NomadChat.WebAPI", Version = "v1" });
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "NomadChat.WebAPI v1"));

            app.UseRouting();

            app.UseCors(builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200"));

            app.UseMiddleware<AuthTokenMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });
        }
    }
}
