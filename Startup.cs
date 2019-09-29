using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using ChenpionPMS.Models;

namespace ChenpionPMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private bool IsSqlite()
        {
            return "Sqlite".Equals(Configuration.GetValue("DatabaseEngine", ""));
        }

        private bool IsMySQL()
        {
            return "MySQL".Equals(Configuration.GetValue("DatabaseEngine", ""));
        }

        private string ConnectionString()
        {
            return Configuration.GetConnectionString("DefaultConnection");
        }

        private void SetupDbContextOptions(DbContextOptionsBuilder options)
        {
            if (IsSqlite())
            {
                options.UseSqlite(ConnectionString());
            }
            else if (IsMySQL())
            {
                options.UseMySql(ConnectionString());
            }
            else
            {
                options.UseInMemoryDatabase(databaseName: "ChenpionPMS_DB");
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => SetupDbContextOptions(options));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.SubstitutionFormat = "VVV";
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant()
                    );
                }
            });
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new Info()
                    {
                        Title = $"ChenpionPMS API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    } );
            }
        }
    }
}
