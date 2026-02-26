using PortfolioKylian.Components;
using PortfolioKylian.Services;

var builder = WebApplication.CreateBuilder(args);

// Ajouter Application Insights avec JavaScript tracking
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    options.EnableAdaptiveSampling = true; // Optimisation des coûts
    options.EnableQuickPulseMetricStream = true; // Monitoring en temps réel
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Ajout du service de culture
builder.Services.AddScoped<ICultureService, CultureService>();

// Ajout du helper de localisation
builder.Services.AddScoped<PortfolioKylian.Resources.LocalizationHelper>();

// Ajout du service de toast personnalisé
builder.Services.AddSingleton<ICustomToastService, CustomToastService>();

// Ajout du HttpClient et service de contact
builder.Services.AddHttpClient();
builder.Services.AddScoped<IContactService, FormspreeContactService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/404");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
