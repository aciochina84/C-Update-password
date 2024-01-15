using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    internal class Articolo
    {
        public Int32 Id { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public Int32 Giacenza { get; set; }
        public Int32 AliquotaIva { get; set; }
        public Int32 Prezzo { get; set;}
        public Articolo() { }
    }
}
