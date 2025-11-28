using DAL;
using ENT;

namespace BL
{
    public class ClsRecorridoBl
    {
        private readonly ClsRecorridoDal _dal = new();
        /// <summary>
        /// funcion que llama a la dal para obtener los recorridos del usuario que tiene la sesion iniciada
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Task<List<ClsRecorrido>> GetRecorridosUsuarioAsync(string idUsuario)
            => _dal.ObtenerRecorridosUsuarioAsync(idUsuario);
        /// <summary>
        /// funcion que obtiene un recorrido por su id para poder mostrarselo en detalle recorrido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ClsRecorrido> GetRecorridoPorIdAsync(string id)
            => _dal.ObtenerRecorridoPorIdAsync(id);
        /// <summary>
        /// funcion que llama a la dal para crear un nuevo recorrido
        /// </summary>
        /// <param name="recorrido"></param>
        /// <returns></returns>
        public Task<bool> CrearRecorridoAsync(ClsRecorrido recorrido)
            => _dal.CrearRecorridoAsync(recorrido);
        /// <summary>
        /// funcion que llama a la dal para eliminar un recorrido por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> EliminarRecorridoAsync(string id)
            => _dal.EliminarRecorridoAsync(id);
    }
}
