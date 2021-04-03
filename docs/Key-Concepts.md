| [Main](About-Liquid) > Key Concepts |
|----|

A small group of core concepts balances the many aspects dealt by Liquid and determines its general shape and strengths.


## Architectural Strategies
- [**Platform abstraction layer:**](Platform-Abstraction-Layer.md) using the best from PaaS offerings while avoiding lock-in;

- [**Business logic seggregation:**](Business-logic-seggregation.md) avoid mixing domain (business) logic along with underlying platform (technical) logic;

- [**DevOps for microservices:**](DevOps-for-Microservices.md) giving this fine-grained unit of software an easier way of being built, deployed and tested in a continuous fashion.


## Prescriptive Programming Model
- [**APIs as (formal) object models:**](API-as-formal-Object-Model.md) leveraging the abstraction and formality of a well-formed API as a way to avoid code redundancy and error propensity;

- [**Encapsulated domain (business) logic:**](Encapsulated-Domain-Logic.md) putting domain logic in a format (of classes) independent of the protocols used to access it;

- [**Business error handling:**](Business-Error-Handling.md) defining a simple yet comprehensive way of dealing with business logic deviation;

- [**Microservice sizing:**](Microservice-Sizing.md) giving shape to the various parameters of a well-designed microservice.
