namespace bd_boga_sql_server
{
    partial class VentanaRegistros
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewRegistros = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegistros)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRegistros
            // 
            this.dataGridViewRegistros.AllowUserToAddRows = false;
            this.dataGridViewRegistros.AllowUserToDeleteRows = false;
            this.dataGridViewRegistros.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRegistros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRegistros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRegistros.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRegistros.Name = "dataGridViewRegistros";
            this.dataGridViewRegistros.ReadOnly = true;
            this.dataGridViewRegistros.RowHeadersVisible = false;
            this.dataGridViewRegistros.Size = new System.Drawing.Size(472, 190);
            this.dataGridViewRegistros.TabIndex = 0;
            // 
            // VentanaRegistros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 190);
            this.Controls.Add(this.dataGridViewRegistros);
            this.Name = "VentanaRegistros";
            this.Text = "VentanaRegistros";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegistros)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRegistros;
    }
}