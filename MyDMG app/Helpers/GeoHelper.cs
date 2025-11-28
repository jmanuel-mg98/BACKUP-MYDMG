public static class GeoHelper
{
    private const double R = 6371000; // Radio terrestre en metros

    /// <summary>
    /// Calcula un punto desplazado desde una lat/lng una distancia dada en una dirección.
    /// Bearing en grados: 0=N, 90=E, 180=S, 270=O
    /// </summary>
    public static (double lat, double lng) CalcularDestino(double latInicial, double lngInicial, double metros, double bearing)
    {
        double latRad = Deg2Rad(latInicial);
        double lngRad = Deg2Rad(lngInicial);
        double angDist = metros / R;
        double bearingRad = Deg2Rad(bearing);

        double lat2 = Math.Asin(
            Math.Sin(latRad) * Math.Cos(angDist) +
            Math.Cos(latRad) * Math.Sin(angDist) * Math.Cos(bearingRad)
        );

        double lng2 = lngRad +
            Math.Atan2(
                Math.Sin(bearingRad) * Math.Sin(angDist) * Math.Cos(latRad),
                Math.Cos(angDist) - Math.Sin(latRad) * Math.Sin(lat2)
            );

        return (Rad2Deg(lat2), Rad2Deg(lng2));
    }

    private static double Deg2Rad(double deg) => deg * Math.PI / 180.0;
    private static double Rad2Deg(double rad) => rad * 180.0 / Math.PI;
}

