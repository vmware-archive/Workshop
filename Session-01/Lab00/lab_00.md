# Lab 0 - Accessing the Workshop Environment

## Cloud Foundry Command Line Interface (CLI) -  Target and Login

1. If you haven't already done so, download the latest release of the [Cloud Foundry CLI](https://github.com/cloudfoundry/cli/releases) for your operating system and install it.

1. From a command prompt, set the Cloud Foundry target for the CLI.

   Note: Use the appropriate HTTP URL for your workshop environment.

   ```bash
   > cf api https://api.run.haas-76.pez.pivotal.io --skip-ssl-validation
   ```

1. Login to Cloud Foundry and follow the prompts, choosing the (student-X) username assigned to you by the instructor. Ask your instructor for the password.

   ```bash
   > cf login
   API endpoint: https://api.run.haas-76.pez.pivotal.io

   Email> student-1

   Password>
   Authenticating...
   OK

   Targeted org student-1

   Targeted space development

   API endpoint:   https://api.run.haas-76.pez.pivotal.io (API version: 2.54.0)
   User:           student-1
   Org:            student-1
   Space:          development

   ```

## Pivotal Cloud Foundry Apps Manager - Login

1. Login to the Pivotal Apps Manager using the URL appropriate for your setup. (e.g. <https://apps.run.haas-76.pez.pivotal.io>).

   Note: Use the same username and password you entered when logging in using the Cloud Foundry CLI.

   ---

   ![login](../../Common/images/lab-student-login.png)

   ---

   Upon success, you should see a screen similar to below:

   ---

   ![logged](../../Common/images/lab-student-loggedin.png)

   ---
