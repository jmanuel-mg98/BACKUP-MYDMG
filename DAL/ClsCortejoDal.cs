using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public class ClsCortejoDal
    {
        private readonly Databases _db;
        private const string DATABASE_ID = "691ce72500229460591f"; // ID real de tu DB
        private const string COLLECTION_ID = "cortejos"; // ID real de tu tabla

        public ClsCortejoDal()
        {
            _db = new Databases(ConectorAppwrite.Client);
        }

        public async Task<bool> CrearCortejoAsync(ClsCortejo cortejo)
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
                        { "nombreCortejo", cortejo.NombreCortejo },
                        { "nParticipantes", cortejo.NParticipantes },
                        { "esPaso", cortejo.EsPaso },
                        { "cantidadPasos", cortejo.CantidadPasos },
                        { "llevaBanda", cortejo.LlevaBanda },
                        { "velocidadMedia", cortejo.VelocidadMedia }
                    }
                );

                return true;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al crear cortejo: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al crear cortejo: {ex.Message}");
            }
        }

        public async Task<List<ClsCortejo>> ObtenerCortejosPorUsuarioAsync(string userId)
        {
            try
            {
                var result = await _db.ListDocuments(
                    databaseId: DATABASE_ID,
                    collectionId: COLLECTION_ID,
                    queries: new List<string> { Query.Equal("idUsuario", userId) }
                );

                var lista = new List<ClsCortejo>();

                foreach (var doc in result.Documents)
                {
                    var data = doc.Data;
                    lista.Add(new ClsCortejo
                    {
                        Id = doc.Id,
                        NombreCortejo = data.ContainsKey("nombreCortejo") ? data["nombreCortejo"].ToString() : "Sin nombre",
                        NParticipantes = data.ContainsKey("nParticipantes") ? Convert.ToInt32(data["nParticipantes"]) : 0,
                        EsPaso = data.ContainsKey("esPaso") ? (bool)data["esPaso"] : false,
                        CantidadPasos = data.ContainsKey("cantidadPasos") ? Convert.ToInt32(data["cantidadPasos"]) : 0,
                        LlevaBanda = data.ContainsKey("llevaBanda") ? (bool)data["llevaBanda"] : false,
                        VelocidadMedia = data.ContainsKey("velocidadMedia") ? Convert.ToDouble(data["velocidadMedia"]) : 0,
                        IdUsuario = data.ContainsKey("idUsuario") ? data["idUsuario"].ToString() : null
                    });
                }

                return lista;
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Error al obtener cortejos: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener cortejos: {ex.Message}");
            }
        }
    }
}




