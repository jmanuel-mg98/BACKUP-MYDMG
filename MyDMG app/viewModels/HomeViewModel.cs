using ConectorAppWrite;
using ENT;
using BL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ClsUsuarioBl _usuarioBL;
        private readonly ClsCortejoBl _cortejoBL;
        private readonly ClsRecorridoBl _recorridoBL;

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
        public ObservableCollection<ClsRecorrido> Recorridos { get; set; } = new();

        public ICommand LogoutCommand { get; }
        public ICommand CrearCortejoCommand { get; }
        public ICommand SeleccionarCortejoCommand { get; }
        public ICommand CrearRecorridoCommand { get; }
        public ICommand SeleccionarRecorridoCommand { get; }

        public HomeViewModel()
        {
            _usuarioBL = new ClsUsuarioBl();
            _cortejoBL = new ClsCortejoBl();
            _recorridoBL = new ClsRecorridoBl();

            LogoutCommand = new Command(async () => await Logout());
            CrearCortejoCommand = new Command(async () => await Shell.Current.GoToAsync("//CrearCortejoPage"));
            CrearRecorridoCommand = new Command(async () => await Shell.Current.GoToAsync("//CrearRecorridoPage"));

            SeleccionarCortejoCommand = new Command<ClsCortejo>(c =>
            {
                if (c == null) return;
                Shell.Current.GoToAsync($"//DetalleCortejoPage?id={c.Id}");
            });

            // 🔹 ACTUALIZADO: Ahora navega a DetalleRecorridoPage
            SeleccionarRecorridoCommand = new Command<ClsRecorrido>(r =>
            {
                if (r == null) return;
                Shell.Current.GoToAsync($"//DetalleRecorridoPage?id={r.Id}");
            });

            // Cargar datos al iniciar
            Task.Run(async () => await CargarDatosUsuarioAsync());
        }

        /// <summary>
        /// Carga los datos del usuario autenticado desde Auth y la base de datos.
        /// </summary>
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
                    Recorridos.Clear();
                    return;
                }

                var usuarioCompleto = await _usuarioBL.ObtenerUsuarioCompletoAsync(sesion.UserId);
                if (usuarioCompleto != null)
                {
                    NombreUsuario = usuarioCompleto.Nombre;
                    Hermandad = usuarioCompleto.Hermandad ?? "Hermandad desconocida";
                }
                else
                {
                    NombreUsuario = "Usuario desconocido";
                    Hermandad = "Hermandad desconocida";
                }

                await CargarCortejosAsync();
                await CargarRecorridosAsync();
            }
            catch (Exception ex)
            {
                NombreUsuario = "Error al cargar usuario";
                Hermandad = ex.Message;
            }
        }

        /// <summary>
        /// Carga la lista de cortejos del usuario actual.
        /// </summary>
        public async Task CargarCortejosAsync()
        {
            Cortejos.Clear();

            var sesion = await ConectorAppwrite.GetSesionActual();
            if (sesion == null) return;

            var lista = await _cortejoBL.GetCortejosUsuarioAsync(sesion.UserId)
                        ?? new List<ClsCortejo>();

            foreach (var c in lista)
            {
                Cortejos.Add(c);
            }
        }

        /// <summary>
        /// Carga la lista de recorridos del usuario actual y asigna el nombre del cortejo.
        /// </summary>
        public async Task CargarRecorridosAsync()
        {
            Recorridos.Clear();

            var sesion = await ConectorAppwrite.GetSesionActual();
            if (sesion == null) return;

            var listaRecorridos = await _recorridoBL.GetRecorridosUsuarioAsync(sesion.UserId)
                                  ?? new List<ClsRecorrido>();

            // Obtener nombres de cortejos para mostrar
            foreach (var recorrido in listaRecorridos)
            {
                // Buscar el nombre del cortejo asociado
                var cortejo = Cortejos.FirstOrDefault(c => c.Id == recorrido.IdCortejo);
                if (cortejo != null)
                {
                    recorrido.NombreCortejo = cortejo.NombreCortejo;
                }
                else
                {
                    recorrido.NombreCortejo = "Cortejo no encontrado";
                }

                Recorridos.Add(recorrido);
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario y limpia todos los datos.
        /// </summary>
        private async Task Logout()
        {
            await ConectorAppwrite.cerrarSesion();
            NombreUsuario = string.Empty;
            Hermandad = string.Empty;
            Cortejos.Clear();
            Recorridos.Clear();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}















