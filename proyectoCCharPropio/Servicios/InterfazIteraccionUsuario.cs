using System;
using proyectoCCharPropio.DTOS;

namespace pruebaRazor.DTOs
{
	public interface InterfazIteraccionUsuario
	{
		public bool RegistrarUsuario(UsuarioDTO usu);

		public Task<bool> LoginUsuario(UsuarioDTO usu);

		public bool RecuperarContrasenya(UsuarioDTO usu);

		public Task<UsuarioDTO> ObtieneUsuarioPorGmail(UsuarioDTO usu);
	}
}
