using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    public class TipoTrabajoDTO
    {
        [JsonProperty("IdTipo")]
        public int IdTipo { get; set; }

        [JsonProperty("DescripcionTipo")]
        public string DescripcionTipo { get; set; } = null!;

        [JsonProperty("FechaExpiracion")]
        public DateTime? FechaExpiracion { get; set; }

        [JsonProperty("PrecioTipo")]
        public float PrecioTipo { get; set; }

        public TipoTrabajoDTO(string descripcionTipo, float precioTipo)
        {
            DescripcionTipo = descripcionTipo;
            PrecioTipo = precioTipo;
        }

        public TipoTrabajoDTO(int idTipo, string descripcionTipo, DateTime? fechaExpiracion, float precioTipo)
        {
            IdTipo = idTipo;
            DescripcionTipo = descripcionTipo;
            FechaExpiracion = fechaExpiracion;
            PrecioTipo = precioTipo;
        }

        public TipoTrabajoDTO()
        {
        }
    }
}
