using DAL;
using ENT;

namespace BL
{
    public class ClsRecorridoBl
    {
        private readonly ClsRecorridoDal _dal = new();

        public Task<List<ClsRecorrido>> GetRecorridosUsuarioAsync(string idUsuario)
            => _dal.ObtenerRecorridosUsuarioAsync(idUsuario);

        public Task<ClsRecorrido> GetRecorridoPorIdAsync(string id)
            => _dal.ObtenerRecorridoPorIdAsync(id);

        public Task<bool> CrearRecorridoAsync(ClsRecorrido recorrido)
            => _dal.CrearRecorridoAsync(recorrido);

        public Task<bool> EditarRecorridoAsync(ClsRecorrido recorrido)
            => _dal.ActualizarRecorridoAsync(recorrido);

        public Task<bool> EliminarRecorridoAsync(string id)
            => _dal.EliminarRecorridoAsync(id);
    }
}
