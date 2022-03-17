using PROJECTNAME.Domain;
using PROJECTNAME.Domain.Entities;
using Liquid.WebApi.Http.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            //TODO: Register and configure repository Liquid Cartridge
            //
            // Examples:
            //
            // [Mongo Cartridge]
            // 1. add Liquid Cartridge using CLI : dotnet add package Liquid.Repository.Mongo --version 2.*
            // 3. import liquid cartridge reference here: using Liquid.Repository.Mongo.Extensions;
            // 4. call cartridge DI method here : services.AddLiquidMongoRepository<ENTITYNAMEEntity, ENTITYIDTYPE>("Liquid:MongoSettings:Entities");
            // 5. edit appsettings.json file to include database configurations.
            //
            //[EntityFramework Cartridge]
            // 1. add DbContext using CLI command: dotnet new liquiddbcontextproject --projectName PROJECTNAME --entityName ENTITYNAME
            // 2. add PROJECTNAME.Repository to solution: dotnet sln add PROJECTNAME.Repository/PROJECTNAME.Repository.csproj
            // 3. add PROJECTNAME.Repository project reference to PROJECTNAME.WebApi project: dotnet add reference ../PROJECTNAME.Repository/PROJECTNAME.Repository.csproj
            // 4. add database dependency in this project using CLI command: dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.0.13
            // 5. import EntityFrameworkCore reference here: using Microsoft.EntityFrameworkCore;
            // 6. set database options here: Action<DbContextOptionsBuilder> options = (opt) => opt.UseInMemoryDatabase("CRUD");
            // 7. add Liquid Cartridge in this project using CLI command: dotnet add package Liquid.Repository.EntityFramework --version 2.*
            // 8. import liquid cartridge reference here: using Liquid.Repository.EntityFramework.Extensions;
            // 9. import PROJECTNAME.Repository here: using PROJECTNAME.Repository;
            // 9. call cartridge DI method here: services.AddLiquidEntityFramework<LiquidDbContext, ENTITYNAMEEntity, ENTITYIDTYPE>(options);
            // 10. edit appsettings.json file to include database configurations if necessary (for InMemory it's not necessary).

            services.AddLiquidHttp(typeof(IDomainInjection).Assembly);

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
