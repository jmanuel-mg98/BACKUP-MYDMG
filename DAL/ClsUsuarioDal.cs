using ENT;
using Plugin.Firebase.Auth;

namespace DAL
{
    public class ClsUsuarioDal
    {
        private readonly IFirebaseAuth _firebaseAuth;

        public ClsUsuarioDal()
        {
            _firebaseAuth = CrossFirebaseAuth.Current;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                return user != null; // Adjusted to check the user object directly
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegistrarAsync(ClsUsuario usuario)
        {
            try
            {
                var user = await _firebaseAuth.CreateUserAsync(usuario.Email, usuario.Password);
                return user != null; // Adjusted to check the user object directly
            }
            catch
            {
                return false;
            }
        }
    }
}

