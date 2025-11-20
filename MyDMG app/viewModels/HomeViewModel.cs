using ConectorAppWrite;
using ENT;
using BL;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ClsUsuarioBL _usuarioBL;

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

        public ICommand LogoutCommand { get; }

        public HomeViewModel()
        {
            _usuarioBL = new ClsUsuarioBL();
            LogoutCommand = new Command(async () => await Logout());

            // Cargar los datos del usuario actual
            Task.Run(async () => await CargarDatosUsuario());
        }

        private async Task CargarDatosUsuario()
        {
            try
            {
                // Nombre desde Auth
                var sesion = await ConectorAppwrite.GetSesionActual();
                NombreUsuario = sesion?.UserId ?? "Usuario Desconocido"; // Aquí puedes reemplazar UserId por nombre si Appwrite lo devuelve

                if (sesion != null)
                {
                    // Hermandad desde la base de datos
                    var detalleUsuario = await _usuarioBL.ObtenerDetalleUsuarioActualAsync(sesion.UserId);
                    Hermandad = detalleUsuario?.Hermandad ?? "Hermandad desconocida";
                }
                else
                {
                    Hermandad = "Hermandad desconocida";
                }
            }
            catch (Exception ex)
            {
                NombreUsuario = "Error al cargar usuario";
                Hermandad = ex.Message;
            }
        }

        private async Task Logout()
        {
            ConectorAppwrite.cerrarSesion();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
