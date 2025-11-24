using ConectorAppWrite;
using ENT;
using BL;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDMG_app.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly ClsUsuarioBl _usuarioBL;
        private readonly ClsCortejoBl _cortejoBL;

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

        public ObservableCollection<ClsCortejo> Cortejos { get; set; } = new ObservableCollection<ClsCortejo>();

        public ICommand LogoutCommand { get; }
        public ICommand NavegarCrearCortejoCommand { get; }

        public HomeViewModel()
        {
            _usuarioBL = new ClsUsuarioBl();
            _cortejoBL = new ClsCortejoBl();

            LogoutCommand = new Command(async () => await Logout());
            NavegarCrearCortejoCommand = new Command(async () => await Shell.Current.GoToAsync("//CrearCortejoPage"));

            // Cargar datos y cortejos al inicio
            Task.Run(async () => await CargarDatosUsuarioAsync());
            Task.Run(async () => await CargarCortejosAsync());
        }

        public async Task CargarDatosUsuarioAsync()
        {
            try
            {
                var sesion = await ConectorAppwrite.GetSesionActual();
                if (sesion == null)
                {
                    NombreUsuario = "Usuario desconocido";
                    Hermandad = "Hermandad desconocida";
                    return;
                }

                var usuarioCompleto = await _usuarioBL.ObtenerUsuarioCompletoAsync(sesion.UserId);

                if (usuarioCompleto != null)
                {
                    NombreUsuario = usuarioCompleto.Nombre ?? "Sin nombre";
                    Hermandad = usuarioCompleto.Hermandad ?? "Sin hermandad";
                }
                else
                {
                    NombreUsuario = "Error";
                    Hermandad = "No se pudo cargar";
                }
            }
            catch (Exception ex)
            {
                NombreUsuario = "Error al cargar usuario";
                Hermandad = ex.Message;
            }
        }

        public async Task CargarCortejosAsync()
        {
            try
            {
                Cortejos.Clear();
                var sesion = await ConectorAppwrite.GetSesionActual();
                if (sesion == null) return;

                var listaCortejos = await _cortejoBL.ObtenerCortejosUsuarioAsync(sesion.UserId);
                foreach (var c in listaCortejos)
                    Cortejos.Add(c);
            }
            catch (Exception ex)
            {
                // Manejar errores de carga de cortejos
            }
        }

        private async Task Logout()
        {
            ConectorAppwrite.cerrarSesion();

            // Limpiar datos y lista
            NombreUsuario = string.Empty;
            Hermandad = string.Empty;
            Cortejos.Clear();

            await Shell.Current.GoToAsync("//LoginPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




