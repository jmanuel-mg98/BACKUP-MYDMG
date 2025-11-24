using BL;
using ENT;
using System.ComponentModel;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    [QueryProperty(nameof(Id), "Id")]
    public class DetalleCortejoViewModel : INotifyPropertyChanged
    {
        private readonly ClsCortejoBl _bl;

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
                _ = CargarCortejo();
            }
        }

        public ClsCortejo Cortejo { get; set; }
        public bool Editando { get; set; }

        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand VolverCommand { get; }

        public DetalleCortejoViewModel()
        {
            _bl = new ClsCortejoBl();
            EditarCommand = new Command(async () => await Editar());
            EliminarCommand = new Command(async () => await Eliminar());
            VolverCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }

        private async Task CargarCortejo()
        {
            if (string.IsNullOrEmpty(Id))
                return;

            Cortejo = await _bl.ObtenerCortejoPorIdAsync(Id);
            OnPropertyChanged(nameof(Cortejo));
        }

        private async Task Editar()
        {
            if (!Editando)
            {
                Editando = true;
                OnPropertyChanged(nameof(Editando));
                return;
            }

            bool ok = await _bl.ActualizarCortejoAsync(Cortejo);

            if (ok)
            {
                await App.Current.MainPage.DisplayAlert("OK", "Cortejo actualizado", "OK");
                Editando = false;
                OnPropertyChanged(nameof(Editando));
            }
        }

        private async Task Eliminar()
        {
            bool confirmar = await App.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Seguro que quieres eliminar este cortejo?",
                "Sí", "No");

            if (!confirmar) return;

            bool ok = await _bl.EliminarCortejoAsync(Id);

            if (!ok)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
                return;
            }

            await Shell.Current.GoToAsync("//HomePage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}




