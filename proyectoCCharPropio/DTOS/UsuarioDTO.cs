using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    [Serializable]
    public class UsuarioDTO
	{
        // Atributos
        [JsonProperty("IdUsuario")]
        private int id_usuario;
        [JsonProperty("NombreUsuario")]
        private string nombre_usuario;
        [JsonProperty("TelefonoUsuario")]
        private string telefono_usuario;
        [JsonProperty("CorreoUsuario")]
        private string correo_usuario;
        [JsonProperty("ContraseniaUsuario")]
        private string contrasenia_usuario;
       [JsonProperty("IdAcceso")]
        private int id_acceso;
        [JsonProperty("FotoUsuario")]
        private byte[] foto_usuario;

        // Constructores

        public UsuarioDTO(int id_usuario, string nombre_usuario,string tlf_usuario, string email_usuario, string clave_usuario, byte[] foto)
		{
			this.id_usuario = id_usuario;
			this.nombre_usuario = nombre_usuario;
			this.telefono_usuario = tlf_usuario;
			this.correo_usuario = email_usuario;
			this.contrasenia_usuario = clave_usuario;
			this.foto_usuario= foto;
		}

		public UsuarioDTO(string nombre_usuario,string tlf_usuario, string email_usuario, string clave_usuario)
		{
			this.nombre_usuario = nombre_usuario;
			this.telefono_usuario = tlf_usuario;
			this.correo_usuario = email_usuario;
			this.contrasenia_usuario = clave_usuario;
		}

		public UsuarioDTO()
		{
		}

		// Getter y Setter

		public int Id_usuario { get => id_usuario; set => id_usuario = value; }
		public string Nombre_usuario { get => nombre_usuario; set => nombre_usuario = value; }
		public string Telefono_usuario { get => telefono_usuario; set => telefono_usuario = value; }
		public string Correo_usuario { get => correo_usuario; set => correo_usuario = value; }
		public string Contrasenia_usuario { get => contrasenia_usuario; set => contrasenia_usuario = value; }
		public int Id_acceso { get => id_acceso; set => id_acceso = value;}
        public byte[] Foto_usuario { get => foto_usuario; set => foto_usuario = value; }



        // toString
    }
}
