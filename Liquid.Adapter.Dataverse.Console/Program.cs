// See https://aka.ms/new-console-template for more information

using Liquid.Adapter.Dataverse;
using Liquid.Adapter.Dataverse.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

var host = DataverseDependecyInjection.CreateHost().Build();
var serviceProvider = host.Services;

var connectionString = string.Format("AuthType=ClientSecret;Url={0};ClientId={1};ClientSecret={2};", "https://mrcrm-validation.crm.dynamics.com", "b5c2465f-8792-412e-a69c-4e11bdc78f7d", "Upq8Q~EnCevaCsqyhQBQcxAR.SwXYcdS7SBYPc-z");
var service = new ServiceClient(connectionString);

var adapter = serviceProvider.GetRequiredService<DataverseAdapter>();

// Test data
Entity contact1 = new Entity
{
	LogicalName = "contact",
	Attributes =
	{
		new KeyValuePair<string, object>("firstname", "Test"),
		new KeyValuePair<string, object>("lastname", "Case 1")
	}
};

Entity contact2 = new Entity
{
	LogicalName = "contact",
	Attributes =
	{
		new KeyValuePair<string, object>("firstname", "Test"),
		new KeyValuePair<string, object>("lastname", "Case 2")
	}
};

Entity contact3 = new Entity
{
	LogicalName = "contact",
	Attributes =
	{
		new KeyValuePair<string, object>("firstname", "Test"),
		new KeyValuePair<string, object>("lastname", "Case 3")
	}
};

Entity contact4 = new Entity
{
	LogicalName = "contact",
	Attributes =
	{
		new KeyValuePair<string, object>("firstname", "Claudia"),
		new KeyValuePair<string, object>("lastname", "Manzzanti"),
		new KeyValuePair<string, object>("emailaddress1", "claudiamazzanti@crmdemo.dynamics.com")
	}
};

//// Test Case 1: Supress Powr Automate Trigger (Expected: Yes / No)
var c1id = adapter.Create(contact1,
	bypassSynchronousCustomLogic: false,
	suppressPowerAutomateTrigger: true,
	suppressDuplicateDetectionRules: true);

//// Test Case 2: Bypass Synchronous Logic - OK (Expected: No / Yes )
var c2id = adapter.Create(contact2,
	bypassSynchronousCustomLogic: true,
	suppressPowerAutomateTrigger: false,
	suppressDuplicateDetectionRules: true);

//// Test Case 3: Bypass Sync Logic and Supress Power Automate -  (Expected: No / No )
var c3id = adapter.Create(contact3,
	bypassSynchronousCustomLogic: true,
	suppressPowerAutomateTrigger: true,
	suppressDuplicateDetectionRules: true);


//// Test Case 4: Enforce Duplicate Detection - OK (Expected: Exception by duplicate record)
try
{
	var c4id = adapter.Create(contact4,
		bypassSynchronousCustomLogic: true,
		suppressPowerAutomateTrigger: true,
		suppressDuplicateDetectionRules: false);
}
catch (Exception e)
{
	Console.WriteLine(e.Message);
}

Console.WriteLine("Done!");