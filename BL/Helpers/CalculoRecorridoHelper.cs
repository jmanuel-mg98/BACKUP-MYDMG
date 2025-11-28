namespace BL.Helpers
{
    public static class CalculoRecorridoHelper
    {
        /// <summary>
        /// Calcula los metros que debería tener un recorrido en función de la velocidad media del cortejo
        /// y la duración del recorrido en minutos.
        /// </summary>
        /// <param name="velocidadMedia">Velocidad media del cortejo en metros por minuto</param>
        /// <param name="duracionMinutos">Duración del recorrido (min)</param>
        /// <returns>Métros totales requeridos</returns>
        public static double CalcularMetrosPorDuracion(double velocidadMedia, int duracionMinutos)
        {
            if (velocidadMedia <= 0 || duracionMinutos <= 0)
                return 0;

            return velocidadMedia * duracionMinutos;
        }
    }
}

