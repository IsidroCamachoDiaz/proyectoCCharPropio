using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace proyectoCCharPropio.DTOS
{
    public class SolicitudDTO
    {
        [JsonProperty("IdSolicitud")]
        private int idSolicitud;

        [JsonProperty("DescripcionSolicitud")]
        private string descripcionSolicitud;

        [JsonProperty("Estado")]
        private bool estado;

        [JsonProperty("FechaLimite")]
        private DateTime fechaLimite;

        [JsonProperty("IdUsuarioNavigation")]
        private UsuarioDTO idUsuario;

        [JsonProperty("Incidencia")]
        private IncidenciaDTO incidencia;

        public int IdSolicitud2 { get => idSolicitud; set => idSolicitud = value; }
        public string DescripcionSolicitud2 { get => descripcionSolicitud; set => descripcionSolicitud = value; }
        public bool Estado2 { get => estado; set => estado = value; }
        public DateTime FechaLimite2 { get => fechaLimite; set => fechaLimite = value; }
        public UsuarioDTO IdUsuario2 { get => idUsuario; set => idUsuario = value; }
        public IncidenciaDTO Incidencia2 { get => incidencia; set => incidencia = value; }

        public SolicitudDTO(int idSolicitud, string descripcionSolicitud, bool estado, DateTime fechaLimite, UsuarioDTO idUsuario, IncidenciaDTO incidencia)
        {
            IdSolicitud2 = idSolicitud;
            DescripcionSolicitud2 = descripcionSolicitud;
            Estado2 = estado;
            FechaLimite2 = fechaLimite;
            IdUsuario2 = idUsuario;
            Incidencia2 = incidencia;
        }

        public SolicitudDTO(string descripcionSolicitud, bool estado, DateTime fechaLimite, UsuarioDTO idUsuario)
        {
            DescripcionSolicitud2 = descripcionSolicitud;
            Estado2 = estado;
            FechaLimite2 = fechaLimite;
            IdUsuario2 = idUsuario;
        }

        public SolicitudDTO()
        {
        }


    }
}
