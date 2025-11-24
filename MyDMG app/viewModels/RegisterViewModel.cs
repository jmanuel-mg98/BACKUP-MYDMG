using ENT;
using BL;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly ClsUsuarioBl _usuarioBL;

        private string _nombre;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _hermandad;

        public string Nombre { get => _nombre; set { _nombre = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }
        public string Hermandad { get => _hermandad; set { _hermandad = value; OnPropertyChanged(); } }

        public ICommand RegistrarCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public RegisterViewModel()
        {
            _usuarioBL = new ClsUsuarioBl();
            RegistrarCommand = new Command(async () => await Registrar());
            GoToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));
        }

        private async Task Registrar()
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "La contraseña no coincide", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Hermandad))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                    return;
                }

                var usuario = new ClsUsuario
                {
                    Nombre = Nombre,
                    Email = Email,
                    Password = Password,
                    Hermandad = Hermandad
                };

                bool registrado = await _usuarioBL.RegistrarUsuarioCompleto(usuario);

                if (!registrado)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No se pudo registrar el usuario", "OK");
                    return;
                }

                await Shell.Current.GoToAsync("//HomePage");
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




