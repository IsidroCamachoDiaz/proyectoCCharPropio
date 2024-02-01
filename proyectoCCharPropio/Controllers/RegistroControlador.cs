﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyectoCCharPropio.DTOS;
using System.Web;
using System;
using proyectoCCharPropio.Recursos;
using pruebaRazor.DTOs;

namespace proyectoCCharPropio.Controllers
{
	[Controller]
	public class RegistroControlador : Controller
	{

        // Modelo para la vista
        public class ModificarViewModel
        {
            public string Token { get; set; }
        }
        // Acción para la vista de bienvenida
        public IActionResult Bienvenida()
		{
            try
			{
				// AQUÍ VA EL CONTROL DE SESIÓN
				string dni = String.Empty;
				string acceso = String.Empty;
				acceso = HttpContext.Session.GetString("acceso");

				if (acceso != "2"&& acceso != "3"&&acceso != "4")
				{
                    Util.EscribirEnElFichero("Un usuario intento acceder a un sitio que no se le permite");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder","error");
					return RedirectToAction("Index");
				}
			}
			catch (Exception e)
			{
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!","Usted tiene que iniciar Sesion Para Poder acceder","error");
				return RedirectToAction("Index");
			}

            // Verificar si el objeto se recuperó correctamente
            Util.EscribirEnElFichero("Se le llevo a la bienvenida");
            return View();
		}
		//Metodo Para Mostrar Alerta
		public IActionResult MostrarAlerta(string titulo,string texto,string tipoDeNotificacion)
		{
			string icono = "";

			if (tipoDeNotificacion=="success")
			{
				icono = "check";
			}

			TempData["MensajeAlerta"] = texto;
			TempData["MostrarAlerta"] = true;
			TempData["Tipo"] = tipoDeNotificacion;
			TempData["Icono"] = icono;
			TempData["Titulo"] = titulo;

			return RedirectToAction("Index");
		}

		// Acción HTTP GET para la vista principal
		[HttpGet]
        public IActionResult Index()
        {
            Util.EscribirEnElFichero("Se llevo al index");
            return View();
        }
        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult ModificarPerfil()
        {
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                acceso = HttpContext.Session.GetString("acceso");

                if (acceso != "2" && acceso != "3" && acceso != "4")
                {
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index");
            }

            string idUsuario = HttpContext.Session.GetString("usuario");
			accionesCRUD acciones =new accionesCRUD();
			UsuarioDTO usuario = acciones.SeleccionarUsuario(idUsuario);

            Util.EscribirEnElFichero("Se le llevo al modificar perfil");
            return View(usuario);
        }
        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult Contrasenia()
        {
            Util.EscribirEnElFichero("Se le llevo a la pestaña de olvidar contraseña");
            return View();
        }

        // Acción HTTP GET para la vista principal
        [HttpGet]
		public IActionResult Modificar()
		{
            //Cojo la url del navegador
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor tk
            string valorTk = HttpUtility.ParseQueryString(uri.Query)["tk"];

			// Crear un modelo y establecer el valor del token
			var modelo = new ModificarViewModel
			{
				Token = valorTk
            };

            // Pasar el modelo a la vista
            Util.EscribirEnElFichero("Se le llevo a la vista de modificar contraseña");
            return View(modelo);
		}

        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult Home()
        {
			UsuarioDTO usuario;
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
				accionesCRUD acciones=new accionesCRUD();
                acceso = HttpContext.Session.GetString("acceso");
				string idUsuario= HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "2" && acceso != "3" && acceso != "4")
                {
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index");
            }
            Util.EscribirEnElFichero("Se le llevo al home");
            return View(usuario);
        }

        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult AltaHecha()
        {
            //Cojo la url del navegador
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
			//Cojo el valor tk
            string valorTk = HttpUtility.ParseQueryString(uri.Query)["tk"];
            accionesCRUD acciones=new accionesCRUD();
			//Cojo el token de la base de datos
			TokenDTO token=acciones.SeleccionarToken("token/"+valorTk);
			//Cojemos el usuario a partir del id del usuario del token 
			UsuarioDTO usuario = acciones.SeleccionarUsuario(token.Id_usuario.ToString());
			//Cojo la fecha de ahora para ver si ha pasado eol tiempo de espera
			DateTime ahora = DateTime.Now;
            if(ahora>token.Fecha_limite_token)
			{
				//Si paso el tiempo se elimina la cuenta para que vuelva a crearla
				acciones.EliminarToken(token.Id_token_tabla.ToString());
				acciones.EliminarUsuario(usuario.Id_usuario.ToString());
                MostrarAlerta("Tiempo Agotado", "Se le agoto el tiempo para verificar se elimino la cuenta vuelva a registrarse", "error");
                Util.EscribirEnElFichero("Una persona hizo el alta tarde y se borro la cuenta y el token :"+usuario.Nombre_usuario);
                return RedirectToAction("index");
            }
			else
			{
				usuario.Alta_usuario = true;
				acciones.ActualizarUsuario(usuario);
                Util.EscribirEnElFichero("Un usuario hizo el alta correctamente: "+usuario.Nombre_usuario);
                MostrarAlerta("Confirmado", "Su cuenta ha sido Verificada", "success");
			}

            return View();
        }

        // Acción HTTP POST para el registro de usuarios
        [HttpPost]
		public ActionResult RegistroUsuario(UsuarioDTO usuarioDTO, IFormFile archivo)
		{
			// Verificar si algún parámetro requerido está vacío
			if ( usuarioDTO.Nombre_usuario == null || usuarioDTO.Telefono_usuario == null || usuarioDTO.Correo_usuario == null || usuarioDTO.Contrasenia_usuario == null)
			{
				MostrarAlerta("¡Campos Incompletos!", "Hay Campos Vacios", "error");
				return RedirectToAction("Index", new { error = "parametroVacio" });
            } 
			else
			{
				//Convierto el archivo en array de byte
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    archivo.CopyTo(memoryStream);
                    usuarioDTO.Foto_usuario = memoryStream.ToArray();
                }
                // Crear instancia de la implementación de interacción con el usuario
                ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();

				// Llamar al método para registrar el usuario
				if (implInteraccionUsuario.RegistrarUsuario(usuarioDTO))
				{
					MostrarAlerta("Registro Completo", "Se le ha enviado un correo para verificar su identidad", "success");
				}
				else
				{
                    MostrarAlerta("Error", "Hubo un error intentelo mas tarde", "error");
                }

				//HttpContext.Session.SetString("correo", usuarioDTO.Dni_usuario);
				//HttpContext.Session.SetString("acceso", "1");

				// Redireccionar a la vista de index
				return RedirectToAction("index");
            }	
		}

        [HttpPost]
        public ActionResult LoginUsuario(UsuarioDTO usuarioDTO)
        {
			try
			{
				//Comprobamos que no hay metido datos vacios
				if (usuarioDTO.Correo_usuario == null || usuarioDTO.Contrasenia_usuario == null)
				{
					MostrarAlerta("¡Campos Incompletos!", "Hay Campos Vacios", "error");
					return RedirectToAction("Index", new { error = "parametroVacio" });
				}
				else
				{
					ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
					accionesCRUD acciones = new accionesCRUD();
					bool ok = implInteraccionUsuario.LoginUsuario(usuarioDTO, HttpContext).Result;
					if (ok)
					{

						// Aquí usamos ViewBag para pasar el modelo a la vista
						ViewBag.UsuarioDTO = usuarioDTO;
						usuarioDTO = acciones.SeleccionarUsuario("Correo/" + usuarioDTO.Correo_usuario);
						//return View("Home", usuarioDTO);
						return RedirectToAction("Home");
					}
					else
					{
                        string verificado = HttpContext.Session.GetString("verificado");
                        if (verificado=="false")
						{
                            MostrarAlerta("¡No esta dado de alta!", "Tiene que darse de alta en la aplicacion", "error");
                            return RedirectToAction("Index", new { error = "parametrosIncorrectos" });
                        }

						MostrarAlerta("¡Datos Incorrectos!", "El Correo y/o Clave son incorrectos", "error");
						return RedirectToAction("Index", new { error = "parametrosIncorrectos" });
					}
				}
			}catch(Exception e)
			{
                MostrarAlerta("¡Hubo un erro!", "Hubo un erroe intentelo mas tarde", "error");
                return RedirectToAction("Index", new { error = "parametrosIncorrectos" });
            }
        }

		[HttpPost]
		public async Task<ActionResult> RecuperarContrasenya(UsuarioDTO usuarioDTO)
		{
			accionesCRUD acciones = new accionesCRUD(); 
			Console.WriteLine(usuarioDTO.Correo_usuario);
			if (usuarioDTO.Correo_usuario == null)
			{
				return RedirectToAction("Index", new { error = "parametroVacio" });
			}
			else
			{
				// Inicializamos la implementacion para tener los métodos
				ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
				UsuarioDTO usu = null;
				try
				{
					usu = acciones.SeleccionarUsuario("correo/"+usuarioDTO.Correo_usuario);
				}
				catch (Exception e)
				{
					Console.WriteLine("[ERROR-RegistroControlador-RecuperarContrasenya] Error: " + e.Message);
				}
				
				if (usu != null)
				{
					// Si tenemos el usuario tendremos que coger el id del usuario y enviar a la tabla token_tabla
					// El token generado, una fecha limite de 10 minutos y el id del usuario
					bool ok = implInteraccionUsuario.RecuperarContrasenya(usu);
					if (ok)
					{
						MostrarAlerta("¡Revise su Bandeja!","Se le ha enviado un correo para cambiar la clave", "success");
						return RedirectToAction("Index", new { bien = "emailExiste" });
					}
					else
					{
						MostrarAlerta("¡Hubo Un Error!","Ha habido un error Por Favor Intentelo En Otro Momento", "error");
						return RedirectToAction("Index", new { error = "errorNoSeHaHechoPost" });
					}
				} 
				else
				{
					MostrarAlerta("¡Correo No Existe!","El email Proporcionado no existe", "error");
					return RedirectToAction("Index", new { error = "emailNoExiste" });
				}
			}
		}

		// Método modificar contrasenya
		[HttpPost]
		public async Task<ActionResult> ModificarContrasenya(UsuarioDTO usuario, ModificarViewModel m)
		{
			ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
			//Cogemos el valor del modelo
			string contrasenia1 = Util.EncriptarContra(usuario.Contrasenia_usuario);
            string contrasenia2 = Util.EncriptarContra(usuario.Correo_usuario);

            string valorTk=m.Token.ToString();
			TokenDTO token = null;
			try
			{
				//Cogemos el token que le pertenece
				token = await implInteraccionUsuario.ObtenerToken(valorTk);
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR-RegistroControlador-RecuperarContrasenya] Error: " + e.Message);
			}

			// Comprobamos si la fecha limite del token es igual o mayor que la hora actual
			// Si es mayor que la hora actual, quiere decir que el token ha caducado.
			try
			{

				DateTime horaActual = DateTime.Now;
				DateTime fechaToken = token.Fecha_limite_token;

				bool ok = false;
				if (fechaToken > horaActual)
				{
					if (contrasenia1 != contrasenia2)
					{

                        Util.EscribirEnElFichero("Un usuario no puse bien las 2 contraseñas");
                        MostrarAlerta("¡Incorrecta las contraseñas!", "No puso bien las contraseñas iguales", "error");
                        return RedirectToAction("Index", new { error = "tokenCaducado" });
                    }
					// El token sigue siendo válido
					ok = await implInteraccionUsuario.CambiarContrasenia(contrasenia1, token);
                }
				else
				{
                    Util.EscribirEnElFichero("Una persona persona se paso de la fecha limite de cambiar contraseña");
                    MostrarAlerta("¡Demasiado Tarde!", "No se pudo cambiar la clave por paso el limite de la fecha", "error");
					// El token es inválido
					return RedirectToAction("Index", new { error = "tokenCaducado" });
				}

				if (ok)
				{
                    MostrarAlerta("¡Clave Cambiada!", "La clave se Modifico Correctamente", "success");
					return RedirectToAction("Index", new { bien = "claveCambiada" });
				}
				else
				{
                    MostrarAlerta("¡Hubo Un Error!", "Ha habido un error Por Favor Intentelo En Otro Momento", "error");
					return RedirectToAction("Index", new { error = "errorNoSeHaHechoPost" });
				}
			
			}catch (Exception e)
			{
                Util.EscribirEnElFichero("hubo un error en cambiar la contraseña"+e.Message);
                MostrarAlerta("¡Hubo Un Error!", "Ha habido un error Por Favor Intentelo En Otro Momento", "error");
                return RedirectToAction("Index", new { error = "errorNoSeHaHechoPost" });
            }
		}

        [HttpPost]
        public ActionResult ModificarPerfil(UsuarioDTO usuarioDTO, IFormFile archivo)
        {
            ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
            accionesCRUD acciones = new accionesCRUD();
			//Ponemos a nulo para buscar
			UsuarioDTO usuarioBD=null;
            //Comprobamos que no hay metido datos vacios
            if (usuarioDTO.Correo_usuario == null || usuarioDTO.Nombre_usuario == null|| usuarioDTO.Telefono_usuario==null)
            {
                MostrarAlerta("¡Campos Incompletos!", "No puede dejar campos vacios", "error");
                return RedirectToAction("Index", new { error = "parametroVacio" });
            }

			try
			{
				bool cambioCorreo = false;
				bool modifico=false;
				//Buscamos al usuario por el id para actualizarlo
				usuarioBD = acciones.SeleccionarUsuario(usuarioDTO.Id_usuario.ToString());
				//Comprobomos si no lo encontro
                if (usuarioBD == null)
				{
                    MostrarAlerta("No se encontro su usuario", "No se pudo encontrar a su usuario intentelo mas tarde", "error");
                    return RedirectToAction("Index", new { error = "parametroVacio" });
                }
				//Comprobamos si metio una imagen
                if (archivo != null && archivo.Length > 0)
                {
                    //Convierto el archivo en array de byte
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        archivo.CopyTo(memoryStream);
                        usuarioBD.Foto_usuario = memoryStream.ToArray();
                    }
					modifico = true;
                }
				//Comprobamos si cambio algun campo
                if (usuarioDTO.Correo_usuario!=usuarioBD.Correo_usuario|| usuarioDTO.Nombre_usuario != usuarioBD.Nombre_usuario|| 
					usuarioDTO.Telefono_usuario != usuarioBD.Telefono_usuario|| usuarioDTO.Contrasenia_usuario != null)
				{
					//Si puso una contraseña nueva se encripta
					if (usuarioDTO.Contrasenia_usuario != null)
					{
						usuarioBD.Contrasenia_usuario = Util.EncriptarContra(usuarioDTO.Contrasenia_usuario);
					}
					//Comprobamos si cambio el correo
					if(usuarioDTO.Correo_usuario != usuarioBD.Correo_usuario)
					{
						//Le asignamos nuevo correo
						usuarioBD.Correo_usuario = usuarioDTO.Correo_usuario;
						//Cambiamos el alta
						usuarioBD.Alta_usuario = false;
						//Comfirmamos que cambio el correo
						cambioCorreo = true;
                    }
					//Igualamos los valores
					usuarioBD.Telefono_usuario = usuarioDTO.Telefono_usuario;
					usuarioBD.Nombre_usuario = usuarioDTO.Nombre_usuario;
					//Comfirmamos que se modifcaron campos
					modifico = true;
                }
				//Comprobamos si modifico algo
				if (modifico)
				{
					//Comprobamos que se actualice bien el usuario
					if(acciones.ActualizarUsuario(usuarioBD))
					{
						//Coprobamos si cambio el correo
						if (cambioCorreo)
						{
							//Enviamos un nuevo correo de alta y comprobamos si se hizo correctamente
							if (implInteraccionUsuario.EnviarCorreoConToken(usuarioBD))
							{
                                Util.EscribirEnElFichero("Un usuario cambio su correo correctamente: "+usuarioBD.Nombre_usuario);
                                MostrarAlerta("Correo Cambiado", "Revise su bandeja de entrada le hemos enviado un correo", "success");
                                return RedirectToAction("Index", new { error = "parametroVacio" });
                            }
							//Si no se hizo se avisa al usuario
							else
							{
                                Util.EscribirEnElFichero("Hubo un problema para enviar el correo a un usuario");
                                MostrarAlerta("No se pudo Cambiar el correo", "Hubo un error al cambiar el correo intentelo mas tarde", "error");
                                return RedirectToAction("Home");
                            }
						}
						//Si actualizo pero no cambio el correo
						else
						{
                            Util.EscribirEnElFichero("Actulizo bien sus sin cambiar el correo: "+usuarioBD.Nombre_usuario);
                            MostrarAlerta("Se Actualizo Correctamente", "Se modifico los datos correctamente", "success");
                            return RedirectToAction("ModificarPerfil");
                        }
					}
					//Si falla la actualizacion
					else
					{
                        Util.EscribirEnElFichero("Hubo un problema para actualizar a un usuario");
                        MostrarAlerta("No se pudo actualizar su uauario", "No se pudo actualizar a su usuario intentelo mas tarde", "error");
                        return RedirectToAction("ModificarPerfil", new { error = "parametroVacio" });
                    }
				}
				//Si no cambia nada
				else
				{
                    Util.EscribirEnElFichero("Un usuario quizo actualizar sus datos epro no cambio nada");
                    MostrarAlerta("No Modifico Nada", "No ha modificado ningun valor", "info");
                    return RedirectToAction("ModificarPerfil", new { error = "parametroVacio" });
                }
			}
			catch (Exception e)
			{
                Util.EscribirEnElFichero("Hubo un error "+e.Message);
            }

            return RedirectToAction("Index", new { error = "parametrosIncorrectos" });
        }

        [AllowAnonymous]
		public IActionResult ExternalLogin(string provider, string returnUrl = "/")
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action(nameof(ExternalLoginCallback), new { returnUrl }),
			};

			return Challenge(properties, provider);
		}

		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
		{
			var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Obtén información del usuario desde authenticateResult.Principal

			// Puedes redirigir al usuario a la página deseada (returnUrl) o realizar otras acciones según tus necesidades

			return RedirectToAction("Index", "Home");
		}
	}
}
