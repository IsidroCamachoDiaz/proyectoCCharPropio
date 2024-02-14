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

        public class ModeloIncidenciasModificar
        {
            public UsuarioDTO Usuario { get; set; }

            public SolicitudDTO solicitudModificar { get; set; }

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
        public IActionResult ListaSolicitudes()
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
                AccesoDTO accesoO = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

                if (accesoO.CodigoAcceso1!="Usuario")
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
            List<SolicitudDTO> solicitudes = acciones.HacerGetLista<SolicitudDTO>("api/Solicitud");
            solicitudes = solicitudes.Where(x=>x.IdUsuario2.Id_usuario ==usuario.Id_usuario).ToList();

            //Filtramos por si estan terminadas o no
            List<SolicitudDTO> solicitudesHechas =solicitudes.Where(x => x.Estado2 == true).ToList();
            List<SolicitudDTO> solicitudesPendientes=solicitudes.Where(x => x.Estado2 == false).ToList();

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
        public IActionResult ModificarSolicitud()
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
                AccesoDTO accesoO = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

                if (accesoO.CodigoAcceso1 != "Usuario")
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

            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor tk
            string idSolicitud = HttpUtility.ParseQueryString(uri.Query)["idS"];

            SolicitudDTO solicitudModificar = acciones.SeleccionarSolicitud(idSolicitud);

            if (solicitudModificar == null)
            {
                MostrarAlerta("¡Hubo un problema!", "No se encontro su solicitud", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }

            //Las metemos en el modelo
            var modelo = new ModeloIncidenciasModificar
            {
                Usuario = usuario,
                solicitudModificar=solicitudModificar
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
                AccesoDTO accesoO = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

                if (accesoO.CodigoAcceso1 != "Usuario")
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

            //Le añadimos los valores
            solicitud.Estado2 = false;
            solicitud.FechaLimite2=DateTime.Now;

            //Cogemos el id del usuario para buscarlo en la base d edatos y asignarle la solicitud
            string idUsuario = HttpContext.Session.GetString("usuario");

            solicitud.IdUsuario2 = acciones.SeleccionarUsuario(idUsuario);

            implementacionIncidencias implInci = new implementacionIncidencias();
            
            //Comprobamos si se inserto bien
            if (implInci.crearSolicitud(solicitud))
            {
                MostrarAlerta("Solicitud Creada", "Solicitud Generada Correctamente nuestros tecnico se pondran con su solicitud", "success");
                return RedirectToAction("Home", "RegistroControlador");
            }
            //Si no se creo bien se avisa al usuario
            else
            {
                MostrarAlerta("Error", "No se pudo crear su solicitud intentelo de nuevo mas tarde", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }


        }

        [HttpPost]
        public ActionResult ModificarSolicitud(SolicitudDTO solicitud)
        {
            //Declaramos lo que encesitemos
            accionesCRUD acciones = new accionesCRUD();
            bool cambio = false;
            //Comprobamso si escribio algo si no escribio se avisa al usuario
            if (solicitud.DescripcionSolicitud2 == null || solicitud.DescripcionSolicitud2 == "")
            {
                MostrarAlerta("Campo Vacio", "No puso nada en la descripcion", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }
            SolicitudDTO solicitudBD = acciones.SeleccionarSolicitud(solicitud.IdSolicitud2.ToString());
            
            //Comprobamos si es distinta a la anterior
            if (solicitud.DescripcionSolicitud2 != solicitudBD.DescripcionSolicitud2)
            {
                solicitudBD.DescripcionSolicitud2 = solicitud.DescripcionSolicitud2;
                solicitudBD.Incidencia2 = solicitud.Incidencia2;
                solicitudBD.Incidencia2.Descripcion_Usuario = solicitud.DescripcionSolicitud2;
                cambio = true;
            }

            //Comprobamos si se inserto bien
            if (cambio)
            {
                acciones.ActualizarSolicitud(solicitudBD);
                MostrarAlerta("Solicitud Modificada", "Solicitud Modificada Correctamente nuestros tecnico se pondran con su solicitud", "success");
                return RedirectToAction("Home", "RegistroControlador");
            }
            //Si no cambio la descripcion se avisa al usuario
            else
            {
                MostrarAlerta("Es igual", "No modifico la descripcion", "info");
                return RedirectToAction("Home", "RegistroControlador");
            }


        }

    }
}
