# Lab 2 - Binding to Cloud Foundry Services

>In this lab, we'll be binding a MySQL database service to the application we pushed earlier and then view the configuration data Cloud Foundry provides to the application as a result. In a future lab we will see how easy it is to use __Steeltoe Connectors__ to actually make use of the MySql service in your application

## Preparation

If your instance of the sample _env_ is not running after the steps completed in lab 1.  Make sure to follow the instructions in lab01 to deploy the application again.

After you have the application deployed and started, visit the application in your browser by hitting the route that was assigned to it.  Select the `CloudFoundry Config` menu item and notice the application shows there are not any Cloud Foundry Services listed.

   ---

   ![env-4](../../Common/images/lab-02-cloudfoundry-config.png)

   ---

In this lab we are going to change that.

## Managing Services

Cloud Foundry services are managed through two primary types of CLI operations.

* Create/Delete - These operations create or delete instances of a service. For a database this could mean creating/deleting a schema in an existing multitenant cluster or creating/deleting a dedicated database cluster.

  ```bash
  > cf create-service SERVICE_PLAN SERVICE_INSTANCE [-c PARAMETERS_AS_JSON] [-t TAGS]
  > cf delete-service SERVICE_INSTANCE [-f]
  ```

* Bind/Unbind - These operations create or delete unique credential sets for an existing service instance that can then be injected into the environment of an application instance.

  ```bash
  > cf bind-service APP_NAME SERVICE_INSTANCE [-c PARAMETERS_AS_JSON]
  > cf unbind-service APP_NAME SERVICE_INSTANCE
  ```

## The Services Marketplace

There are two ways to discover what services are available on Cloudfoundry.

The first is to simply use the CLI and just enter:

```bash
> cf marketplace
Getting services from marketplace in org test / space test as admin...
OK

service                       plans                     description
app-autoscaler                standard                  Scales bound applications in response to load
metrics-forwarder             unlimited                 Custom metrics service
p-circuit-breaker-dashboard   standard                  Circuit Breaker Dashboard for Spring Cloud Applications
p-config-server               standard                  Config Server for Spring Cloud Applications
p-mysql                       100mb                     MySQL databases on demand
p-rabbitmq                    standard                  RabbitMQ is a robust and scalable high-performance multi-protocol messaging broker.
p-redis                       dedicated-vm, shared-vm   Redis service to provide pre-provisioned instances configured as a datastore, running on a shared or dedicated VM.
p-service-registry            standard                  Service Registry for Spring Cloud Applications
p.redis                       cache-small               Redis service to provide on-demand dedicated instances configured as a cache.

TIP:  Use 'cf marketplace -s SERVICE' to view descriptions of individual plans of a given service.
```

The second way is to use the Pivotal Apps Manager. If you haven't already done so, login to it by visiting https://apps.run.haas-76.pez.pivotal.io or the appropriate link for your workshop setup. Once logged in, click on the ''Marketplace'' link

---

  ![env-5](../../Common/images/lab-mkt-link.png)

---

You should see something like the following, depending on what services have been installed on your Cloud Foundry.

---

  ![env-5](../../Common/images/lab-marketplace.png)

---

## Creating and Binding Services

1. Let's begin by creating a MySQL service instance. From the Apps Manager UI service marketplace, select _MySQL_ service.

   ---

     ![env-6](../../Common/images/lab-mysql.png)

   ---

1. Next, select the `100mb` plan.

   ---

     ![env-7](../../Common/images/lab-mysql2.png)

   ---

1. Next, provide an instance name for the instance you are about to create. In the drop-down list next to _Bind to App_ select your workshop application, and then click the add button.

   ---

    ![env-7](../../Common/images/lab-mysql3.png)

   ---

1. Notice the admonition to `Use 'cf restage' to ensure your env variable changes take effect`. In order for the service bindings to be seen by the application, it will need to be restaged/restarted. All service bindings for an application are provided to it as configuration data via the `VCAP_SERVICES` environment variable.  We'll have a look at its format shortly.

1. Now let's _restage_ the application, which cycles our application back through the staging process before redeploying the application. A _restage_ is generally recommended because Cloud Foundry buildpack processing also has access to injected environment variables and can install or configure things differently based on their values.

   ```bash
   > cf restage env
   ```

1. You can verify that your service was provisioned by using the Apps Manager and by going to the list of applications in the space and then selecting your application.  Once there, you can then select the _Services_ tab and see what services are bound to the application. You should see something like the following:

   ---

    ![env-7](../../Common/images/lab-mysql4.png)

   ---

1. If you want to observe the actual JSON that is contained in the `VCAP_SERVICES` environment variable, you can do that by selecting the _Settings_ tab, and then click on the `Reveal Env Vars` button on the screen. You should see something like below:

   ```text
   {
    "VCAP_SERVICES": {
      "p-mysql": [
        {
          "credentials": {
            "hostname": "192.168.1.46",
            "port": 3306,
            "name": "cf_4222fb80_c3f7_469d_8446_61e15831697e",
            "username": "HUyrRsitEvYVLfqd",
            "password": "SLMASDwtasDNbsNc",
            "uri": "mysql://HUyrRsitEvYVLfqd:SLMASDwtasDNbsNc@192.168.1.46:3306/cf_4222fb80_c3f7_469d_8446_61e15831697e?reconnect=true",
            "jdbcUrl": "jdbc:mysql://192.168.1.46:3306/cf_4222fb80_c3f7_469d_8446_61e15831697e?user=HUyrRsitEvYVLfqd&password=SLMASDwtasDNbsNc"
          },
          "syslog_drain_url": null,
          "volume_mounts": [],
          "label": "p-mysql",
          "provider": null,
          "plan": "100mb",
          "name": "mysql",
          "tags": [
            "mysql",
            "relational"
          ]
        }
      ]
    }
   ```

1. You may also verify your service was provisioned using the CLI:

   ```bash
   > cf services
   ```

1. Lastly, take a look at your app again. Click on the ``CloudFoundry Configuration`` menu item and you can see how the Steeltoe CloudFoundry configuration provider has parsed the ``VCAP_SERVICES`` and made the details available as configuration data.

   ---

    ![env-7](../../Common/images/lab-app-db.png)

   ---

   Again take some time and see if you can find in the code how this is accomplished. Start with the ``CloudFoundryConfig()`` action in the ``HomeController``.
