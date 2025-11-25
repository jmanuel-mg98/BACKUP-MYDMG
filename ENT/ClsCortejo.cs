using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class ClsCortejo
    {
        public string Id { get; set; }
        public string NombreCortejo { get; set; }
        public int NParticipantes { get; set; }
        public bool EsPaso { get; set; }
        public int CantidadPasos { get; set; }
        public bool LlevaBanda { get; set; }
        public double VelocidadMedia { get; set; }
        public string IdUsuario { get; set; }
    }
}

