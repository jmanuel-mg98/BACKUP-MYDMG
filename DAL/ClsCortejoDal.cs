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
        /// funcion que llama a la base de datos para obtener los cortejos del usuario que tiene la sesion inicada 
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
                Id = d.Id ?? d.Data["$id"]?.ToString(),
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
        /// funcion que obtiene un cortejo por su id para podersarselo a detallecortejo
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
                Id = d.Id ?? d.Data["$id"]?.ToString(),
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
        /// funcion que crea un nuevo cortejo en la base de datos asignadole el id del usuario que tiene la sesion iniciada
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
        /// funcion que actualiza un cortejo en la base de datos a partir de su id
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
        /// funcion que elimina un cortejo de la base de datos a partir de su id 
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









