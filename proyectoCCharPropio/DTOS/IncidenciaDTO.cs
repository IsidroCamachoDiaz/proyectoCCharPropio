using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    public class IncidenciaDTO
    {

        [JsonProperty("IdIncidencia")]
        private int IdIncidencia { get; set; }

        [JsonProperty("CosteIncidencia")]
        private float CosteIncidencia { get; set; }

        [JsonProperty("DescripcionTecnica")]
        private string DescripcionTecnica { get; set; }

        [JsonProperty("DescripcionUsuario")]
        private string DescripcionUsuario { get; set; } = null!;

        [JsonProperty("EstadoIncidencia")]
        private bool EstadoIncidencia { get; set; }

        [JsonProperty("FechaFin")]
        private DateTime FechaFin { get; set; }

        [JsonProperty("FechaInicio")]
        private DateTime FechaInicio { get; set; }

        [JsonProperty("HorasIncidencia")]
        private int HorasIncidencia { get; set; }

        [JsonProperty("IdUsuarioNavigation")]
        private UsuarioDTO IdUsuario;

        [JsonProperty("IdSolicitudNavigation")]
        private SolicitudDTO solicitud;

        [JsonIgnore]
        public SolicitudDTO Solicitud { get => solicitud; set => solicitud = value; }
        public UsuarioDTO Usuario { get => IdUsuario; set => IdUsuario = value; }

        public IncidenciaDTO(string descripcionUsuario, bool estadoIncidencia, SolicitudDTO idSolicitud)
        {
            DescripcionUsuario = descripcionUsuario;
            EstadoIncidencia = estadoIncidencia;
            Solicitud = idSolicitud;
        }

        public IncidenciaDTO()
        {
        }

        public IncidenciaDTO(int idIncidencia, float costeIncidencia, string descripcionTecnica, string descripcionUsuario, bool estadoIncidencia, DateTime fechaFin, DateTime fechaInicio, int horasIncidencia, UsuarioDTO idUsuario, SolicitudDTO idSolicitud)
        {
            IdIncidencia = idIncidencia;
            CosteIncidencia = costeIncidencia;
            DescripcionTecnica = descripcionTecnica;
            DescripcionUsuario = descripcionUsuario;
            EstadoIncidencia = estadoIncidencia;
            FechaFin = fechaFin;
            FechaInicio = fechaInicio;
            HorasIncidencia = horasIncidencia;
            Usuario = idUsuario;
            Solicitud = idSolicitud;
        }


    }
}
