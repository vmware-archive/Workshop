# Lab 8 - Scaling Horizontally

>In this lab we will continue to add functionality to the Fortune Teller application. We will explore some of the existing horizontal scaling issues with the app and how Steeltoe connectors and data protection providers can help solve those issues.

>After completing Lab 7, the app state should be as follows:

* Both components have their configuration centrally maintainable.
* Both components are using a service registry for registrations and discovery.
* The `Fortune Teller Service` can not scale horizontally, as it uses a back-end in-memory database to hold Fortunes.
* The `Fortune Teller UI` can not scale horizontally, as its session state (i.e. Users Fortune) gets lost since the Session is not shared between instances.

>The goals for Lab 8 are to:

* Change `Fortune Teller Service` to use MySql database to store Fortunes
* Change `Fortune Teller UI` to use Redis for its Session storage.
* Change `Fortune Teller UI` to use Redis to store its Data Protection key ring.

>For background information, see: [ASP.NET Core Session State documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state) and [ASP.NET Core Data Protection documentation](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/introduction).

## Preparation

### Step 01 - Run Config Server Locally

We are still using the Config Server, so make sure it is running locally so its easier to develop and test with.

1. Open a command window and change directory to _Workshop/ConfigServer_

   ```bash
   > cd Workshop/ConfigServer
   ```

1. Startup the Config Server

   ```bash
   > mvnw spring-boot:run
   ```

   It will start up on port 8888 and serve configuration data from `Workshop/ConfigServer/steeltoe/config-repo`.

### Step 02 - Run Eureka Server Locally

Here we do the steps to setup and run a Eureka Server locally so its easier to develop and test with.

1. Open a command window and change directory to _Workshop/EurekaServer_

   ```bash
   > cd Workshop/EurekaServer
   ```

1. Startup the Eureka Server

   ```bash
   > mvnw spring-boot:run
   ```

It will start up on port 8761 and serve the Eureka API from "/eureka".

## Use MySQL in Fortune Teller Service

In this section we will be adding code to the `Fortune Teller Service` in order to make use of the `Steeltoe MySQL Connector`. We will use it to connect to a back-end MySql database to store Fortunes in it.

### Step 01 - Add Steeltoe Connector NuGet to Service project

Make changes to your `Fortune Teller Service` project file to include the Steeltoe Connector NuGet.

>**Note:** in addition to the Steeltoe NuGet, you will also need to include a supported Entity Framework Core NuGet to go with it. For this workshop use `Pomelo EntityFrameworkCore` NuGet.

### Step 02 - Use Steeltoe MySql Connector

Ideally, we would use a local MySQL instance on our desktop when running the Fortune Teller Service locally in `Development` mode, and a provisioned instance of MySql when we pushed the app to Cloud Foundry.

If this were the case, we could simply remove the In-memory database we setup in `Startup.cs` and replace it with the Steeltoe Connector and appropriate ADO.NET provider (e.g. `Pomelo`).  Then we would configure the Steeltoe Connector's local MySQL connection information, either in `appsettings.json` or in `Workshop/ConfigServer/steeltoe/config-repo/fortuneservice.yml`. The configuration for the Connector would look something like below:

```json
{
  "spring": {
    "application": {
      "name": "fortuneservice"
    },
    "cloud": {
      "config": {
        "uri": "http://localhost:8888"
      }
    }
  },
  "mysql": {
    "client": {
      "database": "mydatabase",
      "username": "username",
      "password": "password"
    }
  }
}
```

Under this ideal situation, with the above configuration setup, the Steeltoe Connector would use the above configuration when launched locally, but when pushed to Cloud Foundry, it would override this configuration with the MySql service binding it finds on Cloud Foundry.

But, since we are not running MySQL database locally, we will instead want to configure things to use the In-Memory database in `Development` mode (i.e. running locally), and use a MySql database (via the Steeltoe Connector) when running in any other mode (i.e. running on Cloud Foundry - `Production`)

So for this step, make the changes to your `Startup.cs` file to accomplish this. Use In-Memory in `Development` mode and use MySql in `Production`.

### Step 03 - Run Service Locally

Run and verify both Fortune-Tellers continue to run as they did before. Run the application in either a command window or VS2017. Everything should work as it did before as you will still be using the In-Memory database when running locally.

### Step 04 - Create MySQL Service Instance

To create an instance of a MySql database service in your org/space follow these instructions:

1. Open a command window.

1. Using the command window, create an instance of the MySql database on Cloud Foundry.

   ```bash
   > cf create-service p-mysql 100mb myMySqlService
   ```

1. Wait for the service to become available on Cloud Foundry.

   ```bash
   > cf services
   ```

### Step 05 - Configure Service Binding

You need to configure your `Fortune Teller Service` to bind to the database service instance you created above.

Open the `manifest.yml` file for the `Fortune Teller Service` and add the database instance you created above to the services section.

### Step 06 - Push Service to Cloud Foundry

Publish, push and verify the Fortune Teller Service still runs on Cloud Foundry using the new MySql database.

## Use Redis for Session Storage in Fortune Teller UI

In this section we will be adding code to the `Fortune Teller UI` to make use of the Steeltoe Redis connector.
We will use it to hook up the ASP.NET Core `IDistributedCache` to a Redis service instance. Remember, Session uses the `IDistributedCache` to store session state.

>For background information, see [ASP.NET Core Sessions and Caching documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?tabs=aspnetcore2x).

### Step 01 - Add Steeltoe Connector NuGet to UI project

Make changes to your `Fortune Teller UI` project file to include the Steeltoe Connector NuGet.

### Step 02 - Use Steeltoe Redis Connector

Like the case above with MySql, the Steeltoe Redis connector can be used to connect to a local Redis instance for development or to a bound Redis service instance when pushed to Cloud Foundry. Below is a sample configuration you could use if using the connector to connect local:

```json
{
  "spring": {
    "application": {
      "name": "fortuneui"
    },
    "cloud": {
      "config": {
        "uri": "http://localhost:8888",
      }
    }
  },
  "redis": {
    "client": {
      "host": "http://foo.bar",
      "port": 1111
    }
  }
}
```

But, just like the case with MySQL, we are not running Redis locally, so we will instead have to configure things similar to MySql situation above. We will configure things to use an In-Memory cache when in `Development` mode (i.e. running locally), but then use a Redis cache (via the Steeltoe Connector) when running in any other mode (i.e. running on Cloud Foundry - `Production`)

For this step, make the changes to your `Startup.cs` to accomplish what is described above.

### Step 03 - Run UI Locally

Run and verify both Fortune-Tellers continue to run as they did before. Run the application in either a command window or VS2017. Everything should work as before since you are still using the In-Memory cache when running locally.

## Use Redis for Data Protection Key Storage

In this exercise we will be adding code to the `Fortune Teller UI` to make use of the Steeltoe Redis DataProtection Key Storage provider.
We will use it to configure the ASP.NET Core DataProtection service to persist its keys to a Redis service instance.

>For background information, see [Configuring ASP.NET Core Data Protection](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview).

### Step 01 - Add Steeltoe Redis DataProtection Key Storage provider NuGet

Make changes to your `Fortune Teller UI` project file to include the Steeltoe Redis DataProtection Key Storage provider NuGet.

### Step 02 - Use Steeltoe Redis DataProtection Key Storage provider

The default for ASP.NET Core DataProtection services is to store its key ring in a file, local to the running machine. Of course, this will not work well when we want to scale the app horizontally. (i.e. Running multiple instances of the `Fortune-Teller-UI`).

To change this behavior, we are going to configure DataProtection service to use a Redis cache to store its key ring and also automatically configure what Redis cache it uses.

There are two steps to configuring this:

1. Use the Steeltoe Redis connector to add a StackExchange Redis Connection Multiplexer to the container.
1. Instruct the DataProtection service to persist its keys to a Redis cache.

Now, since we are going to use Redis for the backing store and we are not running Redis locally, we will want to wrap the Data Protection configuration in an if statement so we don't use any of this when in `Development` mode (i.e. running locally)

For this step, make the changes to your `Startup.cs` to accomplish what is described above.

### Step 03 - Run Locally

Run and verify both Fortune-Tellers continue to run as they did before. Run the application in either a command window or VS2017. Everything should work as before, since you will still be using the default DataProtection provider which stores keys to local file system.

### Step 04 - Create Redis Service Instance

To create an instance of a Redis cache service in your org/space follow these instructions:

1. Open a command window.

1. Using the command window, create an instance of the Redis cache on Cloud Foundry.

   ```bash
   > cf create-service p-redis shared-vm myRedisService
   ```

1. Wait for the service to become available on Cloud Foundry.

   ```bash
   > cf services
   ```

### Step 05 - Configure Redis Service Binding

Configure your `Fortune Teller UI` to bind to the Redis service instance you created above by opening the `manifest.yml` and adding the Redis instance information to the services section.

### Step 06 - Push UI to Cloud Foundry

Publish, push and verify the Fortune Teller application still runs on Cloud Foundry using the new Redis instance.

### Step 07 - Scale

Scale both of the apps to 2 instances and see if the fortune cached in session remains accessible.

---
Continue the workshop with [Lab 9 - Fault Tolerance & Monitoring - Hystrix Circuit Breakers](../Lab09/README.md)