namespace MyDMG_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Task.Run(async () =>
            {
                var email = await SecureStorage.Default.GetAsync("usuarioEmail");
                if (!string.IsNullOrEmpty(email))
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Shell.Current.GoToAsync("//HomePage");
                    });
                }
            });
        }
    }
}
