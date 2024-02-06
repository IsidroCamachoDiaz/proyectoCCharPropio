using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    public class SolicitudDTO
    {
        [JsonProperty("IdSolicitud")]
        public int IdSolicitud { get; set; }

        [JsonProperty("DescripcionSolicitud")]
        public string DescripcionSolicitud { get; set; } = null!;

        [JsonProperty("Estado")]
        public bool Estado { get; set; }

        [JsonProperty("FechaLimite")]
        public DateTime FechaLimite { get; set; }

        [JsonProperty("IdCliente")]
        public int IdUsuario { get; set; }

        [JsonProperty("Incidencia")]
        public int Incidencia { get; set; }

        public SolicitudDTO(int idSolicitud, string descripcionSolicitud, bool estado, DateTime fechaLimite, int idUsuario, int incidencia)
        {
            IdSolicitud = idSolicitud;
            DescripcionSolicitud = descripcionSolicitud;
            Estado = estado;
            FechaLimite = fechaLimite;
            IdUsuario = idUsuario;
            Incidencia = incidencia;
        }

        public SolicitudDTO(string descripcionSolicitud, bool estado, DateTime fechaLimite, int idUsuario)
        {
            DescripcionSolicitud = descripcionSolicitud;
            Estado = estado;
            FechaLimite = fechaLimite;
            IdUsuario = idUsuario;
        }



    }
}
