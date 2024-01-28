using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoCCharPropio.DTOS
{
	public class TokenDTO
	{
        // Atributos
        [JsonProperty("IdToken")]
        private long id_token_tabla;
        [JsonProperty("Token1")]
        private string token;
        [JsonProperty("IdUsuario")]
        private int id_usuario;
        [JsonProperty("FechaLimite")]
        private DateTime fecha_limite_token;

		// Constructores

		public TokenDTO(string token, int id_usuario, DateTime fecha_limite_token)
		{
			this.token = token;
			this.id_usuario = id_usuario;
			this.fecha_limite_token = fecha_limite_token;
		}

		public TokenDTO()
		{
		}

		// Getter y Setter

		public long Id_token_tabla { get => id_token_tabla; set => id_token_tabla = value; }
		public string Token { get => token; set => token = value; }
		public int Id_usuario { get => id_usuario; set => id_usuario = value; }
		public DateTime Fecha_limite_token { get => fecha_limite_token; set => fecha_limite_token = value; }

	}
}
