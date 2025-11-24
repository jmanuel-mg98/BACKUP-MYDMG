using ConectorAppWrite;
using ENT;
using Appwrite.Services;
using Appwrite.Models;

namespace DAL
{
    public class ClsUsuarioDal
    {
        private readonly Account _account;

        public ClsUsuarioDal()
        {
            _account = new Account(ConectorAppwrite.Client);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            return await ConectorAppwrite.iniciarSesion(email, password);
        }

        public async Task<bool> RegistrarAsync(ClsUsuario usuario)
        {
            return await ConectorAppwrite.registrarUsuario(usuario.Email, usuario.Password, usuario.Nombre);
        }

        //  Obtener datos del usuario desde AUTH
        public async Task<ClsUsuario?> ObtenerUsuarioAuthAsync()
        {
            try
            {
                var user = await _account.Get(); 

                return new ClsUsuario
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nombre = user.Name,     
                    Password = "",
                    Hermandad = "",
                    EsAdmin = false
                };
            }
            catch
            {
                return null;
            }
        }
    }
}



