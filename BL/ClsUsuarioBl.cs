using ENT;
using DAL;

namespace BL
{
    public class ClsUsuarioBL
    {
        private readonly ClsUsuarioDal _usuarioDal;
        private readonly ClsDetalleUsuarioDal _detalleUsuarioDal;

        public ClsUsuarioBL()
        {
            _usuarioDal = new ClsUsuarioDal();
            _detalleUsuarioDal = new ClsDetalleUsuarioDal();
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            return await _usuarioDal.LoginAsync(email, password);
        }

        public async Task<bool> RegistrarUsuarioCompleto(ClsUsuario usuario)
        {
            // 1️⃣ Registrar en Auth
            bool ok = await _usuarioDal.RegistrarAsync(usuario);
            if (!ok) return false;

            // 2️⃣ Iniciar sesión para obtener UserId
            bool loginOk = await _usuarioDal.LoginAsync(usuario.Email, usuario.Password);
            if (!loginOk) return false;

            // 3️⃣ Crear detalle en DB
            bool detalleOk = await _detalleUsuarioDal.CrearDetalleUsuarioAsync(usuario);
            return detalleOk;
        }
    }
}



