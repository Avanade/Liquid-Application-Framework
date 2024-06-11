using Liquid.Messaging.ServiceBus.Extensions.DependencyInjection;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers;
using Liquid.WebApi.Http.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLiquidMongoRepository<SampleEntity, Guid>("Liquid:MyMongoDbSettings:Entities");

builder.Services.AddLiquidServiceBusProducer<SampleMessageEntity>("Liquid:ServiceBus", "liquidoutput", true);

builder.Services.AddLiquidHttp(typeof(SampleRequest).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
