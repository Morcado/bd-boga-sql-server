using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_boga_sql_server
{
    public class Atributo
    {
        public string Nombre { get ; private set ; }
        public string Valor { get ; set ; }

        public Atributo( string nombre )
        {
            Nombre = nombre ;
            Valor = string.Empty ;
        }
    } 
}
