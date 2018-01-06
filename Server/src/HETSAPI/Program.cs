using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HETSAPI
{
    /// <summary>
    /// The main Program for the application.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
