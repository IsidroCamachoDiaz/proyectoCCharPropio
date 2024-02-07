using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
	public class AccesoDTO
	{
		[JsonProperty("IdAcceso")]
		private int IdAcceso;

		[JsonProperty("CodigoAcceso")]
		private string CodigoAcceso;

		[JsonProperty("DescripcionAcceso")]
		private string DescripcionAcceso;

		[JsonProperty("Usuarios")]
		private List<UsuarioDTO> ListaUsuariosConAcceso;

		public AccesoDTO()
		{
		}

		public AccesoDTO(int idAcceso, string codigoAcceso, string descripcionAcceso)
		{
			IdAcceso = idAcceso;
			this.CodigoAcceso1 = codigoAcceso;
			DescripcionAcceso1 = descripcionAcceso;
		}

		public AccesoDTO(string codigoAcceso, string descripcionAcceso)
		{
			CodigoAcceso1 = codigoAcceso;
			DescripcionAcceso1 = descripcionAcceso;
		}


        public int IdAcceso1 { get => IdAcceso; set => IdAcceso = value; }
        public List<UsuarioDTO> ListaUsuariosConAcceso1 { get => ListaUsuariosConAcceso; set => ListaUsuariosConAcceso = value; }
        public string DescripcionAcceso1 { get => DescripcionAcceso; set => DescripcionAcceso = value; }
        public string CodigoAcceso1 { get => CodigoAcceso; set => CodigoAcceso = value; }

        public override string ToString()
		{
			return $"--- Datos Acceso ---\nIdAcceso: {IdAcceso}\nCodigoAcceso: {CodigoAcceso1}\nDescripcionAcceso: {DescripcionAcceso}\nLista Usuarios con Acceso: {ListaUsuariosConAcceso1?.ToString()}";
		}
	}
}
