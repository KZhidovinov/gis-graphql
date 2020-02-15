namespace GisApi.ApiServer
{
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(configurationBuilder => configurationBuilder
                .AddEnvironmentVariables(prefix: "DOTNET_"))
                .ConfigureAppConfiguration((hostingContext, config) => config
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true, reloadOnChange: false)
                    .If(
                        hostingContext.HostingEnvironment.IsDevelopment(),
                        builder => builder.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true))
                    .AddEnvironmentVariables())
                .UseDefaultServiceProvider((context, options) =>
                {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = isDevelopment;
                    options.ValidateOnBuild = isDevelopment;
                })
                .ConfigureWebHost(webHostBuilder => webHostBuilder
                    .UseKestrel(kestrelOptions =>
                    {
                        kestrelOptions.AddServerHeader = false;
                        kestrelOptions.AllowSynchronousIO = true;
                    })
                .UseIIS()
                .ConfigureServices(services => services
                    .Configure<IISServerOptions>(config => config.AllowSynchronousIO = true))
                .UseStartup<Startup>());
    }
}
