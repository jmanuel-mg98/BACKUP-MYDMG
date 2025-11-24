using BL;
using ConectorAppWrite;
using DAL;
using ENT;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ClsUsuarioBl _usuarioBL;
        private readonly ClsCortejoBl _cortejoBL;

        private string _nombreUsuario;
        private string _hermandad;
        private ObservableCollection<ClsCortejo> _cortejos;

        public string NombreUsuario
        {
            get => _nombreUsuario;
            set { _nombreUsuario = value; OnPropertyChanged(); }
        }

        public string Hermandad
        {
            get => _hermandad;
            set { _hermandad = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ClsCortejo> Cortejos
        {
            get => _cortejos;
            set { _cortejos = value; OnPropertyChanged(); }
        }

        public ICommand LogoutCommand { get; }
        public ICommand CrearCortejoCommand { get; }

        public HomeViewModel()
        {
            _usuarioBL = new ClsUsuarioBl();
            _cortejoBL = new ClsCortejoBl();

            Cortejos = new ObservableCollection<ClsCortejo>();

            LogoutCommand = new Command(async () => await Logout());
            CrearCortejoCommand = new Command(async () => await Shell.Current.GoToAsync("//CrearCortejoPage"));

            Task.Run(async () =>
            {
                await CargarDatosUsuarioAsync();
                await CargarCortejosAsync();
            });
        }

        public async Task CargarDatosUsuarioAsync()
        {
            try
            {
                var sesion = await ConectorAppwrite.GetSesionActual();
                if (sesion == null)
                {
                    NombreUsuario = "Usuario desconocido";
                    Hermandad = "Hermandad desconocida";
                    return;
                }

                var usuarioCompleto = await _usuarioBL.ObtenerUsuarioCompletoAsync(sesion.UserId);
                NombreUsuario = usuarioCompleto?.Nombre ?? "Sin nombre";
                Hermandad = usuarioCompleto?.Hermandad ?? "Sin hermandad";
            }
            catch (Exception ex)
            {
                NombreUsuario = "Error al cargar usuario";
                Hermandad = ex.Message;
            }
        }

        public async Task CargarCortejosAsync()
        {
            try
            {
                var sesion = await ConectorAppwrite.GetSesionActual();
                if (sesion == null) return;

                var lista = await _cortejoBL.ObtenerCortejosPorUsuarioAsync(sesion.UserId);
                Cortejos.Clear();
                foreach (var c in lista)
                    Cortejos.Add(c);
            }
            catch (Exception ex)
            {
                // Puedes manejar error o mostrar alert
            }
        }

        private async Task Logout()
        {
            ConectorAppwrite.cerrarSesion();
            Cortejos.Clear();
            NombreUsuario = string.Empty;
            Hermandad = string.Empty;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}





