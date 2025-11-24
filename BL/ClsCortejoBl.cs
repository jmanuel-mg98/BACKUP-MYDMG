using DAL;
using ENT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public class ClsCortejoBl
    {
        private readonly ClsCortejoDal _dal = new ClsCortejoDal();

        public Task<bool> CrearCortejoAsync(ClsCortejo cortejo) => _dal.CrearCortejoAsync(cortejo);

        public Task<List<ClsCortejo>> ObtenerCortejosUsuarioAsync(string userId) => _dal.ObtenerCortejosUsuarioAsync(userId);
    }
}




