using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.viewModels
{
    public class HomeViewModel
    {
        public ICommand LogoutCommand { get; }

        public HomeViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
        }

        private async Task Logout()
        {
            SecureStorage.Default.Remove("usuarioEmail");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
