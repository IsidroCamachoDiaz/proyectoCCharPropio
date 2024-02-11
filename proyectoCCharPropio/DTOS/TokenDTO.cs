using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa un token con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de tokens.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class TokenDTO
    {
        /// <summary>
        /// Identificador del token en la tabla.
        /// </summary>
        [JsonProperty("IdToken")]
        private long id_token_tabla;

        /// <summary>
        /// Token generado.
        /// </summary>
        [JsonProperty("Token1")]
        private string token;

        /// <summary>
        /// Identificador del usuario asociado al token.
        /// </summary>
        [JsonProperty("IdUsuario")]
        private int id_usuario;

        /// <summary>
        /// Fecha límite del token.
        /// </summary>
        [JsonProperty("FechaLimite")]
        private DateTime fecha_limite_token;

        /// <summary>
        /// Constructor para crear un objeto TokenDTO con algunos atributos.
        /// </summary>
        /// <param name="token">Token generado.</param>
        /// <param name="id_usuario">Identificador del usuario asociado al token.</param>
        /// <param name="fecha_limite_token">Fecha límite del token.</param>
        public TokenDTO(string token, int id_usuario, DateTime fecha_limite_token)
        {
            this.token = token;
            this.id_usuario = id_usuario;
            this.fecha_limite_token = fecha_limite_token;
        }

        /// <summary>
        /// Constructor predeterminado para crear un objeto TokenDTO sin atributos.
        /// </summary>
        public TokenDTO()
        {
        }

        /// <summary>
        /// Identificador del token en la tabla.
        /// </summary>
        public long Id_token_tabla { get => id_token_tabla; set => id_token_tabla = value; }

        /// <summary>
        /// Token generado.
        /// </summary>
        public string Token { get => token; set => token = value; }

        /// <summary>
        /// Identificador del usuario asociado al token.
        /// </summary>
        public int Id_usuario { get => id_usuario; set => id_usuario = value; }

        /// <summary>
        /// Fecha límite del token.
        /// </summary>
        public DateTime Fecha_limite_token { get => fecha_limite_token; set => fecha_limite_token = value; }
    }
}
