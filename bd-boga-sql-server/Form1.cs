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

namespace bd_boga_sql_server {
    public partial class Form1 : Form {
        private SqlConnection connectionSQL;
        Tabla[] tablas =  new Tabla[11];
        public Form1() {
            InitializeComponent();
            tablas[0] = new TipoTrabajo("TipoTrabajo");
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {
            MuestraTabla(comboBox1.SelectedIndex);
        }

        private void MuestraTabla(int index) {

            DataTable dt = new DataTable();

            if (tablas[index] != null) {
                foreach (var attribute in tablas[index].Atributos) {
                    dt.Columns.Add(new DataColumn(attribute.Nombre));
                }
            }

            string query = String.Format("SELECT * FROM {0}", tablas[0].Nombre);
            // crea un adaptador que va a obtener los datos por medio de la conección
            //SqlDataAdapter adapter = new SqlDataAdapter(query, connectionSQL);
            //adapter.Fill(dt);

            dtTabla.DataSource = dt;
            dtTupla.DataSource = dt;
        }

        private void buttonAgregar_Click(object sender, EventArgs e) {
            InsertaDato(comboBox1.SelectedIndex);
        }

        public void InsertaDato(int index) {
            // Obtiene el autor en el combo
            // la secuencia para insertar en la vase de datos, se usan los parametros @Nom, @Nac para

            try {
                SqlCommand sqlCommand = new SqlCommand(tablas[index].insertQuery, connectionSQL);
                sqlCommand.ExecuteNonQuery();
                // Actualiza la tabla

                foreach (var variable in tablas[index].Variables) {
                    sqlCommand.Parameters.AddWithValue(variable, "poner valor aqui");
                }


            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }

            MuestraTabla(index);
        }

        private void buttonModificar_Click(object sender, EventArgs e) {

        }

        private void buttonEliminar_Click(object sender, EventArgs e) {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

        }
    }
}
