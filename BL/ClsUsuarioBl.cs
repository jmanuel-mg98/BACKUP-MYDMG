using ENT;
using DAL;
using System.Threading.Tasks;

namespace BL
{
    public class ClsUsuarioBl
    {
        private readonly ClsUsuarioDal _usuarioDal;
        private readonly ClsDetalleUsuarioDal _detalleUsuarioDal;

        public ClsUsuarioBl()
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
            bool ok = await _usuarioDal.RegistrarAsync(usuario);
            if (!ok) return false;

            bool loginOk = await _usuarioDal.LoginAsync(usuario.Email, usuario.Password);
            if (!loginOk) return false;

            bool detalleOk = await _detalleUsuarioDal.CrearDetalleUsuarioAsync(usuario);
            return detalleOk;
        }

        // 🔹 Nuevo método: obtener usuario completo
        public async Task<ClsUsuario?> ObtenerUsuarioCompletoAsync(string userId)
        {
            // 1️⃣ Datos desde Auth
            var usuarioAuth = await _usuarioDal.ObtenerUsuarioAuthAsync();
            if (usuarioAuth == null) return null;

            // 2️⃣ Detalles desde DB
            var detalles = await _detalleUsuarioDal.ObtenerDetalleUsuarioAsync(userId);

            if (detalles != null)
            {
                usuarioAuth.Hermandad = detalles.Hermandad;
                usuarioAuth.EsAdmin = detalles.EsAdmin;
            }

            return usuarioAuth;
        }

        public async Task<ClsUsuario> ObtenerDetalleUsuarioActualAsync(string userId)
        {
            return await _detalleUsuarioDal.ObtenerDetalleUsuarioAsync(userId);
        }
    }
}

