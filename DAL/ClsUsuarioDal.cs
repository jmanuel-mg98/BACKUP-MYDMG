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

        /// <summary>
        /// funcion que llama al conector para realizar el inicio de sesion de la aplicacion
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(string email, string password)
        {
            return await ConectorAppwrite.iniciarSesion(email, password);
        }

        /// <summary>
        /// funcion que llama al conector para registrar un nuevo usuario en la aplicacion
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<bool> RegistrarAsync(ClsUsuario usuario)
        {
            return await ConectorAppwrite.registrarUsuario(usuario.Email, usuario.Password, usuario.Nombre);
        }

        /// <summary>
        /// consigue el usuario autenticado actualmente
        /// </summary>
        /// <returns></returns>
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



