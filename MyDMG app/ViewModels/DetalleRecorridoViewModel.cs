using BL;
using ENT;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    [QueryProperty(nameof(RecorridoId), "id")]
    public class DetalleRecorridoViewModel : INotifyPropertyChanged
    {
        private readonly ClsRecorridoBl _bl = new();
        private readonly ClsCortejoBl _cortejoBl = new();
        private string _recorridoId;

        public string RecorridoId
        {
            get => _recorridoId;
            set
            {
                _recorridoId = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(_recorridoId))
                    Task.Run(async () => await CargarRecorrido(_recorridoId));
            }
        }

        public ClsRecorrido Recorrido { get; set; } = new();

        public ICommand EliminarCommand { get; }
        public ICommand VolverCommand { get; }

        public DetalleRecorridoViewModel()
        {
            EliminarCommand = new Command(async () => await Eliminar());
            VolverCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }

        /// <summary>
        /// Carga los datos del recorrido por su ID y obtiene el nombre del cortejo asociado.
        /// </summary>
        /// <param name="id">ID del recorrido</param>
        public async Task CargarRecorrido(string id)
        {
            Recorrido = await _bl.GetRecorridoPorIdAsync(id);

            // Cargar nombre del cortejo
            if (!string.IsNullOrEmpty(Recorrido.IdCortejo))
            {
                try
                {
                    var cortejo = await _cortejoBl.GetCortejoPorIdAsync(Recorrido.IdCortejo);
                    if (cortejo != null)
                    {
                        Recorrido.NombreCortejo = cortejo.NombreCortejo;
                    }
                }
                catch
                {
                    Recorrido.NombreCortejo = "Cortejo no encontrado";
                }
            }

            OnPropertyChanged(nameof(Recorrido));
        }

        /// <summary>
        /// Elimina el recorrido actual después de confirmar con el usuario.
        /// </summary>
        private async Task Eliminar()
        {
            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Seguro que quieres eliminar este recorrido?",
                "Sí", "No");

            if (!confirmar) return;

            bool ok = await _bl.EliminarRecorridoAsync(Recorrido.Id);

            if (!ok)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Éxito", "Recorrido eliminado correctamente", "OK");
            await Shell.Current.GoToAsync("//HomePage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
