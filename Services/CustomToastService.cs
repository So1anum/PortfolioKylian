using PortfolioKylian.Components.Shared;

namespace PortfolioKylian.Services;

public interface ICustomToastService
{
    event Action<string, ToastType>? OnShow;
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
}

public class CustomToastService : ICustomToastService
{
    public event Action<string, ToastType>? OnShow;

    public void ShowSuccess(string message)
    {
        OnShow?.Invoke(message, ToastType.Success);
    }

    public void ShowError(string message)
    {
        OnShow?.Invoke(message, ToastType.Error);
    }

    public void ShowInfo(string message)
    {
        OnShow?.Invoke(message, ToastType.Info);
    }
}
