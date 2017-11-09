# Lab 1 - Pushing Your First ASP.NET Core Application

>In this lab we will publish and push a pre-coded ASP.NET Core MVC application to Cloud Foundry. The application makes use of the __Steeltoe CloudFoundry Configuration provider__ to parse the JSON based configuration data provided by Cloud Foundry as environment variables making it available to the application at runtime.

>Note: We will use this same application for all of the rest of the labs in this section of the workshop (i.e. labs1-4).

## The Application

This is the first application we will look at that is leveraging Steeltoe.

This application was created using the out of the box ASP.NET Core MVC template found in the dotnet CLI. The code generated was then modified to make use of the [Steeltoe CloudFoundry Configuration provider](https://github.com/SteeltoeOSS/Configuration/tree/master/src/Steeltoe.Extensions.Configuration.CloudFoundry[CloudFoundry). The Steeltoe component is used in the application to access the configuration data provided to the application by CloudFoundry environment variables, `VCAP_APPLICATION`, `VCAP_SERVICES` and `CF_*` as application configuration data.

## Some Background

When Microsoft developed ASP.NET Core, the next generation of ASP.NET, they created a number of new `Extension` frameworks which provide services(e.g. Configuration, Logging, Dependency Injection, etc) commonly used or needed when building applications. While these `Extensions` can certainly be used in ASP.NET Core applications, they can also be leveraged in other application types as well; including ASP.NET 4.x, Console, UWP Apps, etc.

As you will see during this workshop, the Steeltoe project has added a couple of additional configuration providers to the list of those offered from Microsoft. These include:

* [CloudFoundry Configuration](https://github.com/SteeltoeOSS/Configuration/tree/master/src/Steeltoe.Extensions.Configuration.CloudFoundry)

* [Config Server Client](https://github.com/SteeltoeOSS/Configuration/tree/master/src/Steeltoe.Extensions.Configuration.ConfigServer)

To get a better understanding of the `Microsoft Configuration Extensions` have a look at the [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration) on the subject.

As you might expect, virtually all of Steeltoe components make use of the CloudFoundry configuration provider in one way or another.

Later on in the workshop we will take a much closer look at the `Configuration Extensions` and the `Steeltoe CloudFoundry Configuration provider`.

## Download Workshop Code

If you haven't already done so, download or clone the Workshop git repository using the command below. If you don't have git installed, you can click 'Download ZIP' on the right side of the page https://github.com/SteeltoeOSS/Workshop[here].

```bash
> git clone https://github.com/SteeltoeOSS/Workshop
```

## Target Cloud Foundry

If you haven't already done so, set the Cloud Foundry target for the CLI.

Note: Use the appropriate HTTP URL for your workshop environment.

```bash
> cf api https://api.run.haas-76.pez.pivotal.io --skip-ssl-validation
```

## Login to Cloud Foundry

Follow the prompts using the credentials you were provided by your instructor.

```bash
> cf login
```

## Install .NET Core SDK

If you haven't already done so, download and install the [.NET Core SDK](https://www.microsoft.com/net/download).

   ---

   ![login](../../Common/images/lab-01-dotnet-install.png)

   ---

## Publish and Push the Application

1. Open a command prompt and cd _Workshop/Session-01/Lab01/CloudFoundry_.  Note in this lab we are not going to write any code, we will be using a pre-coded application.

   ```bash
   > cd Workshop/Session-01/Lab01/CloudFoundry
   ```

1. Use the dotnet CLI to restore the NuGet references used by the application.

   ```bash
   > dotnet restore --configfile nuget.config
   ```

1. Publish the application getting it ready to push to Cloud Foundry. Notice where it creates the published application as you will have to reference that directory in the next step.  Notice that we are specifying a specific runtime by using `-r ubuntu.14.04-x64` during our publish as we will be pushing this application to a Linux cell.

   ```bash
   > dotnet publish -r ubuntu.14.04-x64
   ```

1. Push the application to Cloud Foundry specifying the manifest to use to configure the application (`-f manifest.yml`) and the directory (`-p bin\Debug\netcoreapp2.0\ubuntu.14.04-x64\publish`) containing the published application.

   ```bash
   cf push -f manifest.yml -p bin\Debug\netcoreapp2.0\ubuntu.14.04-x64\publish
   ```
1. Observe the output in the command window created by the `cf push`.  You should see something similar to the following:

   ```bash
   ----
   Using manifest file manifest.yml

   Creating app env in org test / space test as admin...
   OK

   Creating route env-peperine-flannelette.apps.testcloud.com...
   OK

   Binding env-peperine-flannelette.apps.testcloud.com to env...
   OK

   Uploading env...
   Uploading app files from: /Users/DaveT/workspace/dotnet/Workshop/Session-01/Lab01/CloudFoundry/publish/
   Uploading 9.6M, 291 files
   Done uploading
   OK

   Starting app env in org test / space test as admin...
   Downloading binary_buildpack...
   Downloading nodejs_buildpack...
   Downloading staticfile_buildpack...
   Downloading java_buildpack_offline...
   Downloading ruby_buildpack...
   Downloaded java_buildpack_offline
   Downloading python_buildpack...
   Downloaded staticfile_buildpack
   Downloading go_buildpack...
   Downloaded nodejs_buildpack
   Downloading php_buildpack...
   Downloaded binary_buildpack
   Downloaded ruby_buildpack
   Downloading dotnet_core_buildpack...
   Downloaded go_buildpack
   Downloaded php_buildpack
   Downloaded dotnet_core_buildpack
   Downloaded python_buildpack
   Creating container
   Successfully created container
   Downloading app package...
   Downloaded app package (24M)
   Staging...
   -----> Buildpack version 1.0.12
   ASP.NET Core buildpack version: 1.0.12
   ASP.NET Core buildpack starting compile
   -----> Restoring files from buildpack cache
       OK
   -----> Restoring NuGet packages cache
       OK
   -----> Extracting libunwind
       libunwind version: 1.2
       file:///tmp/buildpacks/0d9923c85070a88b0dde407ea8202d62/dependencies/https___buildpacks.cloudfoundry.org_dependencies_manual-binaries_dotnet_libunwind-1.2-linux-x64-f56347d4.tgz
       OK
   -----> Saving to buildpack cache
       Copied 38 files from /tmp/app/libunwind to /tmp/cache
       OK
   -----> Cleaning staging area
       OK
   ASP.NET Core buildpack is done creating the droplet
   Exit status 0
   Staging complete
   Uploading droplet, build artifacts cache...
   Uploading droplet...
   Uploading build artifacts cache...
   Uploaded build artifacts cache (992K)
   Uploaded droplet (24.2M)
   Uploading complete
   Destroying container
   Successfully destroyed container

   1 of 1 instances running

   App started

   OK

   App env was started using this command `cd . && ./CloudFoundry --server.urls http://0.0.0.0:${PORT}`

   Showing health and status for app env in org test / space test as admin...
   OK

   requested state: started
   instances: 1/1
   usage: 1G x 1 instances
   urls: env-peperine-flannelette.apps.testcloud.com
   last uploaded: Wed Mar 15 20:57:22 UTC 2017
   stack: cflinuxfs2
   buildpack: ASP.NET Core (buildpack-1.0.12)

     state     since                    cpu    memory    disk      details
   #0   running   2017-03-15 02:57:55 PM   0.0%   0 of 1G   0 of 1G

   ```

## Understanding Cloud Foundry Push

When you push your application to Cloud Foundry there are a number of things to be aware of.

* The Cloud Foundry CLI uses the manifest to provide the necessary configuration details to Cloud Foundry for the application. Things such as the application name, the memory to be allocated, the operating system to be used (in this case Linux), the number of instances to start, the environment variables to set, and the routes to use when accessing the application. Take a minute and open up `manifest.yml` to see how this is done.  There are two manifests for the application, one to be used when targeting Linux and the other for Windows. You can find them in  `Workshop/Session-01/Lab01/CloudFoundry`.

* In most cases, the CLI indicates each Cloud Foundry API call as it happens. In this case, the CLI has created an application named _env_ and has started it in your assigned Cloud Foundry space.

* All HTTP/HTTPS requests to the applications flows through Cloud Foundry's front-end router called [(Go)Router](https://docs.pivotal.io/pivotalcf/1-7/concepts/architecture/router.html). Here the CLI is creating a route with random word tokens inserted (again, see `manifest.yml` for a hint!) to prevent route collisions across the default domain.

* The CLI is also _binding_ the created route to the application.Routes can actually be bound to multiple applications to support techniques such as [blue-green deployments](https://docs.pivotal.io/pivotalcf/1-7/devguide/deploy-apps/blue-green.html).

* The CLI finally uploads the application bits to Cloud Foundry. Notice that it's uploading several _files_, all those found in the publish directory! This is because Cloud Foundry actually uploads all the files for the deployment for caching purposes.

* When Cloud Foundry starts the application it first must begin the staging process. The staging process prepares the application to run on Cloud Foundry. Cloud Foundry will create two containers, one to stage the application and then a second to actually run or host the prepared bits.

* The final package of your application that is created as a result of the staging process contains all of its necessary runtime bits needed for the application to run. In Cloud Foundry terminology we refer to this as a _droplet_. You will notice from the output that the droplet is being uploaded to Pivotal Cloud Foundry's internal blob store so that it can be easily copied/replicated to one or more [Diego Cells](https://docs.pivotal.io/pivotalcf/1-7/concepts/diego/diego-architecture.html) for execution.

* The CLI tells you exactly what command and argument will be used to start your application.

* Finally the CLI reports the current status of your application's health, including the number of instances started, what stack it's running on and the URL(s) that can be used to access it.

## View Application in AppsManager

1. Open AppsManager and select your org and space:

   ---

   ![apps manager](../../Common/images/lab-01-appsmanager.png)

   ---

1. Select the `env` application and then select the `Settings` tab.

   ---

   ![apps manager](../../Common/images/lab-01-appsmanager-env-variables.png)

   ---

1. Click the `Reveal Env Vars` button to see the environment variables for the application.

   Notice the environment variables `VCAP_APPLICATION` and `VCAP_SERVICES`. These are assigned by CloudFoundry and are intended to provide configuration data for the application. You should see something similar to what you see below:

```text
{
  "staging_env_json": {},
  "running_env_json": {},
  "environment_json": "invalid_key",
  "system_env_json": {
    "VCAP_SERVICES": {}
  },
  "application_env_json": {
    "VCAP_APPLICATION": {
      "cf_api": "https://api.system.testcloud.com",
      "limits": {
        "fds": 16384,
        "mem": 1024,
        "disk": 1024
      },
      "application_name": "env",
      "application_uris": [
        "env-uninebriating-impaler.apps.testcloud.com"
      ],
      "name": "env",
      "space_name": "test",
      "space_id": "86111584-e059-4eb0-b2e6-c89aa260453c",
      "uris": [
        "env-uninebriating-impaler.apps.testcloud.com"
      ],
      "users": null,
      "application_id": "c21b464e-243a-43fc-86b2-1545c90e2239",
      "version": "e5f8aff9-4434-4f54-a4c4-c84569c3d8b3",
      "application_version": "e5f8aff9-4434-4f54-a4c4-c84569c3d8b3"
    }
  }
}
```

## Interact with the Application

1. Visit the application in your browser by hitting the route that was generated by the CLI.  Do a `cf a` to see what URL to use if your unsure of what the route is for the application. You should see something like shown below.

   ---

   ![env-1](../../Common/images/lab-net.png)

   ---

1. Click on the `CloudFoundry Config` menu item. You should see something like shown below.

   ---

   ![env-1](../../Common/images/lab-01-cloudfoundry-config.png)

   ---

   What you are seeing is the configuration information from `VCAP_APPLICATION` and `VCAP_SERVICES`. Take some time and see if you can find in the code how this is accomplished. Start with the `CloudFoundryConfig()` action in the `HomeController`.

1. Click on the `Application Config` menu item. You should see something like shown below.

   ---

   ![env-2](../../Common/images/lab-01-application-config.png)

   ---

   What you are seeing is the configuration information from `appsettings.json` and `appsettings-Development.json`, both configuration files found in the application solution. Take some time and see if you can find in the code how this is accomplished. Start with the `AppConfig()` action in the `HomeController`.

1. Click on the `Subsection Config` menu item. You should see something like shown below.

   ---

   ![env-3](../../Common/images/lab-01-subsection-config.png)

   ---

   What you are seeing is the configuration information from a  subsection of `appsettings.json` and `appsettings-Development.json`. Take some time and see if you can find in the code how this is accomplished. Start with the `SubSectionConfig()` action in the `HomeController`.

1. Click on the `Raw Config` menu item. You should see something like shown below.

   ---

   ![env-4](../../Common/images/lab-01-raw-config.png)

   ---

   What you are seeing is the raw listing of all the configuration information available to the application. Take some time and see if you can find in the code how this is accomplished. Start with the `RawConfig()` action in the `HomeController`.

1. To see how the Cloud Foundry configuration data is integrated into the apps configuration, take a look at the code in `Program.cs`.  Have a look at the `CreateDefaultBuilder(args)` call and you will also see the usage of a `AddCloudFoundry()` method call. This is what causes the Cloud Foundry configuration data to be added to the apps configuration.  For more detail on ASP.NET Core host building,  read over the ASP.NET documentation - [Setting up a Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/hosting?tabs=aspnetcore2x) .

## Interact with Application from CF CLI

1. Get information about the currently deployed application using CLI apps command:

   ```bash
   > cf apps
   ```

   Note the application name for next steps.

1. Get information about running instances, memory, CPU, and other statistics using CLI instances command.

   ```bash
   > cf app env
   ```

1. Stop the deployed application using the CLI.

   ```bash
   > cf stop env
   ```

1. Delete the deployed application using the CLI

   ```bash
   > cf delete env
   ```