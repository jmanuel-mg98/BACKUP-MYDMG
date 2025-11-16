using ConectorAppWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL;

namespace MyDMG_app.viewModels
{
    public class HomeViewModel
    {
        public ICommand LogoutCommand { get; }
        public String NombreUsuario { get;}

        public HomeViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
            NombreUsuario = ConectorAppwrite.user.UserId == null ? "Usuario Desconocido" : ConectorAppwrite.user.UserId;
        }

        private async Task Logout()
        {
            ConectorAppwrite.cerrarSesion();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
