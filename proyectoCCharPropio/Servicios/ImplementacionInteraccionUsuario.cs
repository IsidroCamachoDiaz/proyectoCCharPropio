using Newtonsoft.Json;
using NuGet.Common;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;
using pruebaRazor.DTOs;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;

namespace pruebaRazor.DTOs
{
	public class ImplementacionInteraccionUsuario : InterfazIteraccionUsuario
	{
		public bool RegistrarUsuario(UsuarioDTO usu)
		{
			try
			{
				accionesCRUD acciones = new accionesCRUD();
				usu.Id_acceso = 3;
                usu.Contrasenia_usuario = Util.EncriptarContra(usu.Contrasenia_usuario);
				usu.Alta_usuario = false;
                if (acciones.InsertarUsuario(usu))
				{
                    // Creamos el token
                    // Primero creamos la fecha limite
                    DateTime fechaLimite = DateTime.Now.AddMinutes(10);
                    Console.WriteLine(fechaLimite);

                    // Ahora creamos el token
                    Guid guid = Guid.NewGuid();

                    // Convertir el GUID a una cadena (string)
                    string token = guid.ToString();
                    Correo c = new Correo();
					UsuarioDTO usuarioConId = acciones.SeleccionarUsuario("correo/"+usu.Correo_usuario);
					//Creamos el token
                    TokenDTO tokenDto = new TokenDTO(token, usuarioConId.Id_usuario, fechaLimite);

                    String mensaje = c.MensajeCorreoAlta(token, "https://localhost:7048/RegistroControlador/altaHecha");
                   
					//Comprobamos si se ha enviado bien el correo
					if(c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Recuperar Contraseña", "isidro@isidrocamachodiaz.es", true))
					{
						//Insertamos en el token
						acciones.InsertarToken(tokenDto);
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}

			}
			catch (JsonException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
			}
			catch (SecurityException s)
			{
				Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
				Console.Error.WriteLine(io.Message);
			}
			return false;
		}

		public async Task<bool> LoginUsuario(UsuarioDTO usuario, HttpContext httpContext)
		{

			try
			{
                accionesCRUD acciones = new accionesCRUD();

                // Encriptamos la contraseña del usuario para poder comparar con las de la base de datos
                usuario.Contrasenia_usuario = Util.EncriptarContra(usuario.Contrasenia_usuario);

                UsuarioDTO usuarioBD = acciones.SeleccionarUsuario("correo/" + usuario.Correo_usuario);
                httpContext.Session.SetString("usuario", usuarioBD.Id_usuario.ToString());
                httpContext.Session.SetString("acceso", usuarioBD.Id_acceso.ToString());

                if (usuario.Contrasenia_usuario!=usuarioBD.Contrasenia_usuario)
				{
				
					return false;
				}
				else
				{
                    if (!usuarioBD.Alta_usuario)
                    {
                        return false;
                    }
                    return true;
				}

            }
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public bool RecuperarContrasenya(UsuarioDTO usu)
		{
			bool ok = false;
			accionesCRUD acciones = new accionesCRUD();
           
            try
			{
                // Creamos el token
                // Primero creamos la fecha limite
                DateTime fechaLimite = DateTime.Now.AddMinutes(10);
                Console.WriteLine(fechaLimite);

                // Ahora creamos el token
                Guid guid = Guid.NewGuid();

                // Convertir el GUID a una cadena (string)
                string token = guid.ToString();
        

                Console.WriteLine(token);
				//Creamos el token
                TokenDTO tokenDto = new TokenDTO(token, usu.Id_usuario, fechaLimite);
				//Comprobamos si se creo bien
				if (acciones.InsertarToken(tokenDto))
				{
                    // Aqui llamamos a los nuevos metodos
                    Correo c = new Correo();
                    //Creamos el mensaje del correo con la url uy el token
                    String mensaje = c.MensajeCorreo(token, "https://localhost:7048/RegistroControlador/Modificar");
                    //Enviamos en el mensanje
                    ok = c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Recuperar Contraseña", "isidro@isidrocamachodiaz.es", true);
					//Comprobamos si se envio bien
					if (ok)
						return true;
					else
						return false;
				}
				return false;

            }
			catch (JsonException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] " + e.Message);
			}
			catch (SecurityException s)
			{
				Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
				Console.Error.WriteLine(io.Message);
			}
			return false;
		}


	

		// Método cambiar
		public async Task<bool> CambiarContrasenia(String contrasenia, TokenDTO to)
		{
			accionesCRUD acciones= new accionesCRUD();
			UsuarioDTO usuario = null;

            try
			{
				//Buscamos al usuario por el id de usuario del token
				usuario = acciones.SeleccionarUsuario(to.Id_usuario.ToString());
				//Si no lo encuentra
				if (usuario == null)
				{
                    return false;
                }
				//Si esta se le cambia la contraseña
				usuario.Contrasenia_usuario= contrasenia;
				//Se actualiza en la base de datos y se comprueba que este bien
				if (acciones.ActualizarUsuario(usuario))
				{
					return true;
				}
				return false;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public async Task<TokenDTO> ObtenerToken(string valorTk)
		{
			accionesCRUD acciones = new accionesCRUD();

			try
			{
				TokenDTO tokenEncontrado = acciones.SeleccionarToken("token/" + valorTk);
				return tokenEncontrado;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return null;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return null;
			}
		}
	}
}
