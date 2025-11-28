using DAL;
using ENT;

namespace BL
{
    public class ClsCortejoBl
    {
        private ClsCortejoDal dal = new();
        /// <summary>
        /// funcion que llama a la dal para obtener los cortejos del usuario que tiene la sesion iniciada
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Task<List<ClsCortejo>> GetCortejosUsuarioAsync(string idUsuario)
            => dal.ObtenerCortejosUsuarioAsync(idUsuario);
        /// <summary>
        /// funcion que obtiene un cortejo por su id para poder mostrarselo en detalle cortejo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ClsCortejo> GetCortejoPorIdAsync(string id)
            => dal.ObtenerCortejoPorIdAsync(id);
        /// <summary>
        /// funcion que llama a la dal para crear un nuevo cortejo
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Task<bool> CrearCortejoAsync(ClsCortejo c)
            => dal.CrearCortejoAsync(c);
        /// <summary>
        /// funcion que llama a la dal para editar un cortejo
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Task<bool> EditarCortejoAsync(ClsCortejo c)
            => dal.ActualizarCortejoAsync(c);
        /// <summary>
        /// funcion que llama a la dal para eliminar un cortejo por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> EliminarCortejoAsync(string id)
            => dal.EliminarCortejoAsync(id);
    }
}









