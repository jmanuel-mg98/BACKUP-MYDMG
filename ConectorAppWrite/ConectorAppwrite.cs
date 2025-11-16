using Appwrite;
using Appwrite.Models;
using Appwrite.Services;

namespace ConectorAppWrite
{
    public class ConectorAppwrite
    {
        public static Client client = new Client();
        public static Session user = null;
        private static Account account = null;

        /// <summary>
        /// Inicializa la conexión con Appwrite estableciendo el endpoint, el ID del proyecto y la clave API.
        /// </summary>
        public static void inicializar()
        {
            client
                .SetEndpoint("https://fra.cloud.appwrite.io/v1")
                .SetProject("6919c3be003047bd553c")
                .SetKey("standard_34c938590b093f5d502308e53803996e9d9b95a8add209584fc6e15447a39724497b9020569697661048343ab4f9c5d70d07e48c4b0b5025bb9a1cf486f3343b8a8747dc884f39715e2c991a1b0a2077d711cc9d0a4ec635241eb0a873955fb58ea8b3ccc95cf0a32f3a786ba6187576f4fe472cdac391b5b7ef3cfe45dec143");

           account = new Account(client);
        }

        async public static Task<bool> iniciarSesion(String email, String password)
        {
            bool resultado = false;
            try
            {
                user = await account.CreateEmailPasswordSession(email, password);
                resultado = true;
            }
            catch (AppwriteException ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }

            return resultado;
        }

        async public static Task<bool> registrarUsuario(String email, String password, String nombre)
        {
            bool resultado = false;
            try
            {
                await account.Create(ID.Unique(), email, password, nombre);
                resultado = true;
            }
            catch (AppwriteException ex)
            {
                Console.WriteLine($"Error al registrar usuario: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }

            return resultado;
        }

        /// <summary>
        /// Cierra la sesion activa del usuario.
        /// </summary>
        public static void cerrarSesion()
        {
            account.DeleteSession("current");
        }
    }
}
