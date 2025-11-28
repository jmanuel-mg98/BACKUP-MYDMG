using BL;
using ENT;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MyDMG_app.ViewModels
{
    [QueryProperty(nameof(CortejoId), "id")]
    public class DetalleCortejoViewModel : INotifyPropertyChanged
    {
        private readonly ClsCortejoBl _bl = new();
        private bool _modoEdicion = false;
        private string _cortejoId;

        public string CortejoId
        {
            get => _cortejoId;
            set
            {
                _cortejoId = value;
                OnPropertyChanged();
                // Cargar datos automáticamente cuando se setea el id
                if (!string.IsNullOrEmpty(_cortejoId))
                    Task.Run(async () => await CargarCortejo(_cortejoId));
            }
        }

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
        }

        public async Task CargarCortejo(string id)
        {
            Cortejo = await _bl.GetCortejoPorIdAsync(id);
            OnPropertyChanged(nameof(Cortejo));
        }
        /// <summary>
        /// funcion que guarda los cambios realizados en el cortejo llamando a la BL y mostrando alertas en caso de exito o error
        /// </summary>
        /// <returns></returns>
        private async Task GuardarCambios()
        {
            Cortejo.VelocidadMedia = Helpers.CortejoHelper.CalcularVelocidadMedia(Cortejo);

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
        /// <summary>
        /// funcion que notifica el cambio de una propiedad
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}












