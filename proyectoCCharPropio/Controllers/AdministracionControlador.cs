using Microsoft.AspNetCore.Mvc;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;
using pruebaRazor.DTOs;
using static proyectoCCharPropio.Controllers.RegistroControlador;

namespace proyectoCCharPropio.Controllers
{
    [Controller]
    public class AdministracionControlador : Controller
    {
        public class ModeloAdministracion
        {
            public List<UsuarioDTO> ListaUsuarios { get; set; }
            public UsuarioDTO Usuario { get; set; }
        }
        //Metodo Para Mostrar Alerta
        public IActionResult MostrarAlerta(string titulo, string texto, string tipoDeNotificacion)
        {
            string icono = "";

            if (tipoDeNotificacion == "success")
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


        [HttpGet]
        public IActionResult AdminsitracionUsuarios()
        {
            UsuarioDTO usuario;
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                accionesCRUD acciones = new accionesCRUD();
                acceso = HttpContext.Session.GetString("acceso");
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "4")
                {
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("home");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index");
            }

            var modelo = new ModeloAdministracion
            {
                Usuario= usuario,
            };

            Util.EscribirEnElFichero("Se le llevo a la administracion de usuarios");
            return View(modelo);
        }


        [HttpPost]
        public ActionResult BorrarUsuario(UsuarioDTO usuarioDTO)
        {
           return View(usuarioDTO);
        }

    }
}
