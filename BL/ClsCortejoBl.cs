using DAL;
using ENT;

namespace BL
{
    public class ClsCortejoBl
    {
        private ClsCortejoDal dal = new();

        public Task<List<ClsCortejo>> GetCortejosUsuarioAsync(string idUsuario)
            => dal.ObtenerCortejosUsuarioAsync(idUsuario);

        public Task<ClsCortejo> GetCortejoPorIdAsync(string id)
            => dal.ObtenerCortejoPorIdAsync(id);

        public Task<bool> CrearCortejoAsync(ClsCortejo c)
            => dal.CrearCortejoAsync(c);

        public Task<bool> EditarCortejoAsync(ClsCortejo c)
            => dal.ActualizarCortejoAsync(c);

        public Task<bool> EliminarCortejoAsync(string id)
            => dal.EliminarCortejoAsync(id);
    }
}









