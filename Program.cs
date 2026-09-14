using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Reconciliation.Blazor;
using Reconciliation.Blazor.Core.Endpoints;
using Reconciliation.Blazor.Services;
using static Reconciliation.Blazor.UserAdminService;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                  ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

ApiEndpoints.Initialize(apiBaseUrl);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Add Local Storage Service
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

// Add Authentication Services
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(ApiEndpoints.Base);
})
.AddHttpMessageHandler<ApiAuthHandler>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<JwtAuthStateProvider>());
    
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();                                                                                                
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserAdminService>();
builder.Services.AddScoped<IBatchesService, BatchServices>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IGlobalSettings, GlobalSettingService>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IExceptionService, ExceptionService>();
builder.Services.AddScoped<IManualAdjustment, ManualAdjustmentService>();
builder.Services.AddScoped<IPendingReviewsServices, PendingReviews>();
builder.Services.AddScoped<IExceptions, ExceptionsServices>();
builder.Services.AddScoped<MenuNavigationService>();

builder.Services.AddScoped<AppState>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<TokenServices>();

builder.Services.AddScoped<ApiAuthHandler>();


builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5186") // Your API URL
});
await builder.Build().RunAsync();
