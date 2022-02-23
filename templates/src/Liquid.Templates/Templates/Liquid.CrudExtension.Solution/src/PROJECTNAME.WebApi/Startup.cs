using Liquid.WebApi.Http.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PROJECTNAME.Domain;
using PROJECTNAME.Domain.Entities;

namespace PROJECTNAME.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Register and configure repository Liquid Cartridge
            //
            // Examples:
            //
            // [Mongo Cartridge]
            // 1. add Liquid Cartridge using CLI : dotnet add package Liquid.Repository.Mongo --version 2.X.X
            // 2. call cartridge DI method : services.AddLiquidMongoRepository<SampleEntity, int>("Liquid:MongoSettings:Entities");
            // 3. edit appsettings.json file to include database configurations.
            //
            //[EntityFramework Cartridge]
            // 1. add Liquid Cartridge using CLI command: dotnet add package Liquid.Repository.EntityFramework --version 2.X.X
            // 2. add database dependency using CLI command: dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.X.X
            // 3. add DbContext using CLI command: dotnet new liquiddbcontextproject --projectName SampleProject --entityName Sample
            // 4. set database options : void options(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase(databaseName: "Liquid");
            // 5. call cartridge DI method : services.AddLiquidEntityFramework<ProjectNameDbContext, SampleEntity, int>(options);
            // 6. edit appsettings.json file to include database configurations.

            services.AddLiquidHttp(typeof(IDomainInjection).Assembly);

            services.RegisterCrud<ENTITYNAMEEntity, int>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseLiquidConfigure();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
