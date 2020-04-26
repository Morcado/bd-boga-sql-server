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
        public List<string> NomVariables { get ; protected set ; }

        public bool PK { get; internal set; }

        public Tabla( string nombre ) : base( nombre ) { PK = true; } //=> Variables = new List<string>();

        public void InitializeQuerys( List<string> columnas )
        {
            NomVariables = new List<string>();
            for (int i = 0; i < columnas.Count; ++i)
                NomVariables.Add(columnas[i]);
            string attrInsert = "";
            string varInsert = "";
            string attrVarModify = "";
            if (PK) {
                attrInsert = string.Join(", ", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")).Skip(1));
                varInsert = "@" + string.Join(", @", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")).Skip(1));
                attrVarModify = "@" + string.Join(", @", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")).Skip(1));
            }
            else {
                attrInsert = string.Join(", ", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")));
                varInsert = "@" + string.Join(", @", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")));
                attrVarModify = "@" + string.Join(", @", columnas.Where(x => !x.Contains("Fecha") && !x.Contains("CostoTotal") && !x.Contains("Anticipo") && !x.Contains("CostoTrabajo") && !x.Contains("CostoMaterial")));

            }
            /*for ( int i = 0 ; i < columnas.Count ; i++)
            {
                if (!columnas[i].Contains("Fecha") && !columnas[i].Contains("CostoTotal") && !columnas[i].Contains("Anticipo")) {
                    Columns.Add(new DataColumn(columnas[i]));
                    attrInsert += separacion + columnas[i];
                    varInsert += separacion + "@" + columnas[i];
                    attrVarModify += separacion + columnas[i] + "= @" + columnas[i];
                }
            }*/

            InsertQuery = string.Format( "INSERT INTO Taller.{0} ( {1} ) VALUES( {2} )", TableName, attrInsert, varInsert ) ;
            UpdateQuery = string.Format( "UPDATE Taller.{0} SET {1} WHERE {2}={3}", TableName, attrVarModify, columnas[0], "@" + NomVariables[0] );
            DeleteQuery = string.Format( "DELETE FROM Taller.{0} WHERE {1}={2}", TableName, columnas[0], "@" + NomVariables[0] );
        }

        public void ClearData() {
            Rows.Clear();
        }
    }
}
