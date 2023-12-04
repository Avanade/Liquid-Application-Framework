using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.Adapter.Dataverse.Extensions.DependencyInjection;

namespace Liquid.Adapter.Dataverse.Console
{
	internal static class DataverseDependecyInjection
	{
		public static HostApplicationBuilder CreateHost()
		{
			HostApplicationBuilder builder = Host.CreateApplicationBuilder();

			builder.Configuration.Sources.Clear();

			builder.Configuration
				.AddJsonFile("appsettings.json", true, true);
			//.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

			builder.Services.AddLiquidDataverseAdapter("DataverseSettings");

			return builder;
		}
	}
}
