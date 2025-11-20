using ENT;
using BL;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ConectorAppWrite;
using Appwrite.Models;
using Microsoft.Maui.Storage;

namespace MyDMG_app.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private readonly ClsUsuarioBL _usuarioBL;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel()
        {
            _usuarioBL = new ClsUsuarioBL();

            LoginCommand = new Command(async () => await Login());
            GoToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//RegisterPage"));

            // Verificar si hay sesión activa al iniciar
            _ = VerificarSesionActiva();
        }

        private async Task VerificarSesionActiva()
        {
            try
            {
                Session? sesionActual = await ConectorAppwrite.GetSesionActual();

                if (sesionActual != null)
                {
                    await Shell.Current.GoToAsync("//HomePage");
                }
            }
            catch
            {
                // Ignorar errores de sesión, simplemente no navegar
            }
        }

        private async Task Login()
        {
            try
            {
                bool loginExitoso = await _usuarioBL.LoginAsync(Email, Password);

                if (loginExitoso)
                {
                    // Guardar email en almacenamiento seguro
                    await SecureStorage.Default.SetAsync("usuarioEmail", Email);

                    // Limpiar campos
                    Email = string.Empty;
                    Password = string.Empty;

                    // Navegar a HomePage
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Credenciales inválidas", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




