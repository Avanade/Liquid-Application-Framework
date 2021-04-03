# Liquid Application Framework

This repository contains Liquid Application Framework [documentation](docs/About-Liquid.md), useful links and sample project

| New Major Version Warning |
|----|
- This is the new major version of Liquid Application Framework, launched on April/2021. 
- We've made significant breaking changes and a complete rearchitecture of our framework. So, this version isn't compatible and there is no easy conversion from the old one. 
- We decided to deprecate the old version and it will not receive any kind of updates, not even bug fixes. But, for historical purposes and to allow anyone relying on it to fork the code and maintain its own version of it, we'll keep the [old repository](https://github.com/Avanade/Liquid-Application-Framework-1.0-deprecated) public. 


## What is Liquid?
Liquid is a **multi-cloud** framework designed to **accelerate the development** of cloud-native microservices while avoiding coupling your code to specific cloud providers.

When writing Liquid applications, you stop worrying about the technology and focus on your business logic - Liquid abstracts most of the boilerplate and let you just write domain code that looks great and gets the job done.


## Features
- Abstracts a number of services from cloud providers such as Azure, AWS and Google Cloud to enable you to write code that could run anywhere.
- Brings a directed programming model that will save you time on thinking how to structure your application, allowing you to focus on writing business code.


## Getting Started
To use Liquid, you create a new base ASP.Net Core application and then download and install the following nuget packages:

| Liquid Application Framework Binaries | Type | Version |
| :-- | :--: | :--: |
| [`Liquid.Core`](https://www.nuget.org/packages/Liquid.Core) | **_mandatory_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Core) |
| [`Liquid.Domain`](https://www.nuget.org/packages/Liquid.Domain) | **_desirable_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Domain) |
| [`Liquid.Repository`](https://www.nuget.org/packages/Liquid.Repository) | **_optional_** | Not released |
| [`Liquid.Cache`](https://www.nuget.org/packages/Liquid.Cache) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Cache) |
| [`Liquid.Messaging`](https://www.nuget.org/packages/Liquid.Messaging) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Messaging) |
| [`Liquid.Services`](https://www.nuget.org/packages/Liquid.Services) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Services) |
| [`Liquid.WebApi.Http`](https://www.nuget.org/packages/Liquid.WebApi) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.WebApi.Http) |
| [`Liquid.WebApi.Grpc`](https://www.nuget.org/packages/Liquid.WebApi) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.WebApi.Grpc) |
| [`Liquid.Serverless`](https://www.nuget.org/packages/Liquid.Serverless.AzureFunctions) | **_optional_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Serverless.AzureFunctions) |
| [`Liquid.Tests`](https://www.nuget.org/packages/Liquid.Tests) | **_desirable_** | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Tests) |

And then choose the implementation cartridges you need to your project, for example:
- You can choose to expose an API using an HTTP endpoint (then install [`Liquid.WebApi.Http`](https://www.nuget.org/packages/Liquid.WebApi)) or using the Grpc protocol ([`Liquid.WebApi.Grpc`](https://www.nuget.org/packages/Liquid.Grpc))
- You can choose to use MongoDB as your data repository (then install [`Liquid.Repository.MongoDB`](https://www.nuget.org/packages/Liquid.Repository.MongoDB)) or to use  Entity Framework ([`Liquid.Repository.EntityFramework`](https://www.nuget.org/packages/Liquid.Repository.EntityFramework))
- You can choose to use Azure ServiceBus as your messaging platform (then install [`Liquid.Messaging.Azure`](https://www.nuget.org/packages/Liquid.Messaging.Azure)) or to use AWS SQS ([`Liquid.Messaging.Aws`](https://www.nuget.org/packages/Liquid.Messaging.Aws))


## Contribute
Some of the best ways to contribute are to try things out, file issues, and make pull-requests.
- You can provide feedback by filing issues on GitHub. We accept issues, ideas and questions. 
- You can contribute by creating pull requests for the issues that are listed. Look for issues marked as _good first issue_ if you are new to the project.

In any case, be sure to take a look at [the contributing guide](CONTRIBUTING.md) before starting.


## Useful Links

| Liquid Application Framework Sources | 
| :-- | 
| [Liquid.Core](https://github.com/Avanade/Liquid.Core) |
| [Liquid.Domain](https://github.com/Avanade/Liquid.Domain) |
| [Liquid.Repository](https://github.com/Avanade/Liquid.Repository) |
| [Liquid.Cache](https://github.com/Avanade/Liquid.Cache) |
| [Liquid.Messaging](https://github.com/Avanade/Liquid.Messaging) |
| [Liquid.Services](https://github.com/Avanade/Liquid.Services) |
| [Liquid.WebApi](https://github.com/Avanade/Liquid.WebApi) |
| [Liquid.Serverless](https://github.com/Avanade/Liquid.Serverless) |
| [Liquid.Tests](https://github.com/Avanade/Liquid.Tests) | 

