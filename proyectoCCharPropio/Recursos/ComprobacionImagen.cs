namespace proyectoCCharPropio.Recursos
{
    /// <summary>
    /// Clase para la comprobacion y conversion de imagen a rray de bytes
    /// </summary>
    ///<autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class ComprobacionImagen
    {
        // Método para convertir el archivo en un array de bytes
        public byte[] ConvertirImagenEnBytes(IFormFile archivo)
        {
            //Compruebo si la imagen es nulla o no es una imagen propia
            if (archivo == null || archivo.Length == 0 || !EsImagen(archivo.FileName))
            {
                // Si el archivo es nulo, está vacío o no es una imagen, devolver la imagen por defecto
                string imagenPorDefectoPath = "user.png";
                //Devuelvo la imagen por defecto convertida en array de butes
                return File.ReadAllBytes(imagenPorDefectoPath);
            }

            // Convierte el archivo en un array de bytes
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //Copiamos los datos de flujo a archivo de memoria
                archivo.CopyTo(memoryStream);
                //Lo convertimos en un array d ebytes
                return memoryStream.ToArray();
            }
        }

        // Método para validar si la extensión del archivo corresponde a una imagen
        private bool EsImagen(string nombreArchivo)
        {
            //Cojo la extension del archivo
            string extension = Path.GetExtension(nombreArchivo);
            //Declaro el array de extensiones
            string[] extensionesImagen = { ".jpg", ".jpeg", ".png"};
            //Compruebo si la extension del archivo esta en el array de extensiones validas
            return extensionesImagen.Contains(extension.ToLower());
        }
    }
}
