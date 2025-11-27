namespace ENT
{
    public class ClsRecorrido
    {
        public string Id { get; set; }
        public string IdUsuario { get; set; }
        public string IdCortejo { get; set; }
        public string Nombre { get; set; }
        public string LugarPartida { get; set; }
        public string Horario { get; set; }
        public int DuracionRecorrido { get; set; } // En minutos
        public List<string> Itinerario { get; set; } = new();

        // Propiedades adicionales para mostrar en UI
        public string NombreCortejo { get; set; }
    }
}
