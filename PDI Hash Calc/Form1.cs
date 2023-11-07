using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Hash_Calc
{
    public partial class Form1 : Form
    {
        bool calculando { get; set; }
        bool cancelar { get; set; }


        Task TareaAgregarArchivosLista { get; set; }
        DataTable DTListaArchivos { get; set; }

        CancellationTokenSource CTStoken { get; set; }
        //CancellationToken CTsalida { get; set; }


        public Form1()
        {
            InitializeComponent();

            CTStoken = new CancellationTokenSource();

            DTListaArchivos = new DataTable();
            //***
            //Crea las columnas para la tabla personalizada
            //***
            DataGridViewColumn colNombreArchivo = new DataGridViewColumn();
            colNombreArchivo.Name = "Nombre de Archivo";
            colNombreArchivo.CellTemplate = new DataGridViewTextBoxCell();
            colNombreArchivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colNombreArchivo);
            DTListaArchivos.Columns.Add(colNombreArchivo.Name, typeof(string));


            DataGridViewProgressColumn colProgreso = new DataGridViewProgressColumn();
            colProgreso.Name = "Progreso";
            colProgreso.HeaderText = "Progreso";
            dgvFiles.Columns.Add(colProgreso);
            DTListaArchivos.Columns.Add(colProgreso.Name, typeof(int));

            DataGridViewColumn colMD5 = new DataGridViewColumn();
            colMD5.Name = "MD5";
            colMD5.CellTemplate = new DataGridViewTextBoxCell();
            colMD5.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colMD5);
            DTListaArchivos.Columns.Add(colMD5.Name, typeof(string));

            DataGridViewColumn colSHA1 = new DataGridViewColumn();
            colSHA1.Name = "SHA1";
            colSHA1.CellTemplate = new DataGridViewTextBoxCell();
            colSHA1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA1);
            DTListaArchivos.Columns.Add(colSHA1.Name, typeof(string));

            DataGridViewColumn colSHA256 = new DataGridViewColumn();
            colSHA256.Name = "SHA256";
            colSHA256.CellTemplate = new DataGridViewTextBoxCell();
            colSHA256.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA256);
            DTListaArchivos.Columns.Add(colSHA256.Name, typeof(string));

            DataGridViewColumn colSHA512 = new DataGridViewColumn();
            colSHA512.Name = "SHA512";
            colSHA512.CellTemplate = new DataGridViewTextBoxCell();
            colSHA512.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA512);
            DTListaArchivos.Columns.Add(colSHA512.Name, typeof(string));

            calculando = false;
            cancelar = false;


        }


        private async void dgvFiles_DragDrop(object sender, DragEventArgs e)
        {
            List<string> lista_archivos = new List<string>();

            // Checa si es un archivo/carpeta
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {


                TareaAgregarArchivosLista = Task.Run(() =>
                {
                    // obten la lista de los que se arrastraron (lista original)
                    string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);

                    // Comprueba para cada elemento que tipo es,si archivo o carpeta
                    foreach (string elemento in archivos)
                    {
                        FileAttributes attr = File.GetAttributes(elemento);
                        bool esFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;

                        // Si es un folder, obten todos los elementos del mismo (con posible recursion) y los agrega a las lista
                        if (esFolder)
                        {
                            string[] sub_archivos = Directory.GetFiles(elemento, "*",
                                (cBRecursivo.Checked) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                            if (sub_archivos.Length > 0)
                            {
                                lista_archivos.AddRange(sub_archivos);
                            }
                        }
                        else
                        {
                            // si solo estaba seleccionado un archivo, lo agrega a la lista
                            lista_archivos.Add(elemento);
                        }
                    }

                });

                await TareaAgregarArchivosLista;

                dgvFiles.Rows.Clear();
                dgvFiles.Rows.Add();
                DataGridViewRow r = (DataGridViewRow)dgvFiles.Rows[0].Clone();
                dgvFiles.Rows.Clear();

                List<DataGridViewRow> listadgv_files = new List<DataGridViewRow>();


                IProgress<List<DataGridViewRow>> ProgLista = new Progress<List<DataGridViewRow>>((val) =>
                {
                    dgvFiles.Rows.AddRange(val.ToArray());
                });

                TareaAgregarArchivosLista = Task.Run(() =>
                {
                    //hace vaciado de lista en la tabla
                    for (int i = 0; i < lista_archivos.Count; i++)
                    {
                        //nombre - prog-md5-sha1-sha256-sha512
                        //object[] renglon = new object[] { archivo, 0, "", "", "", "" };
                        //dgvFiles.Rows.Add(renglon);
                        //ProgLista.Report(renglon);

                        var r_temp = (DataGridViewRow)r.Clone();
                        r_temp.Cells[0].Value = lista_archivos[i];
                        r_temp.Cells[1].Value = 0;
                        r_temp.Cells[2].Value = "";
                        r_temp.Cells[3].Value = "";
                        r_temp.Cells[4].Value = "";
                        r_temp.Cells[5].Value = "";

                        listadgv_files.Add(r_temp);
                    }

                    ProgLista.Report(listadgv_files);
                });

                await TareaAgregarArchivosLista;




            }
        }

        private void dgvFiles_DragEnter(object sender, DragEventArgs e)
        {
            // Si el mouse que arrrastra algo, pasa por el contenedor y es un archivo/folder, se copia el origen
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void bLimpiar_Click(object sender, EventArgs e)
        {
            // vaciado de lineas
            dgvFiles.Rows.Clear();
        }

        static ReaderWriterLockSlim RWlock = new ReaderWriterLockSlim();

        private async void bComenzar_Click(object sender, EventArgs e)
        {
            //indice de avance en la tabla de archivos
            //int renglon_actual = 0;
            CTStoken = new CancellationTokenSource();
            CancellationToken CTsalida = CTStoken.Token;

            Object p_lock = new Object();

            if (dgvFiles.Rows.Count <= 0)
            {
                return;
            }


            //**** 
            //Establece las (des)habilitaciones de la interface usuario para evitar errores de interaccion
            //****

            calculando = true;

            bComenzar.Enabled = false;
            gBOpciones.Enabled = false;
            groupBoxAlgoritmos.Enabled = false;
            bCarpeta.Enabled = false;
            bArchivo.Enabled = false;
            bLimpiar.Enabled = false;

            dgvFiles.AllowDrop = false;

            bCancelar.Enabled = true;


            //confirma que por lo menos algun algoritmo este seleccionado
            if (!checkBoxMD5.Checked && !checkBoxSHA1.Checked && !checkBoxSHA256.Checked && !checkBoxSHA512.Checked)
            {
                MessageBox.Show("Debe estar seleccionado por lo menos un algoritmo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //Crea un tabla temporal, para poder usarla en subprocesos
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgvFiles.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            //Copia los datos del datagridview
            foreach (DataGridViewRow row in dgvFiles.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

            // Con este reporteador, se avanza en la tabla de archivos
            IProgress<MensajeReporte> prog_select = new Progress<MensajeReporte>((val) =>
            {
                //dgvFiles.Rows[val.linea].Selected = true;
                dgvFiles.FirstDisplayedScrollingRowIndex = val.linea;

            });

            //***
            //Con los siguientes reporteadores se actualizan las celdas con los hashes calculados, asi mismo el porcentaje
            //***
            IProgress<int> barraprogreso = new Progress<int>((val) =>
            {
                pbarProgresoArchivos.Value = val;
            });
            IProgress<MensajeReporte> progressMD5 = new Progress<MensajeReporte>((s) =>
            {
                //RWlock.EnterWriteLock();
                dgvFiles.Rows[s.linea].Cells[1].Value = s.porcentaje;
                dgvFiles.Rows[s.linea].Cells[2].Value = s.mensaje;
                //RWlock.ExitWriteLock();
            });
            IProgress<MensajeReporte> progressSHA1 = new Progress<MensajeReporte>((s) =>
            {
                //RWlock.EnterWriteLock();
                dgvFiles.Rows[s.linea].Cells[1].Value = s.porcentaje;
                dgvFiles.Rows[s.linea].Cells[3].Value = s.mensaje;
                //RWlock.ExitWriteLock();
            });
            IProgress<MensajeReporte> progressSHA256 = new Progress<MensajeReporte>((s) =>
            {
                //RWlock.EnterWriteLock();
                dgvFiles.Rows[s.linea].Cells[1].Value = s.porcentaje;
                dgvFiles.Rows[s.linea].Cells[4].Value = s.mensaje;
                //RWlock.ExitWriteLock();
            });
            IProgress<MensajeReporte> progressSHA512 = new Progress<MensajeReporte>((s) =>
            {
                //RWlock.ExitWriteLock();
                dgvFiles.Rows[s.linea].Cells[1].Value = s.porcentaje;
                dgvFiles.Rows[s.linea].Cells[5].Value = s.mensaje;
                //RWlock.ExitWriteLock();
            });


            try
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (CTsalida.IsCancellationRequested)
                        {
                            barraprogreso.Report(0);
                            return;
                        }

                        int prog = (100 * i) / dt.Rows.Count;
                        barraprogreso.Report(prog);

                        if (checkBoxMD5.Checked)
                        {
                            GeneradorHash.Calcular(dt.Rows[(int)i][0].ToString(), GeneradorHash.AlgoritmoHash.MD5, progressMD5, i, CTsalida);
                        }
                        if (checkBoxSHA1.Checked)
                        {
                            GeneradorHash.Calcular(dt.Rows[(int)i][0].ToString(), GeneradorHash.AlgoritmoHash.SHA1, progressSHA1, i, CTsalida);
                        }
                        if (checkBoxSHA256.Checked)
                        {
                            GeneradorHash.Calcular(dt.Rows[(int)i][0].ToString(), GeneradorHash.AlgoritmoHash.SHA256, progressSHA256, i, CTsalida);
                        }
                        if (checkBoxSHA512.Checked)
                        {
                            GeneradorHash.Calcular(dt.Rows[(int)i][0].ToString(), GeneradorHash.AlgoritmoHash.SHA512, progressSHA512, i, CTsalida);
                        }
                    }//);
                    barraprogreso.Report(100);

                }, CTsalida);

            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    //Console.WriteLine(ex.Message);
                    return true;
                });
            }


            //***
            //Establece las (des)habilitaciones de la interface usuario para evitar errores de interaccion
            //***

            bComenzar.Enabled = true;
            gBOpciones.Enabled = true;
            groupBoxAlgoritmos.Enabled = true;
            bCarpeta.Enabled = true;
            bArchivo.Enabled = true;
            bLimpiar.Enabled = true;

            dgvFiles.AllowDrop = true;

            bCancelar.Enabled = false;

            calculando = false;
            cancelar = false;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (calculando)
            {
                e.Cancel = true;
            }
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
            CTStoken.Cancel();
        }
    }

    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }
    }

    public class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public DataGridViewProgressCell()
        {
            this.ValueType = typeof(int);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                int progressVal = (int)(value != null ? value : 0);
                float percentage = ((float)progressVal / 100.0f); // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
                Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
                Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
                // Draws the cell grid
                base.Paint(g, clipBounds, cellBounds,
                 rowIndex, cellState, value, formattedValue, errorText,
                 cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
                if (percentage > 0.0)
                {
                    // Draw the progress bar and the text
                    g.FillRectangle(new SolidBrush(Color.FromArgb(203, 235, 108)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
                    g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + (cellBounds.Width / 2) - 5, cellBounds.Y + 2);

                }
                else
                {
                    // draw the text
                    if (this.DataGridView.CurrentRow.Index == rowIndex)
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 2);
                    else
                        g.DrawString(progressVal.ToString() + "%", cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
                }
            }
            catch (Exception e) { }

        }
    }

    public class MensajeReporte
    {
        public int linea { get; set; }
        public int porcentaje { get; set; }
        public string mensaje { get; set; }
    }
}
