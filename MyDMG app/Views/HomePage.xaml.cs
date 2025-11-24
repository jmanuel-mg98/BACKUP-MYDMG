using MyDMG_app.ViewModels;

namespace MyDMG_app.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomeViewModel vm)
        {
            await vm.CargarDatosUsuarioAsync();
            await vm.CargarCortejosAsync();
        }
    }
}


