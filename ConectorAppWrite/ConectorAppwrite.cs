using Appwrite;
using Appwrite.Models;
using Appwrite.Services;

namespace ConectorAppWrite
{
    public class ConectorAppwrite
    {
        public static Client Client = new Client();
        public static Session Sesion = null;
        private static Account Account = null;

        public static void inicializar()
        {
            Client
                .SetEndpoint("https://fra.cloud.appwrite.io/v1")
                .SetProject("6919c3be003047bd553c");

            Account = new Account(Client);
        }

        public static async Task<bool> iniciarSesion(string email, string password)
        {
            try
            {
                Sesion = await Account.CreateEmailPasswordSession(email, password);
                return true;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al iniciar sesión: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al iniciar sesión: {ex.Message}");
            }
        }

        public static async Task<bool> registrarUsuario(string email, string password, string nombre)
        {
            try
            {
                string idNuevoUsuario = ID.Unique();
                await Account.Create(idNuevoUsuario, email, password, nombre);
                return true;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al registrar usuario: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al registrar usuario: {ex.Message}");
            }
        }
        public static async Task<Session?> GetSesionActual()
        {
            try
            {
                Sesion = await Account.GetSession("current");
                return Sesion;
            }
            catch (AppwriteException ex)
            {
                Console.WriteLine($"No hay sesión activa: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return null;
            }
        }

        public static async Task cerrarSesion()
        {
            try
            {
                await Account.DeleteSession("current");
            }
            catch { }

            Sesion = null;
        }
    }
}


