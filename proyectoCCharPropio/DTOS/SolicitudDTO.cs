using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa una solicitud con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de solicitudes.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class SolicitudDTO
    {
        /// <summary>
        /// Identificador de la solicitud.
        /// </summary>
        [JsonProperty("IdSolicitud")]
        private int idSolicitud;

        /// <summary>
        /// Descripción de la solicitud.
        /// </summary>
        [JsonProperty("DescripcionSolicitud")]
        private string descripcionSolicitud;

        /// <summary>
        /// Estado de la solicitud.
        /// </summary>
        [JsonProperty("Estado")]
        private bool estado;

        /// <summary>
        /// Fecha límite de la solicitud.
        /// </summary>
        [JsonProperty("FechaLimite")]
        private DateTime fechaLimite;

        /// <summary>
        /// Usuario asociado a la solicitud.
        /// </summary>
        [JsonProperty("IdUsuarioNavigation")]
        private UsuarioDTO idUsuario;

        [JsonProperty("IdUsuario")]
        private int idUsuarioNumero { get; set; }


        /// <summary>
        /// Identificador de la solicitud.
        /// </summary>
        public int IdSolicitud2 { get => idSolicitud; set => idSolicitud = value; }

        /// <summary>
        /// Descripción de la solicitud.
        /// </summary>
        public string DescripcionSolicitud2 { get => descripcionSolicitud; set => descripcionSolicitud = value; }

        /// <summary>
        /// Estado de la solicitud.
        /// </summary>
        public bool Estado2 { get => estado; set => estado = value; }

        /// <summary>
        /// Fecha límite de la solicitud.
        /// </summary>
        public DateTime FechaLimite2 { get => fechaLimite; set => fechaLimite = value; }

        /// <summary>
        /// Usuario asociado a la solicitud.
        /// </summary>
        public UsuarioDTO IdUsuario2 { get => idUsuario; set => idUsuario = value; }


        /// <summary>
        /// Constructor para crear un objeto SolicitudDTO con todos los atributos.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud.</param>
        /// <param name="descripcionSolicitud">Descripción de la solicitud.</param>
        /// <param name="estado">Estado de la solicitud.</param>
        /// <param name="fechaLimite">Fecha límite de la solicitud.</param>
        /// <param name="idUsuario">Usuario asociado a la solicitud.</param>
        /// <param name="incidencia">Incidencia asociada a la solicitud.</param>
        public SolicitudDTO(int idSolicitud, string descripcionSolicitud, bool estado, DateTime fechaLimite, UsuarioDTO idUsuario)
        {
            IdSolicitud2 = idSolicitud;
            DescripcionSolicitud2 = descripcionSolicitud;
            Estado2 = estado;
            FechaLimite2 = fechaLimite;
            IdUsuario2 = idUsuario;
        }

        /// <summary>
        /// Constructor para crear un objeto SolicitudDTO con algunos atributos.
        /// </summary>
        /// <param name="descripcionSolicitud">Descripción de la solicitud.</param>
        /// <param name="estado">Estado de la solicitud.</param>
        /// <param name="fechaLimite">Fecha límite de la solicitud.</param>
        /// <param name="idUsuario">Usuario asociado a la solicitud.</param>
        public SolicitudDTO(string descripcionSolicitud, bool estado, DateTime fechaLimite, UsuarioDTO idUsuario)
        {
            DescripcionSolicitud2 = descripcionSolicitud;
            Estado2 = estado;
            FechaLimite2 = fechaLimite;
            IdUsuario2 = idUsuario;
        }

        /// <summary>
        /// Constructor predeterminado para crear un objeto SolicitudDTO sin atributos.
        /// </summary>
        public SolicitudDTO()
        {
        }
    }
}
