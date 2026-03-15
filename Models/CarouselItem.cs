namespace PortfolioKylian.Models;

public class CarouselItem
{
    public string Url { get; set; } = string.Empty;
    public string AltKey { get; set; } = string.Empty;
    public string CaptionKey { get; set; } = string.Empty;

    public bool IsVideo => !string.IsNullOrEmpty(Url) && 
        (Url.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
         Url.EndsWith(".webm", StringComparison.OrdinalIgnoreCase) ||
         Url.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) ||
         Url.EndsWith(".mov", StringComparison.OrdinalIgnoreCase));
}
