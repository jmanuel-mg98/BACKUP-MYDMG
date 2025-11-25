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

        public async Task<List<ClsCortejo>> ObtenerCortejosUsuarioAsync(string userId)
        {
            var result = await _db.ListDocuments(
                databaseId: DATABASE_ID,
                collectionId: COLLECTION_ID,
                queries: new List<string>
                {
                    Query.Equal("idUsuario", userId)
                }
            );

            return result.Documents.Select(d => new ClsCortejo
            {
                Id = d.Id,
                NombreCortejo = d.Data["NombreCortejo"].ToString(),
                NParticipantes = Convert.ToInt32(d.Data["NParticipantes"]),
                EsPaso = Convert.ToBoolean(d.Data["EsPaso"]),
                CantidadPasos = Convert.ToInt32(d.Data["CantidadPasos"]),
                LlevaBanda = Convert.ToBoolean(d.Data["LlevaBanda"]),
                VelocidadMedia = Convert.ToDouble(d.Data["VelocidadMedia"]),
                IdUsuario = d.Data["idUsuario"].ToString()
            }).ToList();
        }

        public async Task<ClsCortejo> ObtenerCortejoPorIdAsync(string id)
        {
            var d = await _db.GetDocument(
                databaseId: DATABASE_ID,
                collectionId: COLLECTION_ID,
                documentId: id
            );

            return new ClsCortejo
            {
                Id = d.Id,
                NombreCortejo = d.Data["NombreCortejo"].ToString(),
                NParticipantes = Convert.ToInt32(d.Data["NParticipantes"]),
                EsPaso = Convert.ToBoolean(d.Data["EsPaso"]),
                CantidadPasos = Convert.ToInt32(d.Data["CantidadPasos"]),
                LlevaBanda = Convert.ToBoolean(d.Data["LlevaBanda"]),
                VelocidadMedia = Convert.ToDouble(d.Data["VelocidadMedia"]),
                IdUsuario = d.Data["idUsuario"].ToString()
            };
        }

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
                        { "idUsuario", c.IdUsuario }
                    }
                );
                return true;
            }
            catch
            {
                return false;
            }
        }

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
                        { "VelocidadMedia", c.VelocidadMedia }
                    }
                );
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarCortejoAsync(string id)
        {
            try
            {
                await _db.DeleteDocument(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    documentId: id
                );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}









