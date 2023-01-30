using Blazored.LocalStorage;
using BlazorWASMCustomAuth.Client;
using BlazorWASMCustomAuth.Client.Security;
using BlazorWASMCustomAuth.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7045/") });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<SecurityServiceClient>();
//builder.Services.AddScoped<ITokenManagerService, TokenManagerService>();
//builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddMudServices();



await builder.Build().RunAsync();
