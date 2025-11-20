using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsDetalleUsuarioDal
    {
        private readonly Databases _db;
        private const string DATABASE_ID = "691ce72500229460591f";   // ID real de tu DB
        private const string COLLECTION_ID = "detallesusuario";       // ID real de tu table

        public ClsDetalleUsuarioDal()
        {
            _db = new Databases(ConectorAppwrite.Client);
        }

        public async Task<bool> CrearDetalleUsuarioAsync(ClsUsuario usuario)
        {
            try
            {
                string userId = ConectorAppwrite.Sesion?.UserId;
                if (string.IsNullOrEmpty(userId))
                    return false;

                await _db.CreateDocument(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    documentId: ID.Unique(),
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

        // Nuevo método para obtener detalles del usuario por userId
        public async Task<ClsUsuario> ObtenerDetalleUsuarioAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return null;

                var result = await _db.ListDocuments(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    queries: new List<string>
                    {
                            $"field=idUsuario&value={userId}"
                    }
                );

                if (result.Documents.Count > 0)
                {
                    var doc = result.Documents[0];
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



