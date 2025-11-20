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
    }
}



