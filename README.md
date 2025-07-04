# Liquid Application Framework

![GitHub issues](https://img.shields.io/github/issues/Avanade/Liquid-Application-Framework)
![GitHub](https://img.shields.io/github/license/Avanade/Liquid-Application-Framework)
![GitHub Repo stars](https://img.shields.io/github/stars/Avanade/Liquid-Application-Framework?style=social)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg)](https://avanade.github.io/code-of-conduct/)
[![Ready Ava Maturity](https://img.shields.io/badge/Ready-Ava--Maturity-%23FF5800?labelColor=green)](https://avanade.github.io/maturity-model/)

This repository contains Liquid Application Framework full code, samples, templates and [documentation](docs/About-Liquid.md).

## What is Liquid?

Liquid is a **multi-cloud** framework designed to **accelerate the development** of cloud-native microservices while avoiding coupling your code to specific cloud providers.

When writing Liquid applications, you stop worrying about the technology and focus on your business logic - Liquid abstracts most of the boilerplate and let you just write domain code that looks great and gets the job done.

## Feature

- Abstracts a number of services from cloud providers such as Azure, AWS and Google Cloud to enable you to write code that could run anywhere.
- Brings a directed programming model that will save you time on thinking how to structure your application, allowing you to focus on writing business code.

| Liquid Application Framework Binaries | Version |
| :-- | :--: |
| [`Liquid.Core`](https://www.nuget.org/packages/Liquid.Core) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Core) |
| [`Liquid.Repository.Mongo`](https://www.nuget.org/packages/Liquid.Repository.Mongo) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Repository.Mongo) |
| [`Liquid.Repository.EntityFramework`](https://www.nuget.org/packages/Liquid.Repository.EntityFramework) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Repository.EntityFramework) |
| [`Liquid.Repository.OData`](https://www.nuget.org/packages/Liquid.Repository.OData) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Repository.OData) |
| [`Liquid.Cache.Memory`](https://www.nuget.org/packages/Liquid.Cache.Memory) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Cache.Memory) |
| [`Liquid.Cache.NCache`](https://www.nuget.org/packages/Liquid.Cache.NCache) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Cache.NCache) |
| [`Liquid.Cache.Redis`](https://www.nuget.org/packages/Liquid.Cache.Redis) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Cache.Redis) |
| [`Liquid.Cache.SqlServer`](https://www.nuget.org/packages/Liquid.Cache.SqlServer) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Cache.SqlServer) |
| [`Liquid.Messaging.Kafka`](https://www.nuget.org/packages/Liquid.Messaging.Kafka) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Messaging.Kafka) |
| [`Liquid.Messaging.RabbitMq`](https://www.nuget.org/packages/Liquid.Messaging.RabbitMq) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Messaging.RabbitMq) |
| [`Liquid.Messaging.ServiceBus`](https://www.nuget.org/packages/Liquid.Messaging.ServiceBus) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Messaging.ServiceBus) |
| [`Liquid.WebApi.Http`](https://www.nuget.org/packages/Liquid.WebApi.Http) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.WebApi.Http) |
| [`Liquid.Dataverse`](https://www.nuget.org/packages/Liquid.Dataverse) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Dataverse) |
| [`Liquid.Storage.AzureStorage`](https://www.nuget.org/packages/Liquid.Storage.AzureStorage) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.Storage.AzureStorage) |
| [`Liquid.GenAi.OpenAi`](https://www.nuget.org/packages/Liquid.GenAi.OpenAi) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Liquid.GenAi.OpenAi) |


## Getting Started

You can use Liquid Templates to get your microservice started.

Install the templates by running the following dotnet CLI command at the PowerShell prompt :

```Shell
dotnet new install Liquid.Templates
```
and run dotnet new command with the name and parameters of the following templates:
|Name|Description|
| :-- | :-- |
|`liquidcrudsolution`       |Liquid WebAPI CRUD Solution (Domain and WebAPI projects)              |
|`liquidcrudaddentity`      |Liquid entity class, CRUD mediator handlers and CRUD controller       |
|`liquiddomainaddhandler`   |Liquid mediator command handler                                       |
|`liquiddomainproject`      |Liquid Domain project (mediator command handler)                      |
|`liquidwebapisolution`     |Liquid WebAPI solution (Domain and WebAPI projects)                   |
|`liquidwebapiaddentity`    |Liquid entity class, mediator command handler and CRUD controller     |
|`liquidwebapiproject`      |Liquid WebAPI project                                                 |
|`liquidworkersolution`     |Liquid WorkerService solution (Domain and WorkerService projects)     |
|`liquidworkerproject`      |Liquid WorkerService project                                          |
|`liquidbcontextaddentity`  |Liquid DbContext entity configuration class (for Entity Framework)    |
|`liquiddbcontextproject`   |Liquid Repository project (EntityFramework DbContext configurations)  |


### Sample:
To create an WebAPI solution with CRUD handlers, you must:
- execute command :
```Shell
dotnet new liquidcrudsolution --projectName "some root namespace" --entityName "some entity" --entityIdType "type of your unique ID"
```

- open the folder where the command was executed, and open the project created in the IDE of your choice:

![template_sample](https://user-images.githubusercontent.com/30960065/153954780-0ec8a6c0-153e-4bbc-8f3a-4ccc9c1e7858.png)

- follow the instructions found in the generated code TODOs, and run!

> You can make changes in code, add more classes and project if you need, and also using others Liquid templates to do it!


## Contribute

Some of the best ways to contribute are to try things out, file issues, and make pull-requests.

- You can provide feedback by filing issues on GitHub or open a discussion in [Discussions tab](https://github.com/Avanade/Liquid-Application-Framework/discussions). We accept issues, ideas and questions.
- You can contribute by creating pull requests for the issues that are listed. Look for issues marked as _ready_ if you are new to the project.
- Avanade asks that all commits sign the [Developer Certificate of Origin](https://developercertificate.org/).

In any case, be sure to take a look at [the contributing guide](CONTRIBUTING.md) before starting, and see our [security disclosure](https://github.com/Avanade/avanade-template/blob/main/SECURITY.md) policy.

## Who is Avanade?

[Avanade](https://www.avanade.com) is the leading provider of innovative digital, cloud and advisory services, industry solutions and design-led experiences across the Microsoft ecosystem.
