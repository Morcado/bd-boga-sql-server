using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bd_boga_sql_server
{
    public abstract class Tabla : DataTable
    {
        public string InsertQuery { get ; protected set ; }
        public string DeleteQuery { get ; protected set ; }
        public string UpdateQuery { get ; protected set ; }
        public string[] NomVariables { get ; protected set ; }

        public bool PK { get; internal set; }

        public Tabla( string nombre ) : base( nombre ) { PK = true; } //=> Variables = new List<string>();

        public void InitializeQuerys( string[] columnas )
        {
            NomVariables = new string[columnas.Length] ;

            for( int i = 0 ; i < columnas.Length ; ++i )
                NomVariables[i] = "@" + columnas[i] ;

            string attrInsert = columnas[1] ;
            string varInsert = NomVariables[1] ;
            string attrVarModify = columnas[1] + '=' + NomVariables[1] ;
            string separacion = ", " ;

            Columns.Add( new DataColumn( columnas[0] ) );
            Columns.Add( new DataColumn( columnas[1] ) );
            for( int i = 2 ; i < columnas.Length ; ++i )
            {
                Columns.Add( new DataColumn( columnas[i] ) );
                attrInsert += separacion + Columns[i] ;
                varInsert += separacion + NomVariables[i] ;
                attrVarModify += separacion + Columns[i] + '=' + NomVariables[i] ;
            }

            InsertQuery = string.Format( "INSERT INTO Taller.{0} ( {1} ) VALUES( {2} )", TableName, attrInsert, varInsert ) ;
            UpdateQuery = string.Format( "UPDATE Taller.{0} SET {1} WHERE {2}={3}", TableName, attrVarModify, columnas[0], NomVariables[0] );
            DeleteQuery = string.Format( "DELETE FROM Taller.{0} WHERE {1}={2}", TableName, Columns[0].ColumnName, NomVariables[0] );
        }

        public void ClearData() {
            Rows.Clear();
        }
    }
}
