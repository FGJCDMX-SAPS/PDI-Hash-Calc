namespace PDI_Hash_Calc
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxAlgoritmos = new System.Windows.Forms.GroupBox();
            this.checkBoxSHA512 = new System.Windows.Forms.CheckBox();
            this.checkBoxSHA256 = new System.Windows.Forms.CheckBox();
            this.checkBoxSHA1 = new System.Windows.Forms.CheckBox();
            this.checkBoxMD5 = new System.Windows.Forms.CheckBox();
            this.gBOpciones = new System.Windows.Forms.GroupBox();
            this.cBReporte = new System.Windows.Forms.CheckBox();
            this.cBRecursivo = new System.Windows.Forms.CheckBox();
            this.gBInFiles = new System.Windows.Forms.GroupBox();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.bCarpeta = new System.Windows.Forms.Button();
            this.bArchivo = new System.Windows.Forms.Button();
            this.bComenzar = new System.Windows.Forms.Button();
            this.bCancelar = new System.Windows.Forms.Button();
            this.bLimpiar = new System.Windows.Forms.Button();
            this.pbarProgresoArchivos = new System.Windows.Forms.ProgressBar();
            this.labelProgresoArchivos = new System.Windows.Forms.Label();
            this.groupBoxAlgoritmos.SuspendLayout();
            this.gBOpciones.SuspendLayout();
            this.gBInFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxAlgoritmos
            // 
            this.groupBoxAlgoritmos.Controls.Add(this.checkBoxSHA512);
            this.groupBoxAlgoritmos.Controls.Add(this.checkBoxSHA256);
            this.groupBoxAlgoritmos.Controls.Add(this.checkBoxSHA1);
            this.groupBoxAlgoritmos.Controls.Add(this.checkBoxMD5);
            this.groupBoxAlgoritmos.Location = new System.Drawing.Point(11, 11);
            this.groupBoxAlgoritmos.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxAlgoritmos.Name = "groupBoxAlgoritmos";
            this.groupBoxAlgoritmos.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxAlgoritmos.Size = new System.Drawing.Size(117, 94);
            this.groupBoxAlgoritmos.TabIndex = 3;
            this.groupBoxAlgoritmos.TabStop = false;
            this.groupBoxAlgoritmos.Text = "Algoritmos";
            // 
            // checkBoxSHA512
            // 
            this.checkBoxSHA512.Location = new System.Drawing.Point(4, 75);
            this.checkBoxSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSHA512.Name = "checkBoxSHA512";
            this.checkBoxSHA512.Size = new System.Drawing.Size(69, 16);
            this.checkBoxSHA512.TabIndex = 3;
            this.checkBoxSHA512.Text = "SHA-512";
            this.checkBoxSHA512.UseVisualStyleBackColor = true;
            // 
            // checkBoxSHA256
            // 
            this.checkBoxSHA256.Location = new System.Drawing.Point(4, 55);
            this.checkBoxSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSHA256.Name = "checkBoxSHA256";
            this.checkBoxSHA256.Size = new System.Drawing.Size(69, 16);
            this.checkBoxSHA256.TabIndex = 2;
            this.checkBoxSHA256.Text = "SHA-256";
            this.checkBoxSHA256.UseVisualStyleBackColor = true;
            // 
            // checkBoxSHA1
            // 
            this.checkBoxSHA1.Checked = true;
            this.checkBoxSHA1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSHA1.Location = new System.Drawing.Point(4, 36);
            this.checkBoxSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSHA1.Name = "checkBoxSHA1";
            this.checkBoxSHA1.Size = new System.Drawing.Size(69, 16);
            this.checkBoxSHA1.TabIndex = 1;
            this.checkBoxSHA1.Text = "SHA-1";
            this.checkBoxSHA1.UseVisualStyleBackColor = true;
            // 
            // checkBoxMD5
            // 
            this.checkBoxMD5.Checked = true;
            this.checkBoxMD5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMD5.Location = new System.Drawing.Point(4, 16);
            this.checkBoxMD5.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxMD5.Name = "checkBoxMD5";
            this.checkBoxMD5.Size = new System.Drawing.Size(69, 16);
            this.checkBoxMD5.TabIndex = 0;
            this.checkBoxMD5.Text = "MD5";
            this.checkBoxMD5.UseVisualStyleBackColor = true;
            // 
            // gBOpciones
            // 
            this.gBOpciones.Controls.Add(this.cBReporte);
            this.gBOpciones.Controls.Add(this.cBRecursivo);
            this.gBOpciones.Location = new System.Drawing.Point(133, 12);
            this.gBOpciones.Name = "gBOpciones";
            this.gBOpciones.Size = new System.Drawing.Size(145, 93);
            this.gBOpciones.TabIndex = 4;
            this.gBOpciones.TabStop = false;
            this.gBOpciones.Text = "Opciones";
            // 
            // cBReporte
            // 
            this.cBReporte.AutoSize = true;
            this.cBReporte.Location = new System.Drawing.Point(6, 34);
            this.cBReporte.Name = "cBReporte";
            this.cBReporte.Size = new System.Drawing.Size(118, 17);
            this.cBReporte.TabIndex = 1;
            this.cBReporte.Text = "Archivo de Reporte";
            this.cBReporte.UseVisualStyleBackColor = true;
            // 
            // cBRecursivo
            // 
            this.cBRecursivo.AutoSize = true;
            this.cBRecursivo.Checked = true;
            this.cBRecursivo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBRecursivo.Location = new System.Drawing.Point(6, 15);
            this.cBRecursivo.Name = "cBRecursivo";
            this.cBRecursivo.Size = new System.Drawing.Size(93, 17);
            this.cBRecursivo.TabIndex = 0;
            this.cBRecursivo.Text = "Subdirectorios";
            this.cBRecursivo.UseVisualStyleBackColor = true;
            // 
            // gBInFiles
            // 
            this.gBInFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBInFiles.Controls.Add(this.dgvFiles);
            this.gBInFiles.Location = new System.Drawing.Point(11, 111);
            this.gBInFiles.Name = "gBInFiles";
            this.gBInFiles.Size = new System.Drawing.Size(773, 319);
            this.gBInFiles.TabIndex = 5;
            this.gBInFiles.TabStop = false;
            this.gBInFiles.Text = "Archivos para Procesar";
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowDrop = true;
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.Location = new System.Drawing.Point(3, 16);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.Size = new System.Drawing.Size(767, 300);
            this.dgvFiles.TabIndex = 0;
            this.dgvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvFiles_DragDrop);
            this.dgvFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvFiles_DragEnter);
            // 
            // bCarpeta
            // 
            this.bCarpeta.Location = new System.Drawing.Point(436, 12);
            this.bCarpeta.Name = "bCarpeta";
            this.bCarpeta.Size = new System.Drawing.Size(95, 23);
            this.bCarpeta.TabIndex = 6;
            this.bCarpeta.Text = "Abrir Carpeta";
            this.bCarpeta.UseVisualStyleBackColor = true;
            this.bCarpeta.Visible = false;
            // 
            // bArchivo
            // 
            this.bArchivo.Location = new System.Drawing.Point(537, 12);
            this.bArchivo.Name = "bArchivo";
            this.bArchivo.Size = new System.Drawing.Size(95, 23);
            this.bArchivo.TabIndex = 7;
            this.bArchivo.Text = "Abrir Archivo";
            this.bArchivo.UseVisualStyleBackColor = true;
            this.bArchivo.Visible = false;
            // 
            // bComenzar
            // 
            this.bComenzar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bComenzar.Location = new System.Drawing.Point(628, 82);
            this.bComenzar.Name = "bComenzar";
            this.bComenzar.Size = new System.Drawing.Size(75, 23);
            this.bComenzar.TabIndex = 8;
            this.bComenzar.Text = "Comenzar";
            this.bComenzar.UseVisualStyleBackColor = true;
            this.bComenzar.Click += new System.EventHandler(this.bComenzar_Click);
            // 
            // bCancelar
            // 
            this.bCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancelar.Enabled = false;
            this.bCancelar.Location = new System.Drawing.Point(709, 82);
            this.bCancelar.Name = "bCancelar";
            this.bCancelar.Size = new System.Drawing.Size(75, 23);
            this.bCancelar.TabIndex = 9;
            this.bCancelar.Text = "Cancelar";
            this.bCancelar.UseVisualStyleBackColor = true;
            this.bCancelar.Click += new System.EventHandler(this.bCancelar_Click);
            // 
            // bLimpiar
            // 
            this.bLimpiar.Location = new System.Drawing.Point(284, 82);
            this.bLimpiar.Name = "bLimpiar";
            this.bLimpiar.Size = new System.Drawing.Size(95, 23);
            this.bLimpiar.TabIndex = 10;
            this.bLimpiar.Text = "Limpiar Lista";
            this.bLimpiar.UseVisualStyleBackColor = true;
            this.bLimpiar.Click += new System.EventHandler(this.bLimpiar_Click);
            // 
            // pbarProgresoArchivos
            // 
            this.pbarProgresoArchivos.Location = new System.Drawing.Point(385, 82);
            this.pbarProgresoArchivos.Name = "pbarProgresoArchivos";
            this.pbarProgresoArchivos.Size = new System.Drawing.Size(222, 23);
            this.pbarProgresoArchivos.TabIndex = 11;
            // 
            // labelProgresoArchivos
            // 
            this.labelProgresoArchivos.AutoSize = true;
            this.labelProgresoArchivos.Location = new System.Drawing.Point(382, 66);
            this.labelProgresoArchivos.Name = "labelProgresoArchivos";
            this.labelProgresoArchivos.Size = new System.Drawing.Size(0, 13);
            this.labelProgresoArchivos.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 442);
            this.Controls.Add(this.labelProgresoArchivos);
            this.Controls.Add(this.pbarProgresoArchivos);
            this.Controls.Add(this.bLimpiar);
            this.Controls.Add(this.bCancelar);
            this.Controls.Add(this.bComenzar);
            this.Controls.Add(this.bArchivo);
            this.Controls.Add(this.bCarpeta);
            this.Controls.Add(this.gBInFiles);
            this.Controls.Add(this.gBOpciones);
            this.Controls.Add(this.groupBoxAlgoritmos);
            this.MinimumSize = new System.Drawing.Size(812, 481);
            this.Name = "Form1";
            this.Text = "FGJCDMX - Calculadora de Hash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxAlgoritmos.ResumeLayout(false);
            this.gBOpciones.ResumeLayout(false);
            this.gBOpciones.PerformLayout();
            this.gBInFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAlgoritmos;
        private System.Windows.Forms.CheckBox checkBoxSHA512;
        private System.Windows.Forms.CheckBox checkBoxSHA256;
        private System.Windows.Forms.CheckBox checkBoxSHA1;
        private System.Windows.Forms.CheckBox checkBoxMD5;
        private System.Windows.Forms.GroupBox gBOpciones;
        private System.Windows.Forms.CheckBox cBReporte;
        private System.Windows.Forms.CheckBox cBRecursivo;
        private System.Windows.Forms.GroupBox gBInFiles;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.Button bCarpeta;
        private System.Windows.Forms.Button bArchivo;
        private System.Windows.Forms.Button bComenzar;
        private System.Windows.Forms.Button bCancelar;
        private System.Windows.Forms.Button bLimpiar;
        private System.Windows.Forms.ProgressBar pbarProgresoArchivos;
        private System.Windows.Forms.Label labelProgresoArchivos;
    }
}

