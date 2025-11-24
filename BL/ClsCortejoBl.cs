using DAL;
using ENT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public class ClsCortejoBl
    {
        private readonly ClsCortejoDal _dal;

        public ClsCortejoBl()
        {
            _dal = new ClsCortejoDal();
        }

        // Crear Cortejo
        public async Task<bool> CrearCortejoAsync(ClsCortejo c)
        {
            return await _dal.CrearCortejoAsync(c);
        }

        // Obtener lista de cortejos del usuario actual
        public async Task<List<ClsCortejo>> ObtenerCortejosUsuarioActualAsync()
        {
            return await _dal.ObtenerCortejosUsuarioActualAsync();
        }

        // Obtener cortejo por Id
        public async Task<ClsCortejo> ObtenerCortejoPorIdAsync(string id)
        {
            return await _dal.ObtenerCortejoPorIdAsync(id);
        }

        // Actualizar cortejo
        public async Task<bool> ActualizarCortejoAsync(ClsCortejo c)
        {
            return await _dal.ActualizarCortejoAsync(c);
        }

        // Eliminar cortejo
        public async Task<bool> EliminarCortejoAsync(string id)
        {
            return await _dal.EliminarCortejoAsync(id);
        }
    }
}









