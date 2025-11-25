namespace MyDMG_app.Services;

public static class CortejoNavigationService
{
    public static event EventHandler<string> CortejoIdChanged;

    public static void NavigateToCortejo(string id)
    {
        CortejoIdChanged?.Invoke(null, id);
        Shell.Current.GoToAsync("//DetalleCortejoPage");
    }
}


