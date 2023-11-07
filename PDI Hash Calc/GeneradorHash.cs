using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace PDI_Hash_Calc
{
    public class GeneradorHash
    {
        public enum AlgoritmoHash
        {
            MD5,
            SHA1,
            SHA256,
            SHA512
        }

        public static void Calcular(string file_input, AlgoritmoHash h_alg, IProgress<MensajeReporte> progressHash, int num_linea, CancellationToken Ctoken/*, Object p_lock*/)
        {
            // strean que leerael archivo
            Stream f_stream = new MemoryStream();
            long bytes_leidos;
            // variables de calculo de hash
            HashAlgorithm hash_Alg;
            hash_Alg = MD5.Create();

            switch (h_alg)
            {
                case AlgoritmoHash.MD5:
                    hash_Alg = MD5.Create();
                    break;
                case AlgoritmoHash.SHA1:
                    hash_Alg = SHA1.Create();
                    break;
                case AlgoritmoHash.SHA256:
                    hash_Alg = SHA256.Create();
                    break;
                case AlgoritmoHash.SHA512:
                    hash_Alg = SHA512.Create();
                    break;
            }


            // intenta abrir el archivo
            try
            {
                f_stream = (Stream)File.Open(file_input, FileMode.Open);
            }
            catch (Exception ex)
            {
                // Si esta ocupado por otro proceso o hay algun error de lectura, reporta y continua con el siguiente archivo
                var reporte_msg = new MensajeReporte
                {
                    linea = num_linea,//renglon_actual,
                    porcentaje = 100,
                    mensaje = "Error: " + ex.Message
                };
                //lock (p_lock)
                //{
                progressHash.Report(reporte_msg);
                //}
                return;
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

            try
            {
                do
                {
                    if (Ctoken.IsCancellationRequested)
                    {
                        progressHash.Report(new MensajeReporte
                        {
                            linea = num_linea,
                            porcentaje = 0,
                            mensaje = "Cancelado!"
                        });
                        return;
                    }

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
                        hash_Alg.TransformFinalBlock(buffer, 0, bytesRead);
                    }
                    else
                    {
                        hash_Alg.TransformBlock(buffer, 0, bytesRead, buffer, 0);

                    }
                    //Reporta el avance ( pporcentaje )
                    var reporte_mensaje = new MensajeReporte
                    {
                        linea = num_linea,
                        porcentaje = (size == 0) ? 100 : (int)((100 * totalBytesRead) / size),
                        mensaje = "Calculando..."
                    };

                    progressHash.Report(reporte_mensaje);



                } while (readAheadBytesRead != 0);

            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    //Console.WriteLine(ex.Message);
                    return true;
                });
            }
            finally
            {
                f_stream.Close();
            }

            //Termino el proceso de calculo
            progressHash.Report(new MensajeReporte
            {
                linea = num_linea,
                porcentaje = 100,
                mensaje = BitConverter.ToString(hash_Alg.Hash).ToUpper().Replace("-", "")
            });

        }



    }
}
