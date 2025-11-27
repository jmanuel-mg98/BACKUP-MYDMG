using MyDMG_app.ViewModels;

namespace MyDMG_app.Views;

public partial class CrearRecorridoPage : ContentPage
{
    private CrearRecorridoViewModel vm;

    public CrearRecorridoPage()
    {
        InitializeComponent();
        vm = new CrearRecorridoViewModel();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Inicializamos datos de forma segura después de que la página aparece
        await vm.InitializeAsync();
    }
}

