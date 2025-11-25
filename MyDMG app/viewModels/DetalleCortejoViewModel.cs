using BL;
using ENT;
using MyDMG_app.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class DetalleCortejoViewModel : INotifyPropertyChanged
    {
        private readonly ClsCortejoBl _bl = new();
        private bool _modoEdicion = false;

        public ClsCortejo Cortejo { get; set; } = new();

        public bool ModoEdicion
        {
            get => _modoEdicion;
            set
            {
                _modoEdicion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoModoEdicion));
            }
        }

        public bool NoModoEdicion => !ModoEdicion;

        public ICommand EditarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand VolverCommand { get; }

        public DetalleCortejoViewModel()
        {
            EditarCommand = new Command(() => ModoEdicion = true);
            GuardarCommand = new Command(async () => await GuardarCambios());
            EliminarCommand = new Command(async () => await Eliminar());
            VolverCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));

            // Suscribirse al evento del servicio de navegación
            CortejoNavigationService.CortejoIdChanged += async (s, id) =>
            {
                if (!string.IsNullOrEmpty(id))
                    await CargarCortejo(id);
            };
        }

        public async Task CargarCortejo(string id)
        {
            Cortejo = await _bl.GetCortejoPorIdAsync(id);
            OnPropertyChanged(nameof(Cortejo));
        }

        private async Task GuardarCambios()
        {
            bool ok = await _bl.EditarCortejoAsync(Cortejo);

            if (!ok)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo editar", "OK");
                return;
            }

            ModoEdicion = false;
            await App.Current.MainPage.DisplayAlert("Correcto", "Cortejo actualizado", "OK");
        }

        private async Task Eliminar()
        {
            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Seguro que quieres eliminar este cortejo?",
                "Sí", "No");

            if (!confirmar) return;

            bool ok = await _bl.EliminarCortejoAsync(Cortejo.Id);

            if (!ok)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
                return;
            }

            await Shell.Current.GoToAsync("//HomePage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}










