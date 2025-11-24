using ConectorAppWrite;
using ENT;
using BL;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MyDMG_app.ViewModels
{
    public class CrearCortejoViewModel : INotifyPropertyChanged
    {
        private readonly ClsCortejoBl _cortejoBL;

        private string _nombreCortejo;
        private int _nParticipantes;
        private bool _esPaso;
        private int _cantidadPasos;
        private bool _llevaBanda;

        // Propiedades bindables
        public string NombreCortejo
        {
            get => _nombreCortejo;
            set { _nombreCortejo = value; OnPropertyChanged(); }
        }

        public int NParticipantes
        {
            get => _nParticipantes;
            set { _nParticipantes = value; OnPropertyChanged(); }
        }

        public bool EsPaso
        {
            get => _esPaso;
            set { _esPaso = value; OnPropertyChanged(); }
        }

        public int CantidadPasos
        {
            get => _cantidadPasos;
            set { _cantidadPasos = value; OnPropertyChanged(); }
        }

        public bool LlevaBanda
        {
            get => _llevaBanda;
            set { _llevaBanda = value; OnPropertyChanged(); }
        }

        // Comandos
        public ICommand CrearCortejoCommand { get; }
        public ICommand VolverHomeCommand { get; }

        public CrearCortejoViewModel()
        {
            _cortejoBL = new ClsCortejoBl();
            CrearCortejoCommand = new Command(async () => await CrearCortejo());
            VolverHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }

        private async Task CrearCortejo()
        {
            try
            {
                string userId = ConectorAppwrite.Sesion?.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No hay sesión activa", "OK");
                    return;
                }

                var cortejo = new ClsCortejo
                {
                    NombreCortejo = NombreCortejo,
                    NParticipantes = NParticipantes,
                    EsPaso = EsPaso,
                    CantidadPasos = CantidadPasos,
                    LlevaBanda = LlevaBanda,
                    IdUsuario = userId,
                    VelocidadMedia = 0 // Inicialmente 0, se puede calcular después
                };

                bool ok = await _cortejoBL.CrearCortejoAsync(cortejo);
                if (ok)
                {
                    await App.Current.MainPage.DisplayAlert("Éxito", "Cortejo creado correctamente", "OK");
                    // Limpiar campos
                    NombreCortejo = string.Empty;
                    NParticipantes = 0;
                    EsPaso = false;
                    CantidadPasos = 0;
                    LlevaBanda = false;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo crear el cortejo", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


