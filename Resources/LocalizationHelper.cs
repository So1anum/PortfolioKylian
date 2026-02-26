using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace PortfolioKylian.Resources;

public class LocalizationHelper
{
    private readonly Services.ICultureService _cultureService;

    public event Action? OnLanguageChanged;

    public LocalizationHelper(Services.ICultureService cultureService)
    {
        _cultureService = cultureService;
        _cultureService.OnCultureChanged += () => OnLanguageChanged?.Invoke();
    }

    public string this[string key]
    {
        get
        {
            var currentCulture = _cultureService.CurrentCulture.Name;

            if (currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase))
            {
                return Translations.French.TryGetValue(key, out var value) ? value : key;
            }
            else
            {
                return Translations.English.TryGetValue(key, out var value) ? value : key;
            }
        }
    }

    /// <summary>
    /// Retourne un MarkupString pour afficher du HTML dans les traductions
    /// </summary>
    public MarkupString Html(string key)
    {
        return new MarkupString(this[key]);
    }
}
