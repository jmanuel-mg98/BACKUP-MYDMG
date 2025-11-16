namespace MyDMG_app.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new viewModels.HomeViewModel();
    }
}