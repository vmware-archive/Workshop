# Lab 10 - Securing Service Endpoints

>In this lab we will continue to add functionality to the Fortune Teller application. In this lab we will learn how to add security to our application. We will secure our `Fortune Teller Service` REST endpoints so that you must be authenticated and have `read.fortunes` permission to access it.  We will also implement a OAuth2 `Log In` functionality in our `Fortune Teller UI` which uses the Cloud Foundry identity store for authentication and permissions.

>After completing Lab 9, the app in its current state is as follows:

* The application can now scale horizontally, and has fault tolerance built in.
* But, unfortunately every one has access to the Fortune service. ;-)

>The goals for Lab 10 are to:

* Change `Fortune Teller Service` to secure the REST endpoints by requiring valid OAuth Bearer tokens with `read.fortunes` permission to access Fortunes.
* Change `Fortune Teller UI` to require users to authenticate using the Cloud Foundry UAA before fetching a Fortune and to have `read.fortunes` permission to access Fortunes.

>For some background information on ASP.NET Core Security, have a look at this [documentation](https://docs.microsoft.com/en-us/aspnet/core/security/)

## Add Security to Fortune Teller Service

In this section we will be adding code to the `Fortune Teller Service` to use JWT Bearer token based authentication and authorization.

### Step 01 - Add Steeltoe Security Nuget to Fortune Service

Make changes to your `Fortune Teller Service` project file to include the Steeltoe Security NuGet.

### Step 02 - Add Authentication to Fortune Service

Make changes to `Startup.cs` to add ASP.NET Core Authentication services to the container and add the CloudFoundry JWT Bearer token security provider to the AuthenticationBuilder.

### Step 03 - Use Authentication in Fortune Service

Make changes to `Startup.cs` to use the above added Authentication services in the pipeline.

### Step 04 - Add Authorization to Fortune Service

Make changes to your `Startup.cs` class to add ASP.NET Core Authorization services to the container and a single authorization policy that requires a `scope` claim with the value `read.fortunes`.

### Step 05 - Add Security to Fortune Service FortunesController

Make changes to `FortunesController.cs` to secure access to all the REST endpoints by requiring the above security policy.

### Step 06 - Run Locally - Fortune Service

Run and verify the Fortune Teller Service and verify you no longer have access to the REST endpoints, and in fact they return a 401. Run the application either in a command window or within VS2017.

## Add Security to Fortune UI

In this section we will be adding code to the `Fortune Teller UI` to use the Cloud Foundry UAA Server for authentication and authorization by implementing the OAuth2 Authorization_Code security flow.

### Step 01 - Add Steeltoe Security Nuget to Fortune UI

Make changes to your `Fortune Teller UI` project file to include the Steeltoe Security NuGet.

### Step 02 - Add Authentication to Fortune UI

Make changes to `Startup.cs` to add ASP.NET Core Authentication services to the container and add the CloudFoundry OAuth security provider to the AuthenticationBuilder.

Also we will want to add the Cookie authentication provider to the builder and configure Cookies to be the default authentication scheme. We want cookies to be used to store authenticated credentials for the user.

When a user is not authenticated, we want the Cloud Foundry provider to be used to handle challenged requests, so configure the CloudFoundry provider to be the default challenge scheme.

### Step 03 - Add Authentication to Fortune UI

Make changes to `Startup.cs` to use the above added Authentication services in the pipeline.

### Step 04 - Add Authorization to Fortune UI

Make changes to your `Startup.cs` class to add ASP.NET Core Authorization services to the container and a single authorization policy that requires a `scope` claim with the value `read.fortunes`.

### Step 05 - Add Security to Fortune UI FortunesController

Make changes to `FortunesController.cs` to secure access to the following actions:

* `RandomFortune()` - require `read.fortunes` permissions.
* `Login()` - require authenticated user - forces redirect to UAA for auth.

### Step 06 - Forward Access Token to Fortune Service

Make changes to the `FortuneServiceClient.cs` to include the Bearer token (i.e. Access Token) for the authenticated user to the Fortune Service.

Note: In order to accomplish this, the client will need access to `IHttpContextAccessor` in order to access the authenticated user and the Access Token.

## Verify on Cloud Foundry

### Step 01 - Create OAuth Service Instance

To create an instance of a Hystrix dashboard service in your org/space follow these instructions:

1. Open a command window.

1. Using the command window, create an instance of the Hystrix dashboard on Cloud Foundry.

   Note: Before you can issue this command you will need to replace the `xxxxxxx` with the appropriate values for your environment.  Ask your instructor for those values.

   ```bash
   > cf cups myOAuthService -p "{\"client_id\": \"xxxxxxxxxx\",\"client_secret\": \"xxxxxxxxxx\",\"uri\": \"uaa://login.xxxxx.xxxxx.com\"}"
   ```

1. Wait for the service to become available on Cloud Foundry.

   ```bash
   > cf services
   ```

### Step 02 - Configure Service Binding

You need to configure both of your applications to bind to the OAuth service instance you created above.

Open the `manifest.yml` file for in each project and add to the services section the OAuth instance you created above.

### Step 03 - Using Self-Signed Certificates

In some cases you may find that your Cloud Foundry setup has been installed using self-signed certificates. If that is the case, you will likely run into certificate verification errors when communicating with the UAA server. If that is the case you can disable certificate validation by adding `security:oauth2:client:validate_certificates=false` to your configuration file.

Check with your instructor to see if you need to do this.

### Step 04 - Push to Cloud Foundry

Publish, push and verify the Fortune Teller application still runs on Cloud Foundry. At this point, if your application is running properly, you will have to login before obtaining a Fortune.
