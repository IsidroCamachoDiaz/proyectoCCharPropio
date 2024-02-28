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
				//Declaramos lo que necesitemos
				accionesCRUD acciones = new accionesCRUD();

				//Cogemos todos los accesos
				List<AccesoDTO> accesos = acciones.HacerGetLista<AccesoDTO>("api/Acceso");

				//Buscamos el acceso del usuario
				foreach(AccesoDTO a in accesos)
				{
					//Comprobamos si es el acceso del usuario
					if (a.CodigoAcceso1 == "Usuario")
					{
                        //Le asignamos el id del usuario
                        usu.Id_acceso = a.IdAcceso1;
						break;
					}
				}
			
                usu.Contrasenia_usuario = Util.EncriptarContra(usu.Contrasenia_usuario);
				//Le asiganamos que no esta dado de alta
				usu.Alta_usuario = false;

				//Comproabamos si se inserto bien
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

					//Creamos el mensaje de correo de alta
                    String mensaje = c.MensajeCorreoAlta(token, "https://localhost:7048/RegistroControlador/altaHecha");
                   
					//Comprobamos si se ha enviado bien el correo
					if(c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Alta De Usuario", "isidro@isidrocamachodiaz.es", true))
					{
						//Insertamos en el token
						Util.EscribirEnElFichero("Se ha creado un usuario correctamente se le envio un correo de alta");
						acciones.InsertarToken(tokenDto);
						return true;
					}
					//No se pudo enviar el correo
					else
					{
						Util.EscribirEnElFichero("Hubo problemas para enviar el correo");
						return false;
					}
				}
				//No se puede insertar el usuario
				else
				{
					Util.EscribirEnElFichero("Hubo problemas para insertar al usuario");
					return false;
				}

			}
			catch (JsonException e)
			{
				Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
				Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
				Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)	
			{
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
			}
			catch (SecurityException s)
			{
                Util.EscribirEnElFichero(s.Message);
                Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
                Util.EscribirEnElFichero(io.Message);
                Console.Error.WriteLine(io.Message);
			}
			return false;
		}

		public async Task<bool> LoginUsuario(UsuarioDTO usuario, HttpContext httpContext)
		{

			try
			{
				//Creamo lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();

                // Encriptamos la contraseña del usuario para poder comparar con las de la base de datos
                usuario.Contrasenia_usuario = Util.EncriptarContra(usuario.Contrasenia_usuario);
				//Buscamos al usuario
                UsuarioDTO usuarioBD = acciones.SeleccionarUsuario("correo/" + usuario.Correo_usuario);

				//Comprobamos si encontro el usuario
				if(usuarioBD == null)
				{
                    Util.EscribirEnElFichero("Una persona puso un correo que no esta asociada a ninguna cuenta");
					return false;
                }

				//ponemos los valores que necesitemos
                httpContext.Session.SetString("usuario", usuarioBD.Id_usuario.ToString());
                httpContext.Session.SetString("acceso", usuarioBD.Id_acceso.ToString());
				//Este campo es para verificar el controlador si esta dado de alta
                httpContext.Session.SetString("verificado", "");

				//Comprobamos si coincide con la de la base de datos
                if (usuario.Contrasenia_usuario!=usuarioBD.Contrasenia_usuario)
				{
                    Util.EscribirEnElFichero("Un usuario intento acceder a la web y puso datos que no se asocian con ninguna cuenta");
                    return false;
				}
				//Si coincide
				else
				{
					//Comprobamos si no esta de alta
                    if (!usuarioBD.Alta_usuario)
                    {
                        Util.EscribirEnElFichero("Un usuario intento logearse sin haber dado de alta la cuenta");
						//Ponemos la variable a false
                        httpContext.Session.SetString("verificado", "false");
                        return false;
                    }
					//Comprobamos si esta de baja
					if (usuarioBD.Fecha_baja!=null)
					{
                        Util.EscribirEnElFichero("Un usuario intento logearse pero esta dado de baja");
                        //Ponemos la variable a false
                        return false;
                    }

                    Util.EscribirEnElFichero("Un usuario accedio correctamente a la web: "+usuarioBD.Nombre_usuario);
                    return true;
				}

            }
			catch (HttpRequestException e)
			{
                Util.EscribirEnElFichero($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
                Util.EscribirEnElFichero($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public bool RecuperarContrasenya(UsuarioDTO usu)
		{
			//Declaramos lo que necesitemos
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
					{
                        Util.EscribirEnElFichero("Se envio un correo para cambiar una contraseña: "+usu.Nombre_usuario);
                        return true;
                    }
					//Si no se envio bien se avisa al usuario
					else
					{
                        Util.EscribirEnElFichero("Hubo un error para enviar un correo a un usuario");
                        return false;
                    }
				}
                Util.EscribirEnElFichero("Hubo problemas para insertar el token");
                return false;

            }
			catch (JsonException e)
			{
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Se produjo un error al crear el flujo de salida. |" + e);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Ya se ha utilizado el método para insertar el usuario. |" + e);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)
			{
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] " + e.Message);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] " + e.Message);
			}
			catch (SecurityException s)
			{
                Util.EscribirEnElFichero(s.Message);
                Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
                Util.EscribirEnElFichero(io.Message);
                Console.Error.WriteLine(io.Message);
			}
			return false;
		}


	

		// Método cambiar
		public async Task<bool> CambiarContrasenia(String contrasenia, TokenDTO to)
		{
			//Declaramos lo que necesitemos
			accionesCRUD acciones= new accionesCRUD();
			UsuarioDTO usuario = null;

            try
			{
				//Buscamos al usuario por el id de usuario del token
				usuario = acciones.SeleccionarUsuario(to.Id_usuario.ToString());

				//Si no lo encuentra
				if (usuario == null)
				{
                    Util.EscribirEnElFichero("Error al cambiar la contarseña no se encontro al usuario");
                    return false;
                }

				//Si esta se le cambia la contraseña
				usuario.Contrasenia_usuario= contrasenia;

				//Se actualiza en la base de datos y se comprueba que este bien
				if (acciones.ActualizarUsuario(usuario))
				{
                    Util.EscribirEnElFichero("Se actualizo la contraseña de un usuario: "+usuario.Nombre_usuario);
                    return true;
				}
                Util.EscribirEnElFichero("No se actualizo bien el usuario en cambiar la contraseña");
                return false;
			}
			catch (HttpRequestException e)
			{
                Util.EscribirEnElFichero($"Error de solicitud HTTP: {e.Message}");
                Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
                Util.EscribirEnElFichero($"Error al deserializar JSON: {e.Message}");
                Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public async Task<TokenDTO> ObtenerToken(string valorTk)
		{
			//Declaramos lo que necesitemos
			accionesCRUD acciones = new accionesCRUD();

			try
			{
				//Buscamos el token y lo devolvemos
                TokenDTO tokenEncontrado = acciones.SeleccionarToken("token/" + valorTk);
				return tokenEncontrado;
			}
			catch (HttpRequestException e)
			{
                Util.EscribirEnElFichero($"Error de solicitud HTTP: {e.Message}");
                Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return null;
			}
			catch (JsonException e)
			{
                Util.EscribirEnElFichero($"Error al deserializar JSON: {e.Message}");
                Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return null;
			}
		}

        public bool EnviarCorreoConToken(UsuarioDTO usu)
		{
			//Declaramos lo que encesitemos
			accionesCRUD acciones = new accionesCRUD();

            // Creamos el token
            // Primero creamos la fecha limite
            DateTime fechaLimite = DateTime.Now.AddMinutes(10);

            // Ahora creamos el token
            Guid guid = Guid.NewGuid();

            // Convertir el GUID a una cadena (string)
            string token = guid.ToString();
            Correo c = new Correo();
            //Creamos el token
            TokenDTO tokenDto = new TokenDTO(token, usu.Id_usuario, fechaLimite);

			//Creamos el mensaje de correo de alta
            String mensaje = c.MensajeCorreoAlta(token, "https://localhost:7048/RegistroControlador/altaHecha");

            //Comprobamos si se ha enviado bien el correo
            if (c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Alta del correo", "isidro@isidrocamachodiaz.es", true))
            {
                //Insertamos en el token
                acciones.InsertarToken(tokenDto);
                Util.EscribirEnElFichero("Se hizo una peticion de alta y se envio correctamente el correo y se actualizo el usuario: "+usu.Nombre_usuario);
                return true;
            }
			//Si no se envia se devuelve false
            else
            {
                Util.EscribirEnElFichero("No se pudo enviar un correo para un alta");
                return false;
            }
        }

    }
}
