using DAL;
using ENT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public class ClsCortejoBl
    {
        private readonly ClsCortejoDal _cortejoDal;

        public ClsCortejoBl()
        {
            _cortejoDal = new ClsCortejoDal();
        }

        public async Task<bool> CrearCortejoAsync(ClsCortejo cortejo)
        {
            return await _cortejoDal.CrearCortejoAsync(cortejo);
        }

        public async Task<List<ClsCortejo>> ObtenerCortejosPorUsuarioAsync(string userId)
        {
            return await _cortejoDal.ObtenerCortejosPorUsuarioAsync(userId);
        }
    }
}





