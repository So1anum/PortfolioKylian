using System.Globalization;
using Microsoft.JSInterop;

namespace PortfolioKylian.Services;

public interface ICultureService
{
    event Action? OnCultureChanged;
    CultureInfo CurrentCulture { get; }
    Task InitializeAsync();
    Task SetCultureAsync(string culture);
    string GetCurrentCultureCode();
}

public class CultureService : ICultureService
{
    private readonly IJSRuntime _jsRuntime;
    private CultureInfo _currentCulture = new("fr-FR");

    public event Action? OnCultureChanged;
    public CultureInfo CurrentCulture => _currentCulture;

    public CultureService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Essaie de récupérer la langue sauvegardée dans localStorage
            var savedCulture = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "preferredCulture");

            if (!string.IsNullOrEmpty(savedCulture))
            {
                // Utilise la langue sauvegardée
                await SetCultureAsync(savedCulture);
            }
            else
            {
                // Tente de récupérer la langue du navigateur
                var browserCulture = await _jsRuntime.InvokeAsync<string>("eval", "navigator.language || navigator.userLanguage");

                // Si la langue contient "fr", on garde le français, sinon anglais
                var culture = browserCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase) ? "fr-FR" : "en-US";

                await SetCultureAsync(culture);
            }
        }
        catch
        {
            // Par défaut : français
            await SetCultureAsync("fr-FR");
        }
    }

    public async Task SetCultureAsync(string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        _currentCulture = cultureInfo;

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        // Sauvegarder la préférence dans localStorage
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "preferredCulture", culture);
        }
        catch
        {
            // Ignorer les erreurs de localStorage (peut ne pas être disponible lors du prerendering)
        }

        // Forcer la mise à jour de tous les composants
        await Task.Delay(10); // Petit délai pour s'assurer que tout est prêt
        OnCultureChanged?.Invoke();
    }

    public string GetCurrentCultureCode()
    {
        return _currentCulture.TwoLetterISOLanguageName;
    }
}
