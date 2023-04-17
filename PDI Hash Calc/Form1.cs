using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDI_Hash_Calc
{
    public partial class Form1 : Form
    {
        bool calculando { get; set; }
        bool cancelar { get; set; }


        public Form1()
        {
            InitializeComponent();

            //***
            //Crea las columnas para la tabla personalizada
            //***
            DataGridViewColumn colNombreArchivo = new DataGridViewColumn();
            colNombreArchivo.Name = "Nombre de Archivo";
            colNombreArchivo.CellTemplate = new DataGridViewTextBoxCell();
            colNombreArchivo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colNombreArchivo);

            DataGridViewProgressColumn colProgreso = new DataGridViewProgressColumn();
            colProgreso.Name = "Progreso";
            colProgreso.HeaderText = "Progreso";
            dgvFiles.Columns.Add(colProgreso);

            DataGridViewColumn colMD5 = new DataGridViewColumn();
            colMD5.Name = "MD5";
            colMD5.CellTemplate = new DataGridViewTextBoxCell();
            colMD5.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colMD5);

            DataGridViewColumn colSHA1 = new DataGridViewColumn();
            colSHA1.Name = "SHA1";
            colSHA1.CellTemplate = new DataGridViewTextBoxCell();
            colSHA1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA1);

            DataGridViewColumn colSHA256 = new DataGridViewColumn();
            colSHA256.Name = "SHA256";
            colSHA256.CellTemplate = new DataGridViewTextBoxCell();
            colSHA256.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA256);

            DataGridViewColumn colSHA512 = new DataGridViewColumn();
            colSHA512.Name = "SHA512";
            colSHA512.CellTemplate = new DataGridViewTextBoxCell();
            colSHA512.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvFiles.Columns.Add(colSHA512);

            calculando = false;
            cancelar = false;


        }

        private void dgvFiles_DragDrop(object sender, DragEventArgs e)
        {
            List<string> lista_archivos = new List<string>();

            // Checa si es un archivo/carpeta
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {          
                // obten la lista de los que se arrastraron (lista original)
                string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);
                                
                // Comprueba para cada elemento que tipo es,si archivo o carpeta
                foreach (string elemento in archivos)
                {
                    FileAttributes attr = File.GetAttributes(elemento);
                    bool esFolder = (attr & FileAttributes.Directory) == FileAttributes.Directory;

                    // Si es un folder, obten todos los elementos del mismo (con posible recursion) y los agrega a las lista
                    if(esFolder)
                    {
                        string[] sub_archivos = Directory.GetFiles(elemento, "*", 
                            (cBRecursivo.Checked)? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                        if(sub_archivos.Length > 0)
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

                //hace vaciado de lista en la tabla
                foreach (string archivo in lista_archivos)
                {
                    //nombre - prog-md5-sha1-sha256-sha512
                    object[] renglon = new object[] { archivo, 0, "", "", "", "" };
                    dgvFiles.Rows.Add(renglon);
                }



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

        private async void bComenzar_Click(object sender, EventArgs e)
        {
            //indice de avance en la tabla de archivos
            int renglon_actual = 0;

            if (dgvFiles.Rows.Count <= 0)
            {
                return;
            }


            //**** 
            //Establece las (des)habilitaciones de la interface usuario para evitar errores de interaccion
            //*****

            calculando = true;

            bComenzar.Enabled = false;
            gBOpciones.Enabled = false;
            groupBoxAlgoritmos.Enabled = false;
            bCarpeta.Enabled = false;
            bArchivo.Enabled = false;
            bLimpiar.Enabled = false;


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
            IProgress<MensajeReporte> barraprogreso = new Progress<MensajeReporte>((val) =>
            {
                dgvFiles.Rows[val.linea].Cells[1].Value = val.porcentaje;
            });
            IProgress<MensajeReporte> progressMD5 = new Progress<MensajeReporte>((s) =>
            {
                dgvFiles.Rows[s.linea].Cells[2].Value = s.mensaje;
            });
            IProgress<MensajeReporte> progressSHA1 = new Progress<MensajeReporte>((s) =>
            {
                dgvFiles.Rows[s.linea].Cells[3].Value = s.mensaje;
            });
            IProgress<MensajeReporte> progressSHA256 = new Progress<MensajeReporte>((s) =>
            {
                dgvFiles.Rows[s.linea].Cells[4].Value = s.mensaje;
            });
            IProgress<MensajeReporte> progressSHA512 = new Progress<MensajeReporte>((s) =>
            {
                dgvFiles.Rows[s.linea].Cells[5].Value = s.mensaje;
            });


            await Task.Run(() => 
            {
                foreach(DataRow r in dt.Rows) 
                {
                    // strean que leerael archivo
                    Stream f_stream = new MemoryStream();
                    long bytes_leidos;
                    // variables de calculo de hash
                    HashAlgorithm hash_MD5, hash_SHA1, hash_SHA256, hash_SHA512;
                    hash_MD5 = MD5.Create();
                    hash_SHA1 = SHA1.Create();
                    hash_SHA256 = SHA256.Create();
                    hash_SHA512 = SHA512.Create();

                    // reporteador para seleccionar la linea
                    prog_select.Report(new MensajeReporte { linea = renglon_actual});
                    
                    // intenta abrir el archivo
                    try
                    {
                        f_stream = (Stream)File.Open(r[0].ToString(), FileMode.Open);
                    }
                    catch(Exception ex) 
                    {
                        // Si esta ocupado por otro proceso o hay algun error de lectura, reporta y continua con el siguiente archivo
                        var reporte_msg = new MensajeReporte
                        {
                            linea = renglon_actual,
                            mensaje = "Error: " + ex.Message
                        };
                        if(checkBoxMD5.Checked)
                            progressMD5.Report(reporte_msg);
                        if(checkBoxSHA1.Checked)   
                            progressSHA1.Report(reporte_msg);
                        if(checkBoxSHA256.Checked)
                            progressSHA256.Report(reporte_msg);
                        if(checkBoxSHA512.Checked)
                            progressSHA512.Report(reporte_msg);

                        renglon_actual++;
                        continue;
                    }
                    finally
                    {

                    }

                    //tamaño del buffer de lectura
                    int bufferSize = 4096;

                    // variables de lectura (buffer intermedios)
                    byte[] readAheadBuffer, buffer;
                    int readAheadBytesRead, bytesRead;
                    long size, totalBytesRead = 0;

                    bytes_leidos = 0;
                    size = f_stream.Length;

                    // inicializa y lee el archivo (buffersize cantidad de bytes)
                    readAheadBuffer = new byte[bufferSize];
                    readAheadBytesRead = f_stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                    totalBytesRead += readAheadBytesRead;
                    bytes_leidos = totalBytesRead;

                    do
                    {
                        //***
                        //En este loop se lee un fragmenteo de archivo y se integra al calculo de hashes
                        //***

                        bytesRead = readAheadBytesRead;
                        buffer = readAheadBuffer;

                        readAheadBuffer = new byte[bufferSize];
                        readAheadBytesRead = f_stream.Read(readAheadBuffer, 0, readAheadBuffer.Length);

                        totalBytesRead += readAheadBytesRead;
                        bytes_leidos = totalBytesRead;

                        // si ya no hay mas que leer, finaliza el calculo de hash
                        if (readAheadBytesRead == 0)
                        {
                            if (checkBoxMD5.Checked)
                                hash_MD5.TransformFinalBlock(buffer, 0, bytesRead);
                            if (checkBoxSHA1.Checked)
                                hash_SHA1.TransformFinalBlock(buffer, 0, bytesRead);
                            if (checkBoxSHA256.Checked)
                                hash_SHA256.TransformFinalBlock(buffer, 0, bytesRead);
                            if (checkBoxSHA512.Checked)
                                hash_SHA512.TransformFinalBlock(buffer, 0, bytesRead);
                        }
                        else
                        {
                            if (checkBoxMD5.Checked)
                                hash_MD5.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                            if (checkBoxSHA1.Checked)
                                hash_SHA1.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                            if (checkBoxSHA256.Checked)
                                hash_SHA256.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                            if (checkBoxSHA512.Checked)
                                hash_SHA512.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                        }
                        //Reporta el avance ( pporcentaje )
                        var reporte_mensaje = new MensajeReporte 
                        { 
                            linea = renglon_actual, 
                            porcentaje = (size == 0) ? 100 : (int)((100 * totalBytesRead) / size) 
                        };
                        barraprogreso.Report(reporte_mensaje);
                        
                        

                    } while (readAheadBytesRead != 0);

                    f_stream.Close();

                    //***
                    //Cuando acab de hacer el calculo, reporta el resultado de hash
                    //***

                    if (checkBoxMD5.Checked)
                    {
                        progressMD5.Report(new MensajeReporte 
                        { 
                            linea  = renglon_actual, 
                            mensaje = BitConverter.ToString(hash_MD5.Hash).ToUpper().Replace("-", "")
                        });
                    }
                    if (checkBoxSHA1.Checked)
                    {
                        progressSHA1.Report(new MensajeReporte
                        {
                            linea=renglon_actual,
                            mensaje = BitConverter.ToString(hash_SHA1.Hash).ToUpper().Replace("-", "")
                        });
                    }
                    if (checkBoxSHA256.Checked)
                    {
                        progressSHA256.Report(new MensajeReporte 
                        { 
                            linea = renglon_actual, 
                            mensaje = BitConverter.ToString(hash_SHA256.Hash).ToUpper().Replace("-", "") 
                        });
                        //flagsha256 = false;
                    }
                    if (checkBoxSHA512.Checked)
                    {
                        progressSHA512.Report(new MensajeReporte 
                        { 
                            linea = renglon_actual, 
                            mensaje = BitConverter.ToString(hash_SHA512.Hash).ToUpper().Replace("-", "") 
                        });
                    }
                    renglon_actual++;

                    if(cancelar)
                    {
                        break;
                    }
                }
            });

            //***
            //Establece las (des)habilitaciones de la interface usuario para evitar errores de interaccion
            //***

            bComenzar.Enabled = true;
            gBOpciones.Enabled = true;
            groupBoxAlgoritmos.Enabled = true;
            bCarpeta.Enabled = true;
            bArchivo.Enabled = true;
            bLimpiar.Enabled = true;

            bCancelar.Enabled = false;

            calculando = false;
            cancelar = false;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(calculando) 
            {
                e.Cancel = true;
            }
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
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
        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                int progressVal = (int)value;
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
