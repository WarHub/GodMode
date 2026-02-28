using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using WarHub.GodMode.Components;
using WarHub.GodMode.Components.Areas.Workspace;
using WarHub.GodMode.GithubPages.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHeadElementHelper();
builder.Services.AddSingleton<GitHubWorkspaceProvider>();
builder.Services.AddScoped<IWorkspaceProviderAggregate, WorkspaceProviderAggregate>();
builder.Services.AddScoped<IWorkspaceContextResolver, WorkspaceContextResolver>();

await builder.Build().RunAsync();
