using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsDetalleUsuarioDal
    {
        private static readonly TablesDB _tables = new TablesDB(ConectorAppwrite.Client);
        private const string DATABASE_ID = "691ce72500229460591f";   
        private const string COLLECTION_ID = "detallesusuario";

        public ClsDetalleUsuarioDal()
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CrearDetalleUsuarioAsync(ClsUsuario usuario)
        {
            try
            {
                string? userId = ConectorAppwrite.Sesion?.UserId;
                if (string.IsNullOrEmpty(userId))
                    return false;


                await _tables.CreateRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: ID.Unique(),
                    data: new Dictionary<string, object>
                    {
                            { "idUsuario", userId },
                            { "Hermandad", usuario.Hermandad },
                            { "esAdmin", false }
                    }
                );

                return true;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al crear detalle usuario: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al crear detalle usuario: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClsUsuario?> ObtenerDetalleUsuarioAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return null;

                var result = await _tables.ListRows(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    queries: new List<string>
                    {
                        Query.Equal("idUsuario", userId)
                    }
                );
               

                if (result.Rows.Count > 0)
                {
                    var doc = result.Rows[0];
                    var data = doc.Data; // Accede al diccionario de datos del documento
                    return new ClsUsuario
                    {
                        Hermandad = data.ContainsKey("Hermandad") ? data["Hermandad"]?.ToString() : null,
                        EsAdmin = data.ContainsKey("esAdmin") && data["esAdmin"] != null && (bool)data["esAdmin"]
                    };
                }

                return null;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al obtener detalle usuario: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener detalle usuario: {ex.Message}");
            }
        }
    }
}



