using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers;
using Liquid.WebApi.Http.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Liquid.Sample.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddLiquidMongoRepository<SampleEntity, int>("Liquid:MyMongoDbSettings:Entities");

            services.AddLiquidServiceBusProducer<SampleMessageEntity>("Liquid:Messaging:ServiceBus:SampleProducer");

            services.AddLiquidHttp(typeof(SampleRequest).Assembly);

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
