using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<bool> CrearCortejoAsync(ClsCortejo cortejo)
        {
            try
            {
                await _db.CreateDocument(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    documentId: ID.Unique(),
                    data: new Dictionary<string, object>
                    {
                        { "NombreCortejo", cortejo.NombreCortejo },
                        { "nParticipantes", cortejo.NParticipantes },
                        { "esPaso", cortejo.EsPaso },
                        { "cantidadPasos", cortejo.CantidadPasos },
                        { "llevaBanda", cortejo.LlevaBanda },
                        { "velocidadMedia", cortejo.VelocidadMedia },
                        { "idUsuario", cortejo.IdUsuario }
                    });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ClsCortejo>> ObtenerCortejosUsuarioAsync(string userId)
        {
            try
            {
                var result = await _db.ListDocuments(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    queries: new List<string>
                    {
                        Query.Equal("idUsuario", userId)
                    }
                );

                var cortejos = new List<ClsCortejo>();
                foreach (var doc in result.Documents)
                {
                    var data = doc.Data;
                    cortejos.Add(new ClsCortejo
                    {
                        NombreCortejo = data.ContainsKey("NombreCortejo") ? data["NombreCortejo"]?.ToString() : "",
                        NParticipantes = data.ContainsKey("nParticipantes") ? Convert.ToInt32(data["nParticipantes"]) : 0,
                        EsPaso = data.ContainsKey("esPaso") && data["esPaso"] != null && (bool)data["esPaso"],
                        CantidadPasos = data.ContainsKey("cantidadPasos") ? Convert.ToInt32(data["cantidadPasos"]) : 0,
                        LlevaBanda = data.ContainsKey("llevaBanda") && data["llevaBanda"] != null && (bool)data["llevaBanda"],
                        VelocidadMedia = data.ContainsKey("velocidadMedia") ? Convert.ToDouble(data["velocidadMedia"]) : 0,
                        IdUsuario = userId
                    });
                }

                return cortejos;
            }
            catch
            {
                return new List<ClsCortejo>();
            }
        }
    }
}




