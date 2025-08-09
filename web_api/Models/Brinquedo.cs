using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_api.Models
{
    public class Brinquedo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public int IdadeMinima { get; set; }
        public double IdadeMaxima { get; set; }

        public Brinquedo()
        {
            
        }
    }
}