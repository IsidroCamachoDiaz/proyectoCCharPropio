using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    public class TrabajoDTO
    {

        [JsonProperty("IdTrabajo")]
        public int IdTrabajo { get; set; }

        [JsonProperty("DescripcionTrabajo")]
        public string DescripcionTrabajo { get; set; } = null!;

        [JsonProperty("EstadoTrabajo")]
        public bool EstadoTrabajo { get; set; }

        [JsonProperty("HorasTrabajo")]
        public int HorasTrabajo { get; set; }

        [JsonProperty("IdIncidenciaNavigation")]
        public IncidenciaDTO incidencia{ get; set; }

        [JsonProperty("IdTipoIncidenciaNavigation")]
        public TipoTrabajoDTO tipo { get; set; }

        public TrabajoDTO()
        {
        }

        public TrabajoDTO(string descripcionTrabajo, bool estadoTrabajo, int horasTrabajo, IncidenciaDTO incidencia, TipoTrabajoDTO tipo)
        {
            DescripcionTrabajo = descripcionTrabajo;
            EstadoTrabajo = estadoTrabajo;
            HorasTrabajo = horasTrabajo;
            this.incidencia = incidencia;
            this.tipo = tipo;
        }

        public TrabajoDTO(int idTrabajo, string descripcionTrabajo, bool estadoTrabajo, int horasTrabajo, IncidenciaDTO incidencia, TipoTrabajoDTO tipo)
        {
            IdTrabajo = idTrabajo;
            DescripcionTrabajo = descripcionTrabajo;
            EstadoTrabajo = estadoTrabajo;
            HorasTrabajo = horasTrabajo;
            this.incidencia = incidencia;
            this.tipo = tipo;
        }
    }
}
