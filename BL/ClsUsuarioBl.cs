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
        /// <summary>
        /// funcion que llama a la dal para realizar el inicio de sesion
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(string email, string password)
        {
            return await _usuarioDal.LoginAsync(email, password);
        }
        /// <summary>
        /// funcion que llama a la dal para registrar un usuario completo (auth + detalle)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<bool> RegistrarUsuarioCompleto(ClsUsuario usuario)
        {
            bool ok = await _usuarioDal.RegistrarAsync(usuario);
            if (!ok) return false;

            bool loginOk = await _usuarioDal.LoginAsync(usuario.Email, usuario.Password);
            if (!loginOk) return false;

            bool detalleOk = await _detalleUsuarioDal.CrearDetalleUsuarioAsync(usuario);
            return detalleOk;
        }

        /// <summary>
        /// funcion que obtiene el usuario completo (auth + detalle) a partir del id del usuario que tiene la sesion iniciada
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// funcion que obtiene el detalle del usuario (hermandad y si es admin o no) a partir del id del usuario que tiene la sesion iniciada
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ClsUsuario> ObtenerDetalleUsuarioActualAsync(string userId)
        {
            return await _detalleUsuarioDal.ObtenerDetalleUsuarioAsync(userId);
        }
    }
}

