using MyDMG_app.ViewModels;

namespace MyDMG_app.Views
{
    [QueryProperty(nameof(DetalleCortejoViewModel.CortejoId), "id")]
    public partial class DetalleCortejoPage : ContentPage
    {
        public DetalleCortejoPage()
        {
            InitializeComponent();
        }
    }
}




