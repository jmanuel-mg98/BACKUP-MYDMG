using MyDMG_app.ViewModels;

namespace MyDMG_app.Views
{
    [QueryProperty(nameof(DetalleRecorridoViewModel.RecorridoId), "id")]
    public partial class DetalleRecorridoPage : ContentPage
    {
        public DetalleRecorridoPage()
        {
            InitializeComponent();
        }
    }
}