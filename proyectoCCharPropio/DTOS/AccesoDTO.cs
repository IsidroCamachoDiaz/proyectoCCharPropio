using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
	public class AccesoDTO
	{
		[JsonProperty("IdAcceso")]
		private long IdAccesoDto;

		[JsonProperty("CodigoAcceso")]
		private string CodigoAccesoDto;

		[JsonProperty("DescripcionAcceso")]
		private string DescripcionAccesoDto;

		[JsonProperty("Usuarios")]
		private List<UsuarioDTO> ListaUsuariosConAccesoDto;

		public AccesoDTO()
		{
		}

		public AccesoDTO(long idAccesoDto, string codigoAccesoDto, string descripcionAccesoDto)
		{
			IdAccesoDto = idAccesoDto;
			CodigoAccesoDto = codigoAccesoDto;
			DescripcionAccesoDto = descripcionAccesoDto;
		}

		public AccesoDTO(string codigoAcceso, string descripcionAcceso)
		{
			CodigoAccesoDto = codigoAcceso;
			DescripcionAccesoDto = descripcionAcceso;
		}

		public long IdAcceso
		{
			get { return IdAccesoDto; }
			set { IdAccesoDto = value; }
		}

		public string CodigoAcceso
		{
			get { return CodigoAccesoDto; }
			set { CodigoAccesoDto = value; }
		}

		public string DescripcionAcceso
		{
			get { return DescripcionAccesoDto; }
			set { DescripcionAccesoDto = value; }
		}

		public List<UsuarioDTO> ListaUsuariosConAcceso
		{
			get { return ListaUsuariosConAccesoDto; }
			set { ListaUsuariosConAccesoDto = value; }
		}

		public override string ToString()
		{
			return $"--- Datos Acceso ---\nIdAcceso: {IdAccesoDto}\nCodigoAcceso: {CodigoAccesoDto}\nDescripcionAcceso: {DescripcionAccesoDto}\nLista Usuarios con Acceso: {ListaUsuariosConAccesoDto?.ToString()}";
		}
	}
}
