using MyDMG_app.ViewModels;

namespace MyDMG_app.Views;

public partial class HomePage : ContentPage
{
    private HomeViewModel _vm;

    public HomePage()
    {
        InitializeComponent();
        _vm = new HomeViewModel();
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Recargar datos de usuario y cortejos al aparecer
        await _vm.CargarDatosUsuarioAsync();
        await _vm.CargarCortejosAsync();
    }
}



