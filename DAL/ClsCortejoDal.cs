using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsCortejoDal
    {
        private readonly Databases _db;
        private const string DATABASE_ID = "691ce72500229460591f";
        private const string COLLECTION_ID = "cortejos";

        public ClsCortejoDal()
        {
            _db = new Databases(ConectorAppwrite.Client);
        }

        // -----------------------
        // Crear Cortejo
        // -----------------------
        public async Task<bool> CrearCortejoAsync(ClsCortejo c)
        {
            try
            {
                await _db.CreateDocument(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    documentId: ID.Unique(),
                    data: new Dictionary<string, object>
                    {
                        { "NombreCortejo", c.NombreCortejo },
                        { "NParticipantes", c.NParticipantes },
                        { "EsPaso", c.EsPaso },
                        { "CantidadPasos", c.CantidadPasos },
                        { "LlevaBanda", c.LlevaBanda },
                        { "VelocidadMedia", c.VelocidadMedia },
                        { "IdUsuario", c.IdUsuario }
                    }
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        // -----------------------
        // Obtener Cortejos de un usuario
        // -----------------------
        public async Task<List<ClsCortejo>> ObtenerCortejosUsuarioActualAsync()
        {
            string userId = ConectorAppwrite.Sesion?.UserId;
            if (string.IsNullOrEmpty(userId))
                return new();

            var result = await _db.ListDocuments(
                databaseId: DATABASE_ID,
                collectionId: COLLECTION_ID,
                queries: new List<string>
                {
                    Query.Equal("idUsuario", userId)
                }
            );

            List<ClsCortejo> lista = new();

            foreach (var doc in result.Documents)
            {
                lista.Add(MapearCortejo(doc.Id, doc.Data));
            }

            return lista;
        }

        // -----------------------
        // Obtener por ID
        // -----------------------
        public async Task<ClsCortejo> ObtenerCortejoPorIdAsync(string id)
        {
            var doc = await _db.GetDocument(DATABASE_ID, COLLECTION_ID, id);
            return MapearCortejo(doc.Id, doc.Data);
        }

        // -----------------------
        // Actualizar Cortejo
        // -----------------------
        public async Task<bool> ActualizarCortejoAsync(ClsCortejo c)
        {
            try
            {
                await _db.UpdateDocument(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    documentId: c.Id,
                    data: new Dictionary<string, object>
                    {
                        { "NombreCortejo", c.NombreCortejo },
                        { "NParticipantes", c.NParticipantes },
                        { "EsPaso", c.EsPaso },
                        { "CantidadPasos", c.CantidadPasos },
                        { "LlevaBanda", c.LlevaBanda },
                        { "VelocidadMedia", c.VelocidadMedia },
                        { "IdUsuario", c.IdUsuario }
                    }
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        // -----------------------
        // Eliminar Cortejo
        // -----------------------
        public async Task<bool> EliminarCortejoAsync(string id)
        {
            try
            {
                await _db.DeleteDocument(DATABASE_ID, COLLECTION_ID, id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // -----------------------
        // MÉTODO PRIVADO PARA MAPEAR SIN JSON
        // -----------------------
        private ClsCortejo MapearCortejo(string id, IDictionary<string, object> data)
        {
            return new ClsCortejo
            {
                Id = id,
                NombreCortejo = data.ContainsKey("NombreCortejo") ? data["NombreCortejo"]?.ToString() : "",
                NParticipantes = data.ContainsKey("NParticipantes") ? Convert.ToInt32(data["NParticipantes"]) : 0,
                EsPaso = data.ContainsKey("EsPaso") && Convert.ToBoolean(data["EsPaso"]),
                CantidadPasos = data.ContainsKey("CantidadPasos") ? Convert.ToInt32(data["CantidadPasos"]) : 0,
                LlevaBanda = data.ContainsKey("LlevaBanda") && Convert.ToBoolean(data["LlevaBanda"]),
                VelocidadMedia = data.ContainsKey("VelocidadMedia") ? Convert.ToDouble(data["VelocidadMedia"]) : 0,
                IdUsuario = data.ContainsKey("IdUsuario") ? data["IdUsuario"]?.ToString() : ""
            };
        }
    }
}







