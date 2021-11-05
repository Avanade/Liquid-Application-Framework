using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Services.Grpc.Tests.Server
{
    [ExcludeFromCodeCoverage]
    public class GrpcServer
    {
        private static IHost _server;

        public static void StartServer()
        {
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("https://*:8081");
                webBuilder.UseStartup<Startup>();
            });

            _server = hostBuilder.Build();
            _server.StartAsync();
        }

        public static void StopServer()
        {
            _server?.StopAsync();
            _server?.Dispose();
        }

    }
}