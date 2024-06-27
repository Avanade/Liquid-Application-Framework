using Liquid.WebApi.Http.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PROJECTNAME.Domain;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//TODO: Register and configure repository Liquid Cartridge
//
// Examples:
//
// [Mongo Cartridge]
// 1. add Liquid Cartridge using CLI : dotnet add package Liquid.Repository.Mongo --version 6.*
// 3. import liquid cartridge reference here: using Liquid.Repository.Mongo.Extensions;
// 4. call cartridge DI method here : builder.Services.AddLiquidMongoRepository<ENTITYNAME, ENTITYIDTYPE>("Liquid:MyMongoDbSettings:Entities");
// 5. edit appsettings.json file to include database configurations.
//
//[EntityFramework Cartridge]
// 1. add DbContext using CLI command: dotnet new liquiddbcontextproject --projectName PROJECTNAME --entityName ENTITYNAME
// 2. add PROJECTNAME.Repository to solution: dotnet sln add PROJECTNAME.Repository/PROJECTNAME.Repository.csproj
// 3. add PROJECTNAME.Repository project reference to PROJECTNAME.WebApi project: dotnet add reference ../PROJECTNAME.Repository/PROJECTNAME.Repository.csproj
// 4. add database dependency in this project using CLI command: dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.*
// 5. import EntityFrameworkCore reference here: using Microsoft.EntityFrameworkCore;
// 6. set database options here: Action<DbContextOptionsBuilder> options = (opt) => opt.UseInMemoryDatabase("CRUD");
// 7. add Liquid Cartridge in this project using CLI command: dotnet add package Liquid.Repository.EntityFramework --version 6.*
// 8. import liquid cartridge reference here: using Liquid.Repository.EntityFramework.Extensions;
// 9. import PROJECTNAME.Repository here: using PROJECTNAME.Repository;
// 9. call cartridge DI method here: builder.Services.AddLiquidEntityFramework<LiquidDbContext, ENTITYNAME, ENTITYIDTYPE>(options);
// 10. edit appsettings.json file to include database configurations if necessary (for InMemory it's not necessary).

builder.Services.AddLiquidHttp("Liquid", false, typeof(IDomainInjection).Assembly);
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