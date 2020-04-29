using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bd_boga_sql_server
{
    public partial class VentanaRegistros : Form
    {
        public VentanaRegistros( SqlConnection conexion, string query )
        {
            InitializeComponent();
            SqlDataAdapter adapter = new SqlDataAdapter( query, conexion );
            Tabla tablaQuery = new DetalleCompra();
            adapter.Fill( tablaQuery );
            dataGridViewRegistros.DataSource = tablaQuery ;
        }
    }
}
