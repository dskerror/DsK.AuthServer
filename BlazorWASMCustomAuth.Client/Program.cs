using Blazored.LocalStorage;
using BlazorWASMCustomAuth.Client;
using BlazorWASMCustomAuth.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7045/") });
builder.Services.AddScoped<SecurityServiceClient>();

/* ---Authentication--- */
//Add Authorization Core - To be able to use [CascadingAuthenticationState, AuthorizeRouteView, Authorizing], [AuthorizeView, NotAuthorized, Authorized], @attribute [Authorize]
builder.Services.AddAuthorizationCore();
//The CustomAuthenticationStateProvider is to be able to use tokens as the mode of authentication.
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

/* ---Manages saving to local storage--- */
builder.Services.AddBlazoredLocalStorage();

/* ---Manages MudBlazor config--- */
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

await builder.Build().RunAsync();
