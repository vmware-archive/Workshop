using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CloudFoundry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) 
        {
            // Lab01 - Lab04 Start
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            // Lab01 - Lab04 End

            return WebHost.CreateDefaultBuilder(args)

                // Lab01 - Lab04 Start
                .UseConfiguration(configuration)
                // Lab01 - Lab04 End

                .UseStartup<Startup>()
                // Lab01 - Lab04 Start
                .AddCloudFoundry()
                // Lab01 - Lab04 End
                .Build();
        }
    }
}
