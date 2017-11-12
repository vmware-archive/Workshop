using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CloudFoundry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                // Lab01 - Lab04 Start
                .AddCloudFoundry()
                // Lab01 - Lab04 End
                .Build();
    }
}
