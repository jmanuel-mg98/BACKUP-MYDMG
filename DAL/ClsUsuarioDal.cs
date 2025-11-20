using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsUsuarioDal
    {
        public async Task<bool> LoginAsync(string email, string password)
        {
            return await ConectorAppwrite.iniciarSesion(email, password);
        }

        public async Task<bool> RegistrarAsync(ClsUsuario usuario)
        {
            return await ConectorAppwrite.registrarUsuario(usuario.Email, usuario.Password, usuario.Nombre);
        }
    }
}


