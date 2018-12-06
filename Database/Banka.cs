using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Databaze
{
    public class Banka
    {
        public int idBanky { get; set; }
        public string nazev { get; set; }
        public string DIC { get; set; }
        public int ICO { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public int? deleted { get; set; }

        //public List<Klient> Klienti { get; set; }
    }
}
