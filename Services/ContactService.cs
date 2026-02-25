using System.Text;
using System.Text.Json;

namespace PortfolioKylian.Services;

public interface IContactService
{
    Task<bool> SendContactMessageAsync(string name, string email, string subject, string message);
}

public class FormspreeContactService : IContactService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FormspreeContactService> _logger;

    public FormspreeContactService(
        HttpClient httpClient, 
        IConfiguration configuration,
        ILogger<FormspreeContactService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendContactMessageAsync(string name, string email, string subject, string message)
    {
        try
        {
            var formspreeId = _configuration["Formspree:FormId"];
            
            if (string.IsNullOrEmpty(formspreeId))
            {
                _logger.LogError("Formspree FormId n'est pas configuré");
                return false;
            }

            var endpoint = $"https://formspree.io/f/{formspreeId}";

            var payload = new
            {
                name = name,
                email = email,
                subject = subject,
                message = message,
                _replyto = email
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Message de contact envoyé avec succès de {Name} <{Email}>", name, email);
                return true;
            }
            else
            {
                _logger.LogWarning("Échec de l'envoi du message. Status: {StatusCode}", response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'envoi du message de contact");
            return false;
        }
    }
}
