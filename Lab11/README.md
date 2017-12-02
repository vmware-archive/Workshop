# Lab 11 - Production Monitoring & Management

>In this lab we will continue to add functionality to the Fortune Teller application. We will learn how to add management endpoints to our application which integrate with and can be accessed from the Pivotal AppsManager.

>After completing Lab 10, the app state should be as follows:

* The application is now secure; only authenticated users with the right permissions can obtain a Fortune.
* But, we are missing a key piece of functionality; the tools to help manage and monitor the app while in production.

>The goals for Lab 11 are to:

* Add several management endpoints to both apps to give us visibility into its operation in production:
  * Git build information
  * Adjustable logging levels
  * Application specific health checks
  * Trace of last 100 requests

## Add Management Endpoints to Fortune Teller Service

In this section we will be making the changes to the `Fortune Teller Service` to expose the above mentioned endpoints.

### Step 01 - Add Steeltoe Management Nuget to Fortune Service

Make changes to your `Fortune Teller Service` project file to include the Steeltoe Management NuGet.

### Step 02 - Add Management Endpoints to Fortune Service

Make changes to `Startup.cs` to add all of the management endpoints to the service container.

### Step 03 - Use Management Endpoints in Fortune Service

Make changes to `Startup.cs` to use the above added endpoint services in the pipeline.

### Step 04 - Configure the Path for Management Endpoints

Make changes to your `application.yml` file and configure the endpoint path to `/cloudfoundryapplication`. This is the path the Pivotal AppsManager expects the endpoints to be.

>**Note:** we are asking you to add this to `application.yml`, since this will also be needed by the `Fortune Teller UI`.

### Step 05 - Create a MySqlHealthContributor

Add an implementation of a `IHealthContributor` which when called, will check the health of the connection to the MySql database. Call this contributor, `MySqlHealthContributor` and place this new class in the projects folder.

This contributor should make use of an injected `FortuneContext` to test that it can connect to the back-end database by executing a `SELECT 1;` against it.  Have it return the status of the command along with its status computed status.

### Step 06 - Add MySqlHealthContributor to Container

Make changes to your `Startup.cs` class to add the `MySqlHealthContributor` to the service container.

### Step 07 - Add GitInfo to Fortune Service

Make changes to your `Fortune Teller Service` project file to make use of GitInfo to create the `git.properties` file for the project so that it will be returned by the Info endpoint.

### Step 08 - Replace Console Logging Provider

Make changes to your `Program.cs` file to replace usage of the ASP.NET Core Console Logging provider with the Steeltoe Console provider that enables listing and changing the logging levels of all created loggers.

### Step 09 - Push to Cloud Foundry

Publish, push and verify the `Fortune Teller Service` exposes the management endpoints.

## Add Management Endpoints to Fortune Teller UI

In this section we will be making the changes to the `Fortune Teller UI` to expose the above mentioned endpoints.

### Step 01 - Add Steeltoe Management Nuget to Fortune UI

Make changes to your `Fortune Teller UI` project file to include the Steeltoe Management NuGet.

### Step 02 - Add Management Endpoints to Fortune UI

Make changes to `Startup.cs` to add all of the management endpoints to the service container.

### Step 03 - Use Management Endpoints in Fortune UI

Make changes to `Startup.cs` to use the above added endpoint services in the pipeline.

### Step 04 - Add GitInfo to Fortune UI

Make changes to your `Fortune Teller UI` project file to make use of GitInfo to create the `git.properties` file for the project so that it will be returned by the Info endpoint.

### Step 05 - Replace Console Logging Provider

Make changes to your `Program.cs` file to replace usage of the ASP.NET Core Console Logging provider with the Steeltoe Console provider that enables listing and changing the logging levels of all created loggers.

### Step 06 - Push to Cloud Foundry

Publish, push and verify the `Fortune Teller UI` exposes the management endpoints.

---
Congratulations, you've completed the final lab! Join us on [Slack](https://slack.steeltoe.io) to tell us how it went!