using ConectorAppWrite;
using ENT;
using BL;
using MyDMG_app.Services;
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

        public ObservableCollection<ClsCortejo> Cortejos { get; set; } = new();

        public ICommand LogoutCommand { get; }
        public ICommand CrearCortejoCommand { get; }
        public ICommand SeleccionarCortejoCommand { get; }

        public HomeViewModel()
        {
            _usuarioBL = new ClsUsuarioBl();
            _cortejoBL = new ClsCortejoBl();

            LogoutCommand = new Command(async () => await Logout());
            CrearCortejoCommand = new Command(async () => await Shell.Current.GoToAsync("//CrearCortejoPage"));
            SeleccionarCortejoCommand = new Command<ClsCortejo>(c =>
            {
                if (c == null) return;
                CortejoNavigationService.NavigateToCortejo(c.Id);
            });

            // Cargar datos al iniciar
            Task.Run(async () => await CargarDatosUsuarioAsync());
        }

        // ----------------------
        // Cargar datos del usuario
        // ----------------------
        public async Task CargarDatosUsuarioAsync()
        {
            try
            {
                var sesion = await ConectorAppwrite.GetSesionActual();
                if (sesion == null)
                {
                    NombreUsuario = "Usuario desconocido";
                    Hermandad = "Hermandad desconocida";
                    Cortejos.Clear();
                    return;
                }

                NombreUsuario = sesion.UserId ?? "Usuario desconocido";

                var detalle = await _usuarioBL.ObtenerDetalleUsuarioActualAsync(sesion.UserId);
                Hermandad = detalle?.Hermandad ?? "Hermandad desconocida";

                await CargarCortejosAsync();
            }
            catch (Exception ex)
            {
                NombreUsuario = "Error al cargar usuario";
                Hermandad = ex.Message;
            }
        }

        // ----------------------
        // Cargar lista de cortejos
        // ----------------------
        public async Task CargarCortejosAsync()
        {
            Cortejos.Clear();

            var sesion = await ConectorAppwrite.GetSesionActual();
            if (sesion == null) return;

            var lista = await _cortejoBL.GetCortejosUsuarioAsync(sesion.UserId)
                        ?? new System.Collections.Generic.List<ClsCortejo>();

            foreach (var c in lista)
            {
                Cortejos.Add(c);
            }
        }

        // ----------------------
        // Abrir detalle cortejo
        // ----------------------
        private async Task AbrirDetalleCortejo(ClsCortejo c)
        {
            if (c == null) return;
            CortejoNavigationService.NavigateToCortejo(c.Id);
        }

        // ----------------------
        // Cerrar sesión
        // ----------------------
        private async Task Logout()
        {
            await ConectorAppwrite.cerrarSesion();
            NombreUsuario = string.Empty;
            Hermandad = string.Empty;
            Cortejos.Clear();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}














