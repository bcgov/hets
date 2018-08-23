using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Pdf.Server
{
    /// <summary>
    /// The main Program for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build Web Host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
