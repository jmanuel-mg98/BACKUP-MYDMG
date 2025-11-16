using ENT;
using BL;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
        public ICommand RegistrarCommand { get; }

        public LoginViewModel()
        {
            _usuarioBL = new ClsUsuarioBL();
            LoginCommand = new Command(async () => await Login());
            RegistrarCommand = new Command(async () => await Registrar());
        }

        private async Task Login()
        {
            try
            {
                bool result = await _usuarioBL.LoginAsync(Email, Password);
                if (result)
                {
                    // Guardar el usuario en almacenamiento seguro
                    await SecureStorage.Default.SetAsync("usuarioEmail", Email);

                    // Navegar a la página principal
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

        private async Task Registrar()
        {
            try
            {
                var usuario = new ClsUsuario { Email = Email, Password = Password };
                bool result = await _usuarioBL.RegistrarAsync(usuario);

                if (result)
                    await App.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo registrar", "OK");
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


