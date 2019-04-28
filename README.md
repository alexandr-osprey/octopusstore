# Osprey Store

Osprey Store is web store platform providing marketplace for the sellers and the customers.  It was created with a purpose to became familiar and implement software with some fancy frameworks and patterns. Inspired by <a href="https://github.com/dotnet-architecture/eShopOnWeb">Microsoft reference solution</a>.
Please note that it was desined to look good on any screen.
I hope you can find some useful pieces of code there.

Frameworks used
---
- ASP.NET Core 2.1
- NET Standard 2.0
- Angular 7
- Bootstrap 4

Core patterns, principles and features
---
- SOLID principles
- Testability
- Specification pattern
- RESTful web services
- JSON Web Tokens Authentication
- ASP NET Core Authorization
- Bootstrap Flexbox Adaptive Layout

### SOLID

#### S—Single responsibility principle
Each component designed for one thing and has minimal knowledge of outside components and their logic. For example, each service is responsible for lifecycle of it's entity, and supposed to be changed if and only if this lifecycle changes. Although one component may depend on the other one, implementation details are hidden behind it's abstractions.
![SingleResponsibility1](docs/images/SingleResponsibility1.jpg)

#### O—Open/closed principle
Services and controllers created with maximum code reusing possibily in mind. Base classes describe common processes, while child classes are free to extend and reuse it if needed. 
For example, generally the process of creating an entity is similar between different services.
![OpenClosedPrinciple1](docs/images/OpenClosedPrinciple1.jpg)

When we need to add file to storage while creating an entity (ItemVariantImageService), this general process could be easily extended while still keeping and reusing the original logic
![OpenClosedPrinciple2](docs/images/OpenClosedPrinciple2.jpg)

#### L-Liskov substitution principle
Controllers rely on services to create/read/update/delete entities. Although the internal logic may differ greatly from service to service, in the end it's just generic controller calling generic service while passing some arguments.
![LiskovSubstitutionPrinciple1](docs/images/LiskovSubstitutionPrinciple1.jpg)
![LiskovSubstitutionPrinciple2](docs/images/LiskovSubstitutionPrinciple2.jpg)

#### I-Interface segregation principle
As already mentioned, service may rely on other services, but only does so through interfaces. In this example, ItemService requires ICategoryService to filter by category and IItemVariantImageService to maintain database consistency.
![InterfaceSegregationPrinciple1](docs/images/InterfaceSegregationPrinciple1.jpg)

#### D-Dependency inversion principle
Accrording to Clean Architecture principles, the most important one is the dependancy inversion principle which is the pillar of the clean architecture.
While there are still some references from OspreyStore to Infrastructure, it's only limited to database seeding and DI setup, as it's shown on a graph below (because eliminating this project refrenece completely seemed rather complex and verbose by standard NET Core DI).
![DependencyInversionPrinciple1](docs/images/DependencyInversionPrinciple1.jpg)


### Testability
Creating testable code following aforementioned principles is rather easy. Tests also reuse code from base classes extensively, only overriding what exacly should be tested, and by which criteria, which in turn allows to create new tests in no time.
![Testability1](docs/images/Testability1.jpg)


### Specification pattern
Avoing code duplication, keeping interface definitions rather simple in a clean and concise way only possible thanks to EF Core flexibility.
Complex conditions and nested joins are parts of separate classes, not retrieval logic, which allows to reuse code as much as possible.
![SpecificationPattern1](docs/images/SpecificationPattern1.jpg)


### RESTful web services
Each entity has a separate controller with methods whose URIs conform to the defenition of RESTful JSON API.


### Installing

Just clone and run server, no special actions required

