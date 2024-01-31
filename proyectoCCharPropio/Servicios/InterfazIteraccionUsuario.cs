using System;
using proyectoCCharPropio.DTOS;

namespace pruebaRazor.DTOs
{
	public interface InterfazIteraccionUsuario
	{
		public bool RegistrarUsuario(UsuarioDTO usu);

		public Task<bool> LoginUsuario(UsuarioDTO usu, HttpContext httpContext);

		public bool RecuperarContrasenya(UsuarioDTO usu);

	}
}
