using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace bd_boga_sql_server {
    public partial class Form1 : Form {
        
        #region Variables miembro
        //Modificar de acuerdo a la ruta de la base de datos.
        private readonly SqlConnection connectionSQL = new SqlConnection("Server=Luis\\SQLEXPRESS;Database=TallerBoga;Trusted_connection=true");

        //Número de tablas que existen en la base de datos.
        private readonly Tabla[] tablas =  new Tabla[11];

        //Ventana para mostrar registros en demanda.
        private VentanaRegistros ventanaRegistros ;
        #endregion

        #region Constructor
        /*El indice del combobox principal selecciona la tabla.*/
        public Form1() {
            InitializeComponent();
            tablas[0]  = new TipoTrabajo();
            tablas[1]  = new Cliente();
            tablas[2]  = new Empleado();
            tablas[3]  = new Confeccion();
            tablas[4]  = new Prenda();
            tablas[5]  = new Trabajo();
            tablas[6]  = new Material();
            tablas[7]  = new MaterialParaTrabajo();
            tablas[8]  = new Proveedor();
            tablas[9]  = new Compra();
            tablas[10] = new DetalleCompra();
            dtRow.AllowUserToAddRows = false;

            connectionSQL.Open();
        }
        #endregion

        #region Eventos de la interfaz
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {
            ShowTable(comboBox1.SelectedIndex);
            label1.Text = comboBox1.Text;
        }

        private void buttonAgregar_Click(object sender, EventArgs e) {
            InsertData(comboBox1.SelectedIndex);
        }

        private void buttonModificar_Click(object sender, EventArgs e) {
            try
            {
                UpdateData(comboBox1.SelectedIndex);
            }
            catch( Exception exc )
            {
                MessageBox.Show( exc.Message );
                throw;
            }
            
        }

        private void buttonEliminar_Click(object sender, EventArgs e) {
            DeleteData(comboBox1.SelectedIndex);
        }

        /* dtTable_CellClick( object sender, DataGridViewCellEventArgs e )
         * Cada vez que se selecciona un registro en el datagrid, se obtiene la información para
         * modificar/eliminar el registro. 
         * Si se le da click a un registro, los valores aparecerán en el datagrid de
         * "Agregar/Modificar".
         */
        private void dtTable_CellClick( object sender, DataGridViewCellEventArgs e )
        {
            if( e.RowIndex < 0 )
                return ;

            if( dtTable.Columns["Detalle Compra"] != null && e.ColumnIndex == dtTable.Columns["Detalle Compra"].Index )  {

                string idCompra = dtTable.Rows[e.RowIndex].Cells[0].Value.ToString();
                string query = @"SELECT IdDetalleCompra, IdCompra, d.IdMaterial, m.Descripcion, CostoUnitario, Cantidad, Subtotal FROM Taller.DetalleCompra AS d 
INNER JOIN Taller.Material AS m ON d.IdMaterial = m.IdMaterial WHERE IdCompra =" + idCompra ;

                if( ventanaRegistros != null )
                    ventanaRegistros.Close();

                ventanaRegistros = new VentanaRegistros( connectionSQL, query );
                ventanaRegistros.Show();
                return ;
            }

            List<string> values = GetRowValues(dtTable, e.RowIndex, true);
            dtRow.Rows.Clear();
            dtRow.Rows.Add( values.ToArray() );

            //Nombre col + Valor
            List<Tuple<string,string>> registro = new List<Tuple<string, string>>();

            for( int i = 0 ; i < dtTable.Columns.Count ; ++i )  {
                string nomColumna = dtTable.Columns[i].HeaderText ;
                string valor = dtTable.Rows[e.RowIndex].Cells[i].Value.ToString();

                var reg = new Tuple<string,string>( nomColumna, valor );
                registro.Add( reg );
            }

            for( int i = 0 ; i < dtRow.Columns.Count ; ++i )    {
                string nomCol = dtRow.Columns[i].HeaderText ;
                Tuple<string,string> valor ;

                if( ( valor = registro.Find( r => r.Item1 == nomCol ) ) != null )   {
                    Tabla tabMostrar = tablas[comboBox1.SelectedIndex];

                    if( tabMostrar.AdditionalInfoCols != null && tabMostrar.AdditionalInfoCols.Exists( c => c == i ) )
                        dtRow.Rows[0].Cells[i].Value = valor.Item2.ToString() + ": " + tabMostrar.EnumeracionInformacion.Find( en => en.Item1 == int.Parse( valor.Item2 ) ).Item2 ;

                    else
                        dtRow.Rows[0].Cells[i].Value = valor.Item2 ;
                }
            }
        }
        #endregion

        #region Conexiones con la base de datos.
        /*
         * ShowTable( int index )
         * Indexa en "tablas" la tabla seleccionada, recupera los registros de la base de datos
         * y los inserta en el datagrid. Se utiliza también para refrescar la vista de los registros.
         */
        private void ShowTable( int index )
        {
            Tabla tablaMostrar = tablas[index] ;
            string query ;
            dtTable.DataSource = null;
            dtTable.Rows.Clear();
            dtTable.Columns.Clear();

            if( !string.IsNullOrWhiteSpace( tablaMostrar.SelectQuery ) )
                query = tablaMostrar.SelectQuery ;

            else
                query = string.Format( "SELECT * FROM Taller.{0}", tablas[index].TableName );

            // crea un adaptador que va a obtener los datos por medio de la conección
            SqlDataAdapter adapter = new SqlDataAdapter( query, connectionSQL );
            tablaMostrar.Clear();
            adapter.Fill( tablaMostrar );
            dtTable.DataSource = tablaMostrar ;

            if( tablaMostrar.TableName == "Compra" && dtTable.Columns["Detalle Compra"] == null )    {
                DataGridViewButtonColumn detalleCompra = new DataGridViewButtonColumn() {
                    Name = "Detalle Compra",
                    HeaderText = "Detalle Compra",
                    UseColumnTextForButtonValue = true,
                    Text = "Detalle"
                };

                dtTable.Columns.Add( detalleCompra );
            }

            dtTable.Refresh();

            //Obtener la información adicional de la tabla.
            if( !string.IsNullOrWhiteSpace( tablaMostrar.AdditionalInfoQuery ) )
            {
                tablaMostrar.EnumeracionInformacion = new List<Tuple<long, string>>();
                using( SqlDataReader lector = new SqlCommand( tablaMostrar.AdditionalInfoQuery, connectionSQL ).ExecuteReader() ) {
                    while( lector.Read() )
                    {
                        var en = new Tuple<long, string>( lector.GetInt64( 0 ), lector.GetString( 1 ) );
                        tablaMostrar.EnumeracionInformacion.Add( en );
                    }
                }
            }

            // Fila de inserción
            dtRow.Columns.Clear();
            int i = tablaMostrar.PK ? 1 : 0;
            for( ; i < tablaMostrar.NomVariables.Count; i++ )
            {
                string columna = tablaMostrar.NomVariables[i];
                if( columna.Contains( "Id" ) )
                {
                    string comboQuery = string.Format( "SELECT * FROM Taller.{0}", columna.Substring( 2 ) );

                    List<string> columnData = new List<string>();
                    using( SqlCommand command = new SqlCommand( comboQuery, connectionSQL ) )
                    {
                        using( SqlDataReader reader = command.ExecuteReader() )
                        {
                            while( reader.Read() )
                            {
                                long id = reader.GetInt64( 0 );
                                string opcion = id.ToString();

                                if( !string.IsNullOrWhiteSpace( tablaMostrar.AdditionalInfoQuery ) )
                                {
                                    if( tablaMostrar.AdditionalInfoCols != null && tablaMostrar.AdditionalInfoCols.Exists( col => col == i-1 ) )
                                        opcion += ": " + tablaMostrar.EnumeracionInformacion.Find( e => e.Item1 == id ).Item2 ;
                                }

                                columnData.Add( opcion.ToString() );
                            }
                        }
                    }

                    var column = new DataGridViewComboBoxColumn
                    {
                        HeaderText = columna,
                        DataSource = columnData
                    };
                    dtRow.Columns.Add( column );
                }
                else
                {
                    dtRow.Columns.Add( columna, columna );
                }
            }

            dtRow.Rows.Add();
        }

        /*
         * InsertData(int index)
         * Toma los valores del datagrid "Agregar/Insertar" e inserta un nuevo registro
         * a la tabla seleccionada en el combobox.
         * Crea una conexión a la base de datos.
         */
        public void InsertData(int index) {
            // Obtiene el autor en el combo
            // la secuencia para insertar en la base de datos, se usan los parametros @Nom, @Nac para
            try {
                List<string> values = GetRowValues(dtRow, 0);
                SqlCommand sqlCommand = new SqlCommand(tablas[index].InsertQuery, connectionSQL);
                int i = tablas[index].PK ? 1 : 0;
                for (int j = 0; i < tablas[index].NomVariables.Count; i++) {
                    sqlCommand.Parameters.AddWithValue("@" + tablas[index].NomVariables[i], values[j++]);
                }

                sqlCommand.ExecuteNonQuery();

                // Actualiza la tabla
                ShowTable(index);
            } 
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /*
         * UpdateData(int tableNumber)
         * Toma la información de el registro seleccionado, y la información de el datagrid
         * "Agregar/Modificar" para actualizar un registro, generalmente por medio del Id del
         * elemento.
         */
        private void UpdateData(int tableNumber) {

            try {
                int index = int.Parse(dtTable[0, dtTable.CurrentCell.RowIndex].Value.ToString());
                SqlCommand sqlCommand = new SqlCommand(tablas[tableNumber].UpdateQuery, connectionSQL);
                sqlCommand.Parameters.AddWithValue("@" + tablas[tableNumber].NomVariables[0], index);

                List<string> values ;

                try
                {
                    values = GetRowValues(dtRow, 0);
                }
                catch( Exception )
                {
                    throw( new Exception( "Ingresa valores válidos en la primera ventana, y luego presiona \'Modificar\'" ) );
                }
                
                int i = tablas[tableNumber].PK ? 1 : 0;

                for (int j = 0; i < tablas[tableNumber].NomVariables.Count; i++) {
                    string nomvariable = tablas[tableNumber].NomVariables[i];
                    sqlCommand.Parameters.AddWithValue("@" + tablas[tableNumber].NomVariables[i], values[j++]);
                }
                sqlCommand.ExecuteNonQuery();
                ShowTable(tableNumber);
                
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /*
         * DeleteData(int tableNumber)
         * Elimina el registro seleccionado en el datagrid.
         */
        private void DeleteData(int tableNumber) {

            try {
                int clave = int.Parse(dtTable[0, dtTable.CurrentCell.RowIndex].Value.ToString());
                SqlCommand sqlCommand = new SqlCommand(tablas[tableNumber].DeleteQuery, connectionSQL);
                sqlCommand.Parameters.AddWithValue("@" + tablas[tableNumber].NomVariables[0], clave);

                // Agrega los valores al los parámetros del comando sql y lo ejecuta

                sqlCommand.ExecuteNonQuery();
                // Actualiza la tabla
                ShowTable(tableNumber);

            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /*
         * GetRowValues(DataGridView table, int index, bool hasPKkey = false)
         * Obtiene los valores del datagrid "Agregar/Eliminar" y los regresa en una lista.
         */
        private List<string> GetRowValues(DataGridView table, int index, bool hasPKkey = false) 
        {
            List<string> valores = new List<string>();

            if( index < 0 )
                return valores ;
            
            for (int i = hasPKkey ? 1 : 0; i < table.Rows[index].Cells.Count; i++) {
                try
                {
                    if( tablas[comboBox1.SelectedIndex].EnumeracionInformacion != null )    {
                        string info = table.Rows[index].Cells[i].Value.ToString();

                        if(  info.IndexOf(':') > -1 )
                            info = info.Substring( 0, info.IndexOf(':') );

                        valores.Add( info );
                    }
                            
                    else
                        valores.Add(table.Rows[index].Cells[i].Value.ToString());
                }
                catch( NullReferenceException )
                {
                    throw ;
                }
            }

            return valores;
        }

        //???
        private void enableCell(DataGridViewCell dc, bool enabled) {
            //toggle read-only state
            dc.ReadOnly = !enabled;
            if (enabled) {
                //restore cell style to the default value
                dc.Style.BackColor = dc.OwningColumn.DefaultCellStyle.BackColor;
                dc.Style.ForeColor = dc.OwningColumn.DefaultCellStyle.ForeColor;
            } else {
                //gray out the cell
                dc.Style.BackColor = Color.LightGray;
                dc.Style.ForeColor = Color.DarkGray;
            }
        }
        #endregion   
    }
}