using System.Web;
using Microsoft.AspNetCore.Mvc;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;
using proyectoCCharPropio.Servicios;
using pruebaRazor.DTOs;
using static proyectoCCharPropio.Controllers.RegistroControlador;

namespace proyectoCCharPropio.Controllers
{
    [Controller]
    public class ControladorIncidencias : Controller
    {
        public class ModeloIncidencias
        {   
            public UsuarioDTO Usuario { get; set; }

            public List<SolicitudDTO> ListaSolicitudesFinalizadas { get; set; }
            public List<SolicitudDTO> ListaSolicitudesPendientes { get; set; }
        }

        public class ModeloUsuario
        {
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
        public IActionResult ListaIncidencias()
        {
            //Declaramos loq ue necesitamos
            UsuarioDTO usuario;
            accionesCRUD acciones = new accionesCRUD();
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                acceso = HttpContext.Session.GetString("acceso");
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "3")
                {
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index", "RegistroControlador");
            }
            //Cogemos la lista de todas las solicitudes y la filtramos por las del usuario
            List<SolicitudDTO> solicitudes = acciones.HacerGetLista<SolicitudDTO>("Solicitud");
            solicitudes = solicitudes.Where(x=>x.IdUsuario==usuario.Id_usuario).ToList();

            //Filtramos por si estan terminadas o no
            List<SolicitudDTO> solicitudesHechas =solicitudes.Where(x => x.Estado == true).ToList();
            List<SolicitudDTO> solicitudesPendientes=solicitudes.Where(x => x.Estado == false).ToList();

            //Las metemos en el modelo
            var modelo = new ModeloIncidencias
            {
                Usuario = usuario,
                ListaSolicitudesFinalizadas = solicitudesHechas,
                ListaSolicitudesPendientes = solicitudesPendientes
            };

            Util.EscribirEnElFichero("Se le llevo a mostar las solicitudes");
            return View(modelo);
        }

        [HttpGet]
        public IActionResult CrearSolicitud()
        {
            //Declaramos lo que necesitemos
            UsuarioDTO usuario;
            accionesCRUD acciones = new accionesCRUD();

            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                acceso = HttpContext.Session.GetString("acceso");
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "3")
                {
                    Util.EscribirEnElFichero("Un usuario intento acceder a un lugar que no puede de la web");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted no puede acceder a este lugar de la web", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }

            //Metemos en el modelo el usuario
            var modelo = new ModeloUsuario
            {
                Usuario = usuario,
            };

            Util.EscribirEnElFichero("Se le llevo a crear solicitud");
            return View(modelo);
        }

        [HttpPost]
        public ActionResult CrearSolicitud(SolicitudDTO solicitud)
        {
            //Declaramos lo que encesitemos
            accionesCRUD acciones = new accionesCRUD();

            solicitud.Estado = false;
            solicitud.FechaLimite=DateTime.Now;

            implementacionIncidencias implInci = new implementacionIncidencias();
            //Usamos el metodo para borrar

            if (implInci.crearSolicitud(solicitud))
            {
                MostrarAlerta("Solicitud Creada", "Solicitud Generada Correctamente nuestros tecnico se pondran con su solicitud", "success");
                return RedirectToAction("AdministracionUsuarios");
            }
            //Si no se pudo se avisa al usuario
            else
            {
                MostrarAlerta("Error", "No se pudo crear su solicitud intentelo de nuevo mas tarde", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }


        }

    }
}
