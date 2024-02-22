using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa un tipo de trabajo con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de tipos de trabajo.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class TipoTrabajoDTO
    {
        /// <summary>
        /// Identificador del tipo de trabajo.
        /// </summary>
        [JsonProperty("IdTipo")]
        public int IdTipo { get; set; }

        /// <summary>
        /// Descripción del tipo de trabajo.
        /// </summary>
        [JsonProperty("DescripcionTipo")]
        public string DescripcionTipo { get; set; } = null!;

        /// <summary>
        /// Fecha de expiración del tipo de trabajo.
        /// </summary>
        [JsonProperty("FechaExpiracion")]
        public DateTime? FechaExpiracion { get; set; }

        /// <summary>
        /// Precio del tipo de trabajo.
        /// </summary>
        [JsonProperty("PrecioTipo")]
        public float PrecioTipo { get; set; }

        /// <summary>
        /// Constructor para crear un objeto TipoTrabajoDTO con algunos atributos.
        /// </summary>
        /// <param name="descripcionTipo">Descripción del tipo de trabajo.</param>
        /// <param name="precioTipo">Precio del tipo de trabajo.</param>
        public TipoTrabajoDTO(string descripcionTipo, float precioTipo)
        {
            DescripcionTipo = descripcionTipo;
            PrecioTipo = precioTipo;
        }

        /// <summary>
        /// Constructor para crear un objeto TipoTrabajoDTO con todos los atributos.
        /// </summary>
        /// <param name="idTipo">Identificador del tipo de trabajo.</param>
        /// <param name="descripcionTipo">Descripción del tipo de trabajo.</param>
        /// <param name="fechaExpiracion">Fecha de expiración del tipo de trabajo.</param>
        /// <param name="precioTipo">Precio del tipo de trabajo.</param>
        public TipoTrabajoDTO(int idTipo, string descripcionTipo, DateTime? fechaExpiracion, float precioTipo)
        {
            IdTipo = idTipo;
            DescripcionTipo = descripcionTipo;
            FechaExpiracion = fechaExpiracion;
            PrecioTipo = precioTipo;
        }

        /// <summary>
        /// Constructor predeterminado para crear un objeto TipoTrabajoDTO sin atributos.
        /// </summary>
        public TipoTrabajoDTO()
        {
        }
    }
}
