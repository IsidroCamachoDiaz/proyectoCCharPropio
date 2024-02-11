using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejercicioRepaso.Modelo
{
    /// <summary>
    /// Clase para la escritura de fichero log
    /// </summary>
    ///<autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class implementacionEscritura : interfazEscritura
    {
        StreamWriter interfazEscritura.AbrirOCrearFichero(string ruta, bool sobreEscribir)
        {
            StreamWriter sw = null;

            try
            {
                // Comprueba si el archivo ya existe
                if (!File.Exists(ruta))
                {
                    // Si no existe, crea el archivo
                    using (File.Create(ruta)) { }
                }

                // Abre el archivo para escritura
                sw = new StreamWriter(ruta, sobreEscribir);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return sw;
        }

        void interfazEscritura.cerrar(StreamWriter sw)
        {
            //Lo cierra
            sw.Close();
        }

        void interfazEscritura.escribir(StreamWriter sw, string texto)
        {
            //Ecribe el texto
            sw.WriteLine(texto);
        }
    }
}
