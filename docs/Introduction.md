| [Main](About-Liquid) > Introduction | 

In the last years, the always challenging task of developing multi-hundred-line-of-code business applications has reached elevated sophistication and professionalism.
A high-quality application life-cycle is not only for high-tech companies anymore. To be or to become digital demands distinguished software engineering skills for companies of any industry and any size.
Avanade has historically helped several companies and professionals achieving proficiency in the development of modern applications during their digital transformation.
Now Avanade provides Liquid, a framework envisioned to boost such engineering capabilities, available as an Open Source project, for anyone looking for quick, standardized, flexible, extensible and high quality microservices development.

| Topics | 
| :-- | 
| [Technical Context](#Technical-Context) <br> [Avanade as a Thought Leader](#Avanade-as-a-Thought-Leader) <br> [Business Drivers](#Business-Drivers) <br> [Guiding Principles](#Guiding-Principles) | 

### Technical Context

The development of enterprise custom application has passed through several architectural waves in the last couple decades, with each new wave adding some complexity on top of the old ones.
In recent years, with the advent of modern architectural standards such as REST APIs, microservices on containers and single page applications as well as DevOps practices, the modern applications do have new strengths that are of great value to businesses and their customers. 
However, the complexity of designing, building and running such applications increased a lot, demanding great effort from architects, software and operation engineers to fully implement all the mandatory concepts and capabilities.
Moreover, such effort is a repeating task while yet an error-prone one. That opens a great space for building a set of pre-built software components following a general-purpose technical design, thereby condensed as a framework for developing and running those so called modern applications.

### Avanade as a Thought Leader

Born on the union of Microsoft and Accenture, Avanade has quickly become [a major competence center for the Microsoft's ecosystem of technology, services and partners](https://www.avanade.com/en-us/media-center/press-releases/2020-microsoft-global-alliance-si-partner-of-the-year), with a track record of hundreds of enterprise grade, innovative projects delivered.
Microsoft, in its transformation into a cloud service company, has made a complete rotation from a proprietary vendor to an open-source friendly company, thus naturally getting more and more influence on the evolution of the industry. 
In particular, due to its influence in the Single Page Application (SPA) domain [with Angular & Typescript](https://techcrunch.com/2015/03/05/microsoft-and-google-collaborate-on-typescript-hell-has-not-frozen-over-yet/) and microservice architecture with [.NET Core & Docker](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/) (along with its open cloud offering such [Windows Azure](http://azure.microsoft.com) and VisualStudio.com) Microsoft has helped to pave the way for the building of modern digital applications.
On the other hand, Accenture is a top leader in technology consulting field and it continuously infuses Avanade with technical and methodological know-how in enterprise architecture and digital transformation.
Therefore, Avanade is best positioned to advise methods, patterns and building blocks on top of mature stacks and tools such as Microsoft's as well as to materialize the usual tech-agnostic point of view of Accenture.

### Business Drivers

The purpose of Liquid is to allow the building of modern applications with better quality in a more efficient way. Therefore, the following business drivers were stated:
- **Reduce time-to-value with faster development and testing activities:** The learning curve is shorter if developers can abstract technical complexity and have their components easily and automatically put into a DevOps pipeline;
- **Increase quality with code uniformity:** all application components can be written with the same high-level "technical" dialect thus promoting better code analysis, both static and dynamic;
- **Improve application governance, maintenance and evolution:** a single point of technical decision for the entire code allows better standards implementation, code maintenance as well as easier introduction and/or replacement of technical components;
- **Catalyze the use of delivery centers:** by getting most of the technical design out of the box, remote teams can focus on requirements (backlog) and on the design of the business solution (business logic on the application layer).

### Guiding Principles

From a technical perspective, the Liquid development was stressed toward the following principles:
- **Prescriptive yet non-intrusive programming model for business components:** frameworks and platform services (_e.g._ .NET Core and CosmosDB, respectively) are designed for broad use-case scenarios, very much broader than the vast majority of enterprise digital solutions actually demands. Liquid stands for the later scenarios and this is the high value space that it tries to fulfill. Therefore, Liquid [prescribes a "light" way of building business application](Key-Concepts.md/#prescriptive-programming-model) without compromising the capability of developers to use the underlying technology in any other ways, if this is a requirement;
- **Multi platform based on workbench components replacement:** Liquid puts itself between application (business) components and underlying platform (technical) components to avoid (or, at least, minimize) vendor and/or technology lock-in of the application, during both building and execution time. Hence there ought to be a way of [simply replacing Liquid "cartridges"](Key-Concepts.md/#Leveling-up-Platform-Providers) of each type (_e.g._ database repository) from one specific technology to another (_e.g._ CosmosDB and DynamoDB) to seamlessly moving application (business) logic from one platform to another. This, obviously, will depend a lot on how the application development team will deal with specific features provided by the underlying technology being used. Most of the time, we only use a small subset of those features, making it very easy to substitute similar technologies. But, even when this is not the case, Liquid helps to isolate those specifics, allowing the creation of portable code with high control, avoiding spreaded infrastructure code that makes maintenance harder;
- **Built-in lightweight patterns for technology domain:** There are many [design patterns](About-Lightweight-Architectures.md) that must be applied to develop a modern digital application. Many of them can be given to the (upper) application layer as an out-of-the box capability of Liquid;
- **Abstract technical complexity from business software developer:** The true hard work of enterprise architects is (or should be) the structuring of millions of lines of code a large company has. Legacy code has probably grown over decades in an unsupervised manner. That should not be the case with the brand-new code of modern digital application. While dealing with technology (delta) innovation/rennovation is a task architects deal every new year, dealing with applications' structuring and refactoring is (or should be) their daily job. The same is valid for business software developers. Liquid helps engineers [to focus on their useful (business) code](Key-Concepts.md/#Business-Logic-Separation).
