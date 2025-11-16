using ENT;
using DAL;


namespace BL
{
    public class ClsUsuarioBL
    {
        private readonly ClsUsuarioDal _usuarioDal;

        public ClsUsuarioBL()
        {
            _usuarioDal = new ClsUsuarioDal();
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email y contraseña son requeridos");

            return await _usuarioDal.LoginAsync(email, password);
        }

        public async Task<bool> RegistrarAsync(ClsUsuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Password))
                throw new ArgumentException("Email y contraseña son requeridos");

            return await _usuarioDal.RegistrarAsync(usuario);
        }
    }
}

