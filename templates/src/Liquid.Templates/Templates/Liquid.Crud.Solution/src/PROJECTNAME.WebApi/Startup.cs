using PROJECTNAME.Domain.Entities;
using Liquid.WebApi.Http.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.Create;

namespace PROJECTNAME.WebApi
{
    public class Startup
    {

        /// <summary>
        ///     Startup Constructor
        /// </summary>
        /// <param name="configuration">
        ///     Object IConfiguration
        /// </param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     Object IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Register and configure repositories for Liquid Cartridge
            //
            // Examples:
            //
            // [Mongo Cartrige]
            // 1. add Liquid Cartridge using CLI : dotnet add package Liquid.Repository.Mongo --version 2.X.X
            // 2. call cartridge DI method : services.AddLiquidMongoRepository<SampleEntity, int>("Liquid:MongoSettings:Entities");
            // 3. edit appsettings.json file to include data base configurations.
            //
            //[EntityFramework Cartrige]
            // 1. add Liquid Cartridge using CLI command: dotnet add package Liquid.Repository.EntityFramework --version 2.X.X
            // 2. add database dependency using CLI command: dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.X.X
            // 3. add DbContext using CLI command: dotnet new liquiddbcontextproject --projectName SampleProject --entityName SampleEntity
            // 4. set database options : void options(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase(databaseName: "Liquid");
            // 5. call cartridge DI method : services.AddLiquidEntityFramework<ProjectNameDbContext, SampleEntity, int>(options);
            // 6. edit appsettings.json file to include data base configurations.

            services.AddLiquidHttp(typeof(PostENTITYNAMECommand).Assembly);

            services.AddControllers();

            services.AddLogging();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLiquidConfigure();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
