using Toolbelt.Blazor.Extensions.DependencyInjection;
using WarHub.GodMode.Components.Areas.Workspace;
using WarHub.GodMode.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHeadElementHelper();
builder.Services.AddSingleton<LocalFsWorkspaceProvider>();
builder.Services.AddSingleton<GitHubWorkspaceProvider>();
builder.Services.AddScoped<IWorkspaceProviderAggregate, WorkspaceProviderAggregate>();
builder.Services.AddScoped<IWorkspaceContextResolver, WorkspaceContextResolver>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
