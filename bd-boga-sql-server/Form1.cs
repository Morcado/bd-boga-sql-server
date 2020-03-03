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
        public Form1() {
            InitializeComponent();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {
            MuestraTabla();
        }

        private void MuestraTabla() {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Costo", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("NombreConfeccion", typeof(string)));

            dtTabla.DataSource = dt;
            dtTupla.DataSource = dt;
            // for

            // for
        }

        private void buttonAgregar_Click(object sender, EventArgs e) {
            // Obtiene el autor en el combo
            // la secuencia para insertar en la vase de datos, se usan los parametros @Nom, @Nac para
            
            try {

                    //sqlCommand.ExecuteNonQuery();
                    // Actualiza la tabla
                    
                
            } 
            catch (Exception ex) {

            }

        }

        private void buttonModificar_Click(object sender, EventArgs e) {

        }

        private void buttonEliminar_Click(object sender, EventArgs e) {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

        }
    }
}
