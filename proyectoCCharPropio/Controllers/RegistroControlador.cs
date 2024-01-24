using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pruebaRazor.DTOs;
using System.Web;

namespace pruebaRazor.Controllers
{
	[Controller]
	public class RegistroControlador : Controller
	{
		// Acción para la vista de bienvenida
		public IActionResult Bienvenida()
		{
			try
			{
				// AQUÍ VA EL CONTROL DE SESIÓN
				string dni = String.Empty;
				string acceso = String.Empty;
				dni = HttpContext.Session.GetString("dni");
				acceso = HttpContext.Session.GetString("acceso");

				if (acceso != "1")
				{
					MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder","error");
					return RedirectToAction("Index");
				}
			}
			catch (Exception e)
			{
				MostrarAlerta("¡Alerta De Seguridad!","Usted tiene que iniciar Sesion Para Poder acceder","error");
				return RedirectToAction("Index");
			}

			// Verificar si el objeto se recuperó correctamente

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
            return View();
        }
        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult ModificarPerfil()
        {
            return View();
        }
        // Acción HTTP GET para la vista principal
        [HttpGet]
        public IActionResult Contrasenia()
        {
            return View();
        }

        // Acción HTTP GET para la vista principal
        [HttpGet]
		public IActionResult VistaRecuperar()
		{
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

				// Redireccionar a la vista de bienvenida
				return RedirectToAction("Bienvenida");
            }	
		}

        [HttpPost]
        public ActionResult LoginUsuario(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO.Correo_usuario == null || usuarioDTO.Contrasenia_usuario == null)
            {
				MostrarAlerta("¡Campos Incompletos!","Hay Campos Vacios", "error");
				return RedirectToAction("Index", new { error = "parametroVacio" });
            }
            else
            {
                ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
                bool ok = implInteraccionUsuario.LoginUsuario(usuarioDTO).Result;
                if (ok)
                {

					//HttpContext.Session.SetString("correo", usuarioDTO.Dni_usuario);
					//HttpContext.Session.SetString("acceso", "1");


					return RedirectToAction("Bienvenida");
                }
                else
                {
					MostrarAlerta("¡Datos Incorrectos!","El DNI y/o Clave son incorrectos", "error");
					return RedirectToAction("Index", new { error = "parametrosIncorrectos" });
				}
            }
        }

		[HttpPost]
		public async Task<ActionResult> RecuperarContrasenya(UsuarioDTO usuarioDTO)
		{
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
					usu = await implInteraccionUsuario.ObtieneUsuarioPorGmail(usuarioDTO);
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
		public async Task<ActionResult> ModificarContrasenya(UsuarioDTO usuario)
		{
			ImplementacionInteraccionUsuario implInteraccionUsuario = new ImplementacionInteraccionUsuario();
			// Verificar si HttpContext está disponible (para aplicaciones web ASP.NET)
			Console.WriteLine("Contrasenya 1: " + usuario.Contrasenia_usuario);
			Console.WriteLine("Contrasenya 2: " + usuario.Correo_usuario);
			Console.WriteLine("Token: " + usuario.Nombre_usuario);
			bool ok1 = false;
			TokenDTO token = null;
			try
			{
				token = await implInteraccionUsuario.ObtenerToken(usuario);
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR-RegistroControlador-RecuperarContrasenya] Error: " + e.Message);
			}
			Console.WriteLine(token.Id_usuario);

			// Comprobamos si la fecha limite del token es igual o mayor que la hora actual
			// Si es mayor que la hora actual, quiere decir que el token ha caducado.
			DateTime horaActual = DateTime.Now;
			DateTime fechaToken = token.Fecha_limite_token;

			bool ok = false;
			if(fechaToken > horaActual)
			{
				// El token sigue siendo válido
				ok = implInteraccionUsuario.CambiarContrasenia(Util.EncriptarContra(usuario.Contrasenia_usuario), token).Result;
			}
			else
			{
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
