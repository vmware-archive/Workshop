# Steeltoe Workshop

**_This workshop was given at SpringOne Platform 2017. It is no longer maintained. For the labs and course with all the new features of Steeltoe. Please contact the [Pivotal Education](https://pivotal.io/education) team. Thank you._**

Hands on workshop describing how to use the Steeltoe components in developing .NET applications for Cloud Foundry.
 
The workshop provides users with a solid understanding of the tools and techniques used to build enterprise-class ASP.NET applications on Cloud Foundry.

It covers topics such as:

* Pivotal Cloud Foundry & Services
* Micro-services using ASP.NET Core
* Centralized application configuration
* Service discovery
* Horizontal scaling
* Fault tolerance using Circuit Breakers
* Security
* Production Management & Monitoring

When following the workshop, you incrementally build and deploy a sample application (i.e. Fortune Teller) which employs all of above tools & techniques.

## Getting started

### Clone Workshop

Start by checking out this repository.  This can be accomplished either through the GitHub website or if you have Git installed, use the following commands:

```bash
> git clone https://github.com/SteeltoeOSS/Workshop
> cd Workshop
```

### Install Prerequisites

* [Cloud Foundry CLI](https://github.com/cloudfoundry/cli)
* [Git Client](https://git-scm.com/downloads)
* [.NET Core SDK 2.0](https://www.microsoft.com/net/download)
* [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2017](https://www.visualstudio.com/downloads/ )
* [Java 8 JDK](https://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html) - Optional, needed to run Eureka and Config servers locally

Follow the steps in Lab 0 to get everything installed and verified.

### Understand Format

1. The descriptions for each lab can be found in each labXX directory. (e.g.  [Lab05 Description](Lab05/README.md))

1. The completed workshop code for each lab can be found in each labXX directory.  (e.g. [Lab5 Completed Code](Lab05/))

1. The final completed workshop code can be found in the [Final](Final/) directory.

1. When starting with the labs for the section _Building Fortune Teller Application_ (i.e. Lab5-Lab11), you should start with the code in the [Start](Start/) directory.

1. You can find the instructors [Slides](Slides/Workshop.pdf) in the repository.

1. The outline for the workshop:

   _Intro to Pivotal Cloud Foundry_
   * [Lab 0 - Install Prerequisites & Log into Cloud Foundry](Lab00/README.md)
   * [Lab 1 - Running .NET Application on Cloud Foundry](Lab01/README.md)
   * [Lab 2 - Creating and Binding to Cloud Foundry Services](Lab02/README.md)
   * [Lab 3 - Scaling and Operating Applications](Lab03/README.md)
   * [Lab 4 - Monitoring Applications](Lab04/README.md)

   _Building Fortune Teller Application_
   * [Lab 5 - ASP.NET Core Programming Fundamentals](Lab05/README.md)
   * [Lab 6 - Centralized Application Configuration - Config Server](Lab06/README.md)
   * [Lab 7 - Service Discovery - Eureka Server](Lab07/README.md)
   * [Lab 8 - Scaling Horizontally - Redis and Mysql Services](Lab08/README.md)
   * [Lab 9 - Fault Tolerance & Monitoring - Hystrix Circuit Breakers](Lab09/README.md)
   * [Lab 10 - Securing Application Endpoints - OAuth2 and JWT Tokens](Lab10/README.md)
   * [Lab 11 - Production Monitoring & Management - Pivotal Apps Manager](Lab11/README.md)
