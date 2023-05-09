using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Proyecto1.Models
{
    public class Lugares
    {

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public byte[] foto { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string descripcion { get; set; }
        public string direccion { get; set; }
    }
}
