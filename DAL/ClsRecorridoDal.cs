using Appwrite;
using Appwrite.Services;
using ConectorAppWrite;
using ENT;

namespace DAL
{
    public class ClsRecorridoDal
    {
        private static readonly TablesDB _tables = new TablesDB(ConectorAppwrite.Client);
        private const string DATABASE_ID = "691ce72500229460591f";
        private const string COLLECTION_ID = "recorridos"; // Asegúrate que este es el ID de tu colección

        /// <summary>
        /// Obtiene todos los recorridos de un usuario específico.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de recorridos</returns>
        public async Task<List<ClsRecorrido>> ObtenerRecorridosUsuarioAsync(string userId)
        {
            try
            {
                var result = await _tables.ListRows(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    queries: new List<string>
                    {
                        Query.Equal("idUsuario", userId)
                    }
                );

                return result.Rows.Select(d => new ClsRecorrido
                {
                    Id = d.Id,
                    IdUsuario = d.Data["idUsuario"]?.ToString(),
                    IdCortejo = d.Data["idCortejo"]?.ToString(),
                    Nombre = d.Data["nombre"]?.ToString(),
                    LugarPartida = d.Data["lugarPartida"]?.ToString(),
                    Horario = d.Data["horario"]?.ToString(),
                    DuracionRecorrido = Convert.ToInt32(d.Data["duracionRecorrido"]),
                    Itinerario = d.Data.ContainsKey("itinerario") && d.Data["itinerario"] != null
                        ? ((System.Collections.IEnumerable)d.Data["itinerario"]).Cast<object>().Select(x => x.ToString()).ToList()
                        : new List<string>()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener recorridos: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un recorrido por su ID.
        /// </summary>
        /// <param name="id">ID del recorrido</param>
        /// <returns>Recorrido encontrado</returns>
        public async Task<ClsRecorrido> ObtenerRecorridoPorIdAsync(string id)
        {
            try
            {
                var d = await _tables.GetRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: id
                );

                return new ClsRecorrido
                {
                    Id = d.Id,
                    IdUsuario = d.Data["idUsuario"]?.ToString(),
                    IdCortejo = d.Data["idCortejo"]?.ToString(),
                    Nombre = d.Data["nombre"]?.ToString(),
                    LugarPartida = d.Data["lugarPartida"]?.ToString(),
                    Horario = d.Data["horario"]?.ToString(),
                    DuracionRecorrido = Convert.ToInt32(d.Data["duracionRecorrido"]),
                    Itinerario = d.Data.ContainsKey("itinerario") && d.Data["itinerario"] != null
                        ? ((System.Collections.IEnumerable)d.Data["itinerario"]).Cast<object>().Select(x => x.ToString()).ToList()
                        : new List<string>()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener recorrido: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un nuevo recorrido en la base de datos.
        /// </summary>
        /// <param name="recorrido">Objeto recorrido a crear</param>
        /// <returns>True si se creó correctamente</returns>
        public async Task<bool> CrearRecorridoAsync(ClsRecorrido recorrido)
        {
            try
            {
                await _tables.CreateRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: ID.Unique(),
                    data: new Dictionary<string, object>
                    {
                        { "idUsuario", recorrido.IdUsuario },
                        { "idCortejo", recorrido.IdCortejo },
                        { "nombre", recorrido.Nombre },
                        { "lugarPartida", recorrido.LugarPartida },
                        { "horario", recorrido.Horario },
                        { "duracionRecorrido", recorrido.DuracionRecorrido },
                        { "itinerario", recorrido.Itinerario ?? new List<string>() }
                    }
                );
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear recorrido: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza un recorrido existente.
        /// </summary>
        /// <param name="recorrido">Recorrido con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        public async Task<bool> ActualizarRecorridoAsync(ClsRecorrido recorrido)
        {
            try
            {
                await _tables.UpdateRow(
                    databaseId: DATABASE_ID,
                    tableId: COLLECTION_ID,
                    rowId: recorrido.Id,
                    data: new Dictionary<string, object>
                    {
                        { "idCortejo", recorrido.IdCortejo },
                        { "nombre", recorrido.Nombre },
                        { "lugarPartida", recorrido.LugarPartida },
                        { "horario", recorrido.Horario },
                        { "duracionRecorrido", recorrido.DuracionRecorrido },
                        { "itinerario", recorrido.Itinerario ?? new List<string>() }
                    }
                );
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar recorrido: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un recorrido de la base de datos.
        /// </summary>
        /// <param name="id">ID del recorrido a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        public async Task<bool> EliminarRecorridoAsync(string id)
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
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar recorrido: {ex.Message}");
            }
        }
    }
}
