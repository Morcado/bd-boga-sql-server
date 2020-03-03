using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_boga_sql_server
{
    public abstract class Tabla
    {
        public string Nombre { get ; }
        public List<Atributo> Atributos { get ; private set ; }
        public string insertQuery { get ; protected set ; }
        public List<string> Variables { get ; private set ; }

        public Tabla( string nombre )
        {
            Nombre = nombre ;
            Atributos = new List<Atributo>();
            Variables = new List<string>();
        }
    }
}
