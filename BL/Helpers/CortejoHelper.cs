using ENT;

namespace BL.Helpers
{
    public static class CortejoHelper
    {
        public static double CalcularVelocidadMedia(ClsCortejo cortejo)
        {
            if (cortejo == null) return 0;

            double baseVelocidad = 15;
            double k = 2.28;

            double factorParticipantes = 1 - ((double)cortejo.NParticipantes / 2000.0);
            double factorPasos = 1 - (cortejo.CantidadPasos * 0.05);
            double factorEsPaso = cortejo.EsPaso ? 0.8 : 1.0;
            double factorBanda = cortejo.LlevaBanda ? 0.85 : 1.0;

            double velocidad = baseVelocidad * k * factorParticipantes * factorPasos * factorEsPaso * factorBanda;

            return velocidad;
        }
    }
}
