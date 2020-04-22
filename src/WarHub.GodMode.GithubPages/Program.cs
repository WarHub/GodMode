using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using WarHub.GodMode.Components;
using WarHub.GodMode.Components.Areas.Workspace;
using WarHub.GodMode.GithubPages.Services;

namespace WarHub.GodMode.GithubPages
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHeadElementHelper();
            services.AddSingleton<GitHubWorkspaceProvider>();
            services.AddScoped<IWorkspaceProviderAggregate, WorkspaceProviderAggregate>();
            services.AddScoped<IWorkspaceContextResolver, WorkspaceContextResolver>();
        }
    }
}
