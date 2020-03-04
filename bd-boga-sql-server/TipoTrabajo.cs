using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_boga_sql_server
{
    public class TipoTrabajo : Tabla
    {
        public TipoTrabajo( string nombre ) : base( nombre )
        {
            Atributos.Add( new Atributo( "IdTipoTrabajo" ) );
            Atributos.Add( new Atributo( "Costo" ) );
            Atributos.Add( new Atributo( "NombreConfeccion" ) );

            insertQuery = "INSERT INTO Taller.TipoTrabajo( Costo, NombreConfeccion ) VALUES( @Costo, @NomConf )" ;
            modifyQuery = "UPDATE Taller.TipoTrabajo SET Costo=@Costo, NombreConfeccion=@NomConf WHERE IdTipoTrabajo=@Id" ;
            deleteQuery = "DELETE FROM Taller.TipoTrabajo WHERE IdTipoTrabajo=@Id" ;

            Variables.Add( "@Costo" );
            Variables.Add( "@NomConf" );
            Variables.Add( "@Id" );
        }
    }
}
