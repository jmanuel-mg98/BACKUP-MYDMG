using MyDMG_app.ViewModels;

namespace MyDMG_app.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _vm;

        public HomePage()
        {
            InitializeComponent();
            _vm = new HomeViewModel();
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Recargar SIEMPRE datos al entrar, para evitar que aparezcan datos del usuario anterior
            await _vm.CargarDatosUsuarioAsync();

            // Recargar también la lista de cortejos
            await _vm.CargarCortejosAsync();
        }
    }
}

