using PortfolioKylian.Components;
using PortfolioKylian.Services;

var builder = WebApplication.CreateBuilder(args);

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Rediriger les 404 vers notre page personnalisée même en développement
app.UseStatusCodePagesWithReExecute("/404");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
