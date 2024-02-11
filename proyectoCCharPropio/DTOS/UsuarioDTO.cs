using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    [Serializable]
    /// <summary>
    /// Clase que representa un usuario con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de usuarios.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class UsuarioDTO
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        [JsonProperty("IdUsuario")]
        private int id_usuario;

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        [JsonProperty("NombreUsuario")]
        private string nombre_usuario;

        /// <summary>
        /// Número de teléfono del usuario.
        /// </summary>
        [JsonProperty("TelefonoUsuario")]
        private string telefono_usuario;

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        [JsonProperty("CorreoUsuario")]
        private string correo_usuario;

        /// <summary>
        /// Clave o contraseña del usuario.
        /// </summary>
        [JsonProperty("ContraseniaUsuario")]
        private string contrasenia_usuario;

        /// <summary>
        /// Identificador del acceso del usuario.
        /// </summary>
        [JsonProperty("IdAcceso")]
        private int id_acceso;

        /// <summary>
        /// Indicador de alta del usuario.
        /// </summary>
        [JsonProperty("Alta")]
        private bool alta_usuario;

        /// <summary>
        /// Foto del usuario.
        /// </summary>
        [JsonProperty("FotoUsuario")]
        private byte[] foto_usuario;

        /// <summary>
        /// Fecha de baja del usuario (opcional).
        /// </summary>
        [JsonProperty("FechaBaja")]
        private DateTime? fecha_baja;

        /// <summary>
        /// Constructor para crear un objeto UsuarioDTO con todos los atributos.
        /// </summary>
        /// <param name="id_usuario">Identificador único del usuario.</param>
        /// <param name="nombre_usuario">Nombre del usuario.</param>
        /// <param name="tlf_usuario">Número de teléfono del usuario.</param>
        /// <param name="email_usuario">Dirección de correo electrónico del usuario.</param>
        /// <param name="clave_usuario">Clave o contraseña del usuario.</param>
        /// <param name="foto">Foto del usuario.</param>
        public UsuarioDTO(int id_usuario, string nombre_usuario, string tlf_usuario, string email_usuario, string clave_usuario, byte[] foto)
        {
            this.id_usuario = id_usuario;
            this.nombre_usuario = nombre_usuario;
            this.telefono_usuario = tlf_usuario;
            this.correo_usuario = email_usuario;
            this.contrasenia_usuario = clave_usuario;
            this.foto_usuario = foto;
        }

        /// <summary>
        /// Constructor para crear un objeto UsuarioDTO con algunos atributos.
        /// </summary>
        /// <param name="nombre_usuario">Nombre del usuario.</param>
        /// <param name="tlf_usuario">Número de teléfono del usuario.</param>
        /// <param name="email_usuario">Dirección de correo electrónico del usuario.</param>
        /// <param name="clave_usuario">Clave o contraseña del usuario.</param>
        public UsuarioDTO(string nombre_usuario, string tlf_usuario, string email_usuario, string clave_usuario)
        {
            this.nombre_usuario = nombre_usuario;
            this.telefono_usuario = tlf_usuario;
            this.correo_usuario = email_usuario;
            this.contrasenia_usuario = clave_usuario;
        }

        /// <summary>
        /// Constructor predeterminado para crear un objeto UsuarioDTO sin atributos.
        /// </summary>
        public UsuarioDTO()
        {
        }

        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id_usuario { get => id_usuario; set => id_usuario = value; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string Nombre_usuario { get => nombre_usuario; set => nombre_usuario = value; }

        /// <summary>
        /// Número de teléfono del usuario.
        /// </summary>
        public string Telefono_usuario { get => telefono_usuario; set => telefono_usuario = value; }

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        public string Correo_usuario { get => correo_usuario; set => correo_usuario = value; }

        /// <summary>
        /// Clave o contraseña del usuario.
        /// </summary>
        public string Contrasenia_usuario { get => contrasenia_usuario; set => contrasenia_usuario = value; }

        /// <summary>
        /// Identificador del acceso del usuario.
        /// </summary>
        public int Id_acceso { get => id_acceso; set => id_acceso = value; }

        /// <summary>
        /// Foto del usuario.
        /// </summary>
        public byte[] Foto_usuario { get => foto_usuario; set => foto_usuario = value; }

        /// <summary>
        /// Indicador de alta del usuario.
        /// </summary>
        public bool Alta_usuario { get => alta_usuario; set => alta_usuario = value; }

        /// <summary>
        /// Fecha de baja del usuario (opcional).
        /// </summary>
        public DateTime? Fecha_baja { get => fecha_baja; set => fecha_baja = value; }
    }
}
