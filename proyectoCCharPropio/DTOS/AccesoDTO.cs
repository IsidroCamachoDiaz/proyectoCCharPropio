using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa un acceso con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de accesos.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class AccesoDTO
    {
        /// <summary>
        /// Identificador del acceso.
        /// </summary>
        [JsonProperty("IdAcceso")]
        private int IdAcceso;

        /// <summary>
        /// Código del acceso.
        /// </summary>
        [JsonProperty("CodigoAcceso")]
        private string CodigoAcceso;

        /// <summary>
        /// Descripción del acceso.
        /// </summary>
        [JsonProperty("DescripcionAcceso")]
        private string DescripcionAcceso;

        /// <summary>
        /// Lista de usuarios con acceso.
        /// </summary>
        [JsonProperty("Usuarios")]
        private List<UsuarioDTO> ListaUsuariosConAcceso;

        /// <summary>
        /// Constructor predeterminado para crear un objeto AccesoDTO sin atributos.
        /// </summary>
        public AccesoDTO()
        {
        }

        /// <summary>
        /// Constructor para crear un objeto AccesoDTO con todos los atributos.
        /// </summary>
        /// <param name="idAcceso">Identificador del acceso.</param>
        /// <param name="codigoAcceso">Código del acceso.</param>
        /// <param name="descripcionAcceso">Descripción del acceso.</param>
        public AccesoDTO(int idAcceso, string codigoAcceso, string descripcionAcceso)
        {
            IdAcceso = idAcceso;
            this.CodigoAcceso1 = codigoAcceso;
            DescripcionAcceso1 = descripcionAcceso;
        }

        /// <summary>
        /// Constructor para crear un objeto AccesoDTO con algunos atributos.
        /// </summary>
        /// <param name="codigoAcceso">Código del acceso.</param>
        /// <param name="descripcionAcceso">Descripción del acceso.</param>
        public AccesoDTO(string codigoAcceso, string descripcionAcceso)
        {
            CodigoAcceso1 = codigoAcceso;
            DescripcionAcceso1 = descripcionAcceso;
        }

        /// <summary>
        /// Identificador del acceso.
        /// </summary>
        public int IdAcceso1 { get => IdAcceso; set => IdAcceso = value; }

        /// <summary>
        /// Lista de usuarios con acceso.
        /// </summary>
        public List<UsuarioDTO> ListaUsuariosConAcceso1 { get => ListaUsuariosConAcceso; set => ListaUsuariosConAcceso = value; }

        /// <summary>
        /// Descripción del acceso.
        /// </summary>
        public string DescripcionAcceso1 { get => DescripcionAcceso; set => DescripcionAcceso = value; }

        /// <summary>
        /// Código del acceso.
        /// </summary>
        public string CodigoAcceso1 { get => CodigoAcceso; set => CodigoAcceso = value; }

        /// <summary>
        /// Método que devuelve una representación en cadena del objeto AccesoDTO.
        /// </summary>
        /// <returns>Cadena que representa el objeto AccesoDTO.</returns>
        public override string ToString()
        {
            return $"--- Datos Acceso ---\nIdAcceso: {IdAcceso}\nCodigoAcceso: {CodigoAcceso1}\nDescripcionAcceso: {DescripcionAcceso}\nLista Usuarios con Acceso: {ListaUsuariosConAcceso1?.ToString()}";
        }
    }
}
