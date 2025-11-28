using ConectorAppWrite;
using ENT;
using BL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class CrearRecorridoViewModel : INotifyPropertyChanged
    {
        private readonly ClsRecorridoBl _recorridoBL;
        private readonly ClsCortejoBl _cortejoBL;

        private string _nombre;
        private string _lugarPartida;
        private TimeSpan _duracion = TimeSpan.FromHours(1); // Duración por defecto 1 hora
        private ClsCortejo _cortejoSeleccionado;

        // Propiedades bindables
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public string LugarPartida
        {
            get => _lugarPartida;
            set { _lugarPartida = value; OnPropertyChanged(); }
        }

        public TimeSpan Duracion
        {
            get => _duracion;
            set { _duracion = value; OnPropertyChanged(); }
        }

        public ClsCortejo CortejoSeleccionado
        {
            get => _cortejoSeleccionado;
            set { _cortejoSeleccionado = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ClsCortejo> Cortejos { get; set; } = new();

        // Comandos
        public ICommand CrearRecorridoCommand { get; }
        public ICommand VolverHomeCommand { get; }

        public CrearRecorridoViewModel()
        {
            _recorridoBL = new ClsRecorridoBl();
            _cortejoBL = new ClsCortejoBl();

            CrearRecorridoCommand = new Command(async () => await CrearRecorrido());
            VolverHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
        }
        /// <summary>
        /// funcion que inicializa el viewmodel cargando los cortejos
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await CargarCortejos();
        }
        /// <summary>
        /// funcion que carga los cortejos del usuario que tiene la sesion iniciada llamando a la BL y llenando la coleccion observable
        /// </summary>
        /// <returns></returns>
        private async Task CargarCortejos()
        {
            try
            {
                string userId = ConectorAppwrite.Sesion?.UserId;
                if (string.IsNullOrEmpty(userId))
                    return;

                var lista = await _cortejoBL.GetCortejosUsuarioAsync(userId);

                Cortejos.Clear();
                foreach (var cortejo in lista)
                {
                    Cortejos.Add(cortejo);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"No se pudieron cargar los cortejos: {ex.Message}", "OK");
            }
        }
        /// <summary>
        /// funcion que crea un recorrido llamando a la BL y mostrando alertas en caso de exito o error asignandole el id del usuario que tiene la sesion iniciada
        /// </summary>
        /// <returns></returns>
        private async Task CrearRecorrido()
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "El nombre es obligatorio", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(LugarPartida))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "El lugar de partida es obligatorio", "OK");
                    return;
                }

                if (CortejoSeleccionado == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un cortejo", "OK");
                    return;
                }

                string userId = ConectorAppwrite.Sesion?.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No hay sesión activa", "OK");
                    return;
                }

                // Convertir duración a minutos
                int duracionMinutos = (int)Duracion.TotalMinutes;

                var recorrido = new ClsRecorrido
                {
                    IdUsuario = userId,
                    IdCortejo = CortejoSeleccionado.Id,
                    Nombre = Nombre,
                    LugarPartida = LugarPartida,
                    Horario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Puedes personalizar esto
                    DuracionRecorrido = duracionMinutos,
                    Itinerario = new List<string>() // Por ahora vacío, puedes añadir funcionalidad después
                };

                bool ok = await _recorridoBL.CrearRecorridoAsync(recorrido);

                if (ok)
                {
                    await App.Current.MainPage.DisplayAlert("Éxito", "Recorrido creado correctamente", "OK");

                    // Limpiar campos
                    Nombre = string.Empty;
                    LugarPartida = string.Empty;
                    Duracion = TimeSpan.FromHours(1);
                    CortejoSeleccionado = null;

                    // Volver a HomePage
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo crear el recorrido", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error inesperado", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// funcion que avisa de que una propiedad ha cambiado
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
