using ConectorAppWrite;
using ENT;
using BL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyDMG_app.Services;
using BL.Helpers;

namespace MyDMG_app.ViewModels
{
    public class CrearRecorridoViewModel : INotifyPropertyChanged
    {
        private readonly ClsRecorridoBl _recorridoBL;
        private readonly ClsCortejoBl _cortejoBL;
        private readonly GeocodingService _geoService = new GeocodingService();


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
                // --- VALIDACIONES ---
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

                // --- DURACIÓN EN MINUTOS ---
                int duracionMinutos = (int)Duracion.TotalMinutes;

                // --- OBTENER COORDENADAS DEL ORIGEN ---
                var coordenadas = await _geoService.GetCoordinatesAsync(LugarPartida);

                if (coordenadas == null)
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudieron obtener las coordenadas del lugar de partida.",
                        "OK");
                    return;
                }

                double latitud = coordenadas.Value.lat;
                double longitud = coordenadas.Value.lng;

               
                // CALCULAR METROS DEL RECORRIDO
              
                // CortejoSeleccionado ya trae su VelocidadMedia desde la BD
                double velocidadMedia = CortejoSeleccionado.VelocidadMedia;

                // metros = velocidad (m/min) * duración (min)
                double metrosRecorrido = velocidadMedia * duracionMinutos;

                Console.WriteLine($"Metros necesarios para el recorrido: {metrosRecorrido}");

                // calcular punto intermedio para los 4 posibles recorridos

                double distanciaMitad = metrosRecorrido / 2;

                // 0 = norte, 180 = sur, 90 = este, 270 = oeste
                var puntoNorte = GeoHelper.CalcularDestino(latitud, longitud, distanciaMitad, 0);
                var puntoSur = GeoHelper.CalcularDestino(latitud, longitud, distanciaMitad, 180);
                var puntoEste = GeoHelper.CalcularDestino(latitud, longitud, distanciaMitad, 90);
                var puntoOeste = GeoHelper.CalcularDestino(latitud, longitud, distanciaMitad, 270);

                // Puedes verificarlos:
                Console.WriteLine($"Norte: {puntoNorte.lat}, {puntoNorte.lng}");
                Console.WriteLine($"Sur: {puntoSur.lat}, {puntoSur.lng}");
                Console.WriteLine($"Este: {puntoEste.lat}, {puntoEste.lng}");
                Console.WriteLine($"Oeste: {puntoOeste.lat}, {puntoOeste.lng}");

                // --- CREAR OBJETO SIN GUARDAR LAT/LNG NI METROS ---
                var recorrido = new ClsRecorrido
                {
                    IdUsuario = userId,
                    IdCortejo = CortejoSeleccionado.Id,
                    Nombre = Nombre,
                    LugarPartida = LugarPartida,
                    Horario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DuracionRecorrido = duracionMinutos,
                    Itinerario = new List<string>()
                };

                bool ok = await _recorridoBL.CrearRecorridoAsync(recorrido);

                if (ok)
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Éxito",
                        $"Recorrido creado correctamente.\nDistancia estimada: {metrosRecorrido:F2} metros.",
                        "OK");

                    // LIMPIAR CAMPOS
                    Nombre = string.Empty;
                    LugarPartida = string.Empty;
                    Duracion = TimeSpan.FromHours(1);
                    CortejoSeleccionado = null;

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
