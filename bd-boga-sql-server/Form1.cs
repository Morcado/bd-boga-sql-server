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
        private SqlConnection connectionSQL = new SqlConnection("Server=DESKTOP-CDSPVKR\\SQLEXPRESS;Database=TallerDeConfeccion;Trusted_connection=true");

        Tabla[] tablas =  new Tabla[11];
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

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {
            ShowTable(comboBox1.SelectedIndex);
            label1.Text = comboBox1.Text;
        }

        private void ShowTable(int index) {
            dtTable.Columns.Clear();
            int i = 0;
            string query = String.Format("SELECT * FROM Taller.{0}", tablas[index].TableName);
             // crea un adaptador que va a obtener los datos por medio de la conección
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionSQL);
            

            adapter.Fill(tablas[index]);
            dtTable.DataSource = tablas[index];
            dtRow.Columns.Clear();
            if (tablas[index].PK) {
                i++;
            }

            for (; i < tablas[index].NomVariables.Length; i++) {
                dtRow.Columns.Add(tablas[index].Columns[i].ColumnName, tablas[index].Columns[i].ColumnName);
            }

            dtRow.Rows.Add();
        }

        private void buttonAgregar_Click(object sender, EventArgs e) {
            InsertData(comboBox1.SelectedIndex);
        }

        public void InsertData(int index) {
            // Obtiene el autor en el combo
            // la secuencia para insertar en la vase de datos, se usan los parametros @Nom, @Nac para

            try {
                List<string> values = GetRowValues();
                SqlCommand sqlCommand = new SqlCommand(tablas[index].InsertQuery, connectionSQL);
                int i = 0;

                if (tablas[index].PK) {
                    i++;
                }
                for (int j = 0; i < tablas[index].NomVariables.Length; i++, j++) {
                    sqlCommand.Parameters.AddWithValue(tablas[index].NomVariables[i], values[j]);
                }

                sqlCommand.ExecuteNonQuery();
                ShowTable(index);
                // Actualiza la tabla

            } 
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }

           
        }

        private List<string> GetRowValues() {
            List<string> valores = new List<string>();
            for (int i = 0; i < dtRow.Rows[0].Cells.Count; i++) {
                valores.Add(dtRow.Rows[0].Cells[i].Value.ToString());
            }

            return valores;
        }

        private void buttonModificar_Click(object sender, EventArgs e) {

        }

        private void buttonEliminar_Click(object sender, EventArgs e) {
            DeleteData(comboBox1.SelectedIndex);
        }

        private void DeleteData(int index) {

            try {
                int clave = int.Parse(dtTable[0, dtTable.CurrentCell.RowIndex].Value.ToString());
                SqlCommand sqlCommand = new SqlCommand(tablas[index].DeleteQuery, connectionSQL);
                sqlCommand.Parameters.AddWithValue(tablas[index].NomVariables[0], clave);

                // Agrega los valores al los parámetros del comando sql y lo ejecuta

                sqlCommand.ExecuteNonQuery();
                // Actualiza la tabla
                ShowTable(index);

            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

        }
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

    }
}