using ConectorAppWrite;
using ENT;
using BL;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class CrearCortejoViewModel : INotifyPropertyChanged
    {
        private readonly ClsCortejoBl _cortejoBL;

        public string NombreCortejo { get; set; }
        public int NParticipantes { get; set; }
        public bool EsPaso { get; set; }
        public int CantidadPasos { get; set; }
        public bool LlevaBanda { get; set; }

        public ICommand CrearCortejoCommand { get; }

        public CrearCortejoViewModel()
        {
            _cortejoBL = new ClsCortejoBl();
            CrearCortejoCommand = new Command(async () => await CrearCortejo());
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
                    OnPropertyChanged(nameof(NombreCortejo));
                    OnPropertyChanged(nameof(NParticipantes));
                    OnPropertyChanged(nameof(EsPaso));
                    OnPropertyChanged(nameof(CantidadPasos));
                    OnPropertyChanged(nameof(LlevaBanda));
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo crear el cortejo", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
        }

        private double CalcularVelocidadMedia(int participantes, int pasos)
        {
            if (pasos == 0) return 0;
            return participantes / (double)pasos;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

