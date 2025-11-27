using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsCortejoDal
    {
        private static readonly TablesDB _tables = new TablesDB(ConectorAppwrite.Client);

        private const string DATABASE_ID = "691ce72500229460591f";
        private const string COLLECTION_ID = "cortejos";

        public ClsCortejoDal()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ClsCortejo>> ObtenerCortejosUsuarioAsync(string userId)
        {
            var result = await _tables.ListRows(
                databaseId: DATABASE_ID,
                tableId: COLLECTION_ID,
                queries: new List<string>
                {
                    Query.Equal("idUsuario", userId)
                }
            );

            return result.Rows.Select(d => new ClsCortejo
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClsCortejo> ObtenerCortejoPorIdAsync(string id)
        {
            var d = await _tables.GetRow(
                databaseId: DATABASE_ID,
                tableId: COLLECTION_ID,
                rowId: id
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public async Task<bool> CrearCortejoAsync(ClsCortejo c)
        {
            try
            {
                await _tables.CreateRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: ID.Unique(),
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public async Task<bool> ActualizarCortejoAsync(ClsCortejo c)
        {
            try
            {
                await _tables.UpdateRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: c.Id,
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> EliminarCortejoAsync(string id)
        {
            try
            {
                await _tables.DeleteRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: id
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









