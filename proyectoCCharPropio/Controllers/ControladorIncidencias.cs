using System;
using System.Collections.Generic;
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

        public class ModeloPanelIncidencias
        {
            public UsuarioDTO Usuario { get; set; }

            public List<IncidenciaDTO> incidenciasSinAsignar { get; set; }
            public List<IncidenciaDTO> incidenciasMias { get; set; }
            public List<IncidenciaDTO> incidenciasFinalizadas { get; set; }
        }

        public class ModeloIncidenciasModificar
        {
            public UsuarioDTO Usuario { get; set; }

            public SolicitudDTO solicitudModificar { get; set; }

        }

        public class ModeloModificarIncidencia
        {
            public UsuarioDTO Usuario { get; set; }

            public IncidenciaDTO incidenciaModificar { get; set; }

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
                if (usuario == null)
                {
                    Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }
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
        public IActionResult ListaIncidencia()
        {
            //Declaramos loq ue necesitamos
            UsuarioDTO usuario;
            accionesCRUD acciones = new accionesCRUD();
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                acceso = HttpContext.Session.GetString("acceso");

                if(acceso==null) {
                    Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }

                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);
                AccesoDTO accesoO = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

                if (accesoO.CodigoAcceso1 != "Empleado"&& accesoO.CodigoAcceso1 != "Administrador")
                {
                    Util.EscribirEnElFichero("Un usuario intento acceder a las incidencias");
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
            //Cogemos la lista de todas las incidencias y la filtramos por las del usuario
            List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");
            List<IncidenciaDTO> incidenciasSinAsignar= new List<IncidenciaDTO>();
            List<IncidenciaDTO> incidenciasMias= new List<IncidenciaDTO>();
            List < IncidenciaDTO > incidenciasAcabadas= new List<IncidenciaDTO>();

            //Vamos filtrando por cada lista segun el estado y la asignacion de empleados
            foreach (IncidenciaDTO inc in incidencias)
            {
                //Si no esta asifando
                if (inc.Usuario == null)
                {
                    incidenciasSinAsignar.Add(inc);
                }
                //Si es del usuario
                else if (inc.Usuario.Id_usuario == usuario.Id_usuario&&inc.EstadoIncidencia==false)
                {
                    incidenciasMias.Add(inc);
                }
                //Esta acabada
                else if(inc.EstadoIncidencia == true)
                {
                    incidenciasAcabadas.Add(inc);
                }
            }

            //Las metemos en el modelo
            var modelo = new ModeloPanelIncidencias
            {
                Usuario = usuario,
                incidenciasSinAsignar = incidenciasSinAsignar,
                incidenciasMias = incidenciasMias,
                incidenciasFinalizadas=incidenciasAcabadas

             };

            Util.EscribirEnElFichero("Se le llevo a mostar las incidencias");
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
                if (acceso == null)
                {
                    Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }
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

            //Cojemos la url entera
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor del id de la solicitud
            string idSolicitud = HttpUtility.ParseQueryString(uri.Query)["idS"];

            //Cogemos la solicitud
            SolicitudDTO solicitudModificar = acciones.SeleccionarSolicitud(idSolicitud);

            //Comprobamos si lo encontro
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

            Util.EscribirEnElFichero("Se le llevo a modificar solicitud");
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
                if (acceso == null)
                {
                    Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }
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
            try { 
            //Declaramos lo que encesitemos
            accionesCRUD acciones = new accionesCRUD();

            //Le añadimos los valores
            solicitud.Estado2 = false;
            solicitud.FechaLimite2 = DateTime.Now;

            //Cogemos el id del usuario para buscarlo en la base d edatos y asignarle la solicitud
            string idUsuario = HttpContext.Session.GetString("usuario");

            solicitud.IdUsuario2 = acciones.SeleccionarUsuario(idUsuario);

            implementacionIncidencias implInci = new implementacionIncidencias();

            //Comprobamos si se inserto bien
            if (implInci.crearSolicitud(solicitud))
            {
                Util.EscribirEnElFichero("Un usuario creo una solicitud");
                MostrarAlerta("Solicitud Creada", "Solicitud Generada Correctamente nuestros tecnico se pondran con su solicitud", "success");
                return RedirectToAction("Home", "RegistroControlador");
            }
            //Si no se creo bien se avisa al usuario
            else
            {
                Util.EscribirEnElFichero("Un usuario quizo crear una solicitud pero no se pudo crear");
                MostrarAlerta("Error", "No se pudo crear su solicitud intentelo de nuevo mas tarde", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }

            }catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en crear solicitud " + e.Message);
                try
                {
                    return RedirectToAction("Home", "RegistroControlador");
                }
                catch (IOException e1)
                {
                }
            }
            return RedirectToAction("Home", "RegistroControlador");

        }

        [HttpPost]
        public ActionResult ModificarSolicitud(SolicitudDTO solicitud)
        {
            try { 
                //Declaramos lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();
                bool cambio = false;

                //Comprobamos si escribio algo si no escribio se avisa al usuario
                if (solicitud.DescripcionSolicitud2 == null || solicitud.DescripcionSolicitud2 == "")
                {
                    MostrarAlerta("Campo Vacio", "No puso nada en la descripcion", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                SolicitudDTO solicitudBD = acciones.SeleccionarSolicitud(solicitud.IdSolicitud2.ToString());

                List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");

                //Comprobamos si es distinta a la anterior
                if (solicitud.DescripcionSolicitud2 != solicitudBD.DescripcionSolicitud2)
                {
                    //Se loa signamos
                    solicitudBD.DescripcionSolicitud2 = solicitud.DescripcionSolicitud2;

                    //Buscamos la incidencia que tenga la solicitud y le actualizamos la descripcion del usuario
                    foreach (IncidenciaDTO inc in incidencias)
                    {
                        if (inc.solicitud.IdSolicitud2 == solicitudBD.IdSolicitud2)
                        {
                            inc.DescripcionUsuario = solicitud.DescripcionSolicitud2;
                            acciones.ActualizarIncidencia(inc);
                            cambio = true;
                        }
                    }

                }

                //Comprobamos si cambio los valores
                if (cambio)
                {
                    Util.EscribirEnElFichero("Un usuario actualizo una solicitud");
                    acciones.ActualizarSolicitud(solicitudBD);
                    MostrarAlerta("Solicitud Modificada", "Solicitud Modificada Correctamente nuestros tecnico se pondran con su solicitud", "success");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Si no cambio la descripcion se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Un usuario quizo cambiar una solicitud pero no cambio la descripcion");
                    MostrarAlerta("Es igual", "No modifico la descripcion", "info");
                    return RedirectToAction("Home", "RegistroControlador");
                }

             }catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en modificar solicitud " + e.Message);
                try
                {
                    return RedirectToAction("Home", "RegistroControlador");
                }
                catch (IOException e1)
                {
                }
            }
            return RedirectToAction("Home", "RegistroControlador");
        }

        [HttpGet]
        public IActionResult ModificarIncidencia()
        {
            //Declaramos loq ue necesitamos
            UsuarioDTO usuario;
            accionesCRUD acciones = new accionesCRUD();
            try
            {
                // AQUÍ VA EL CONTROL DE SESIÓN
                string acceso = String.Empty;
                acceso = HttpContext.Session.GetString("acceso");
                if (acceso == null)
                {
                    Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);
                AccesoDTO accesoO = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

                if (accesoO.CodigoAcceso1 != "Empleado"&& accesoO.CodigoAcceso1 != "Administrador")
                {
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                    Util.EscribirEnElFichero("Un usuario intento modificar una solicitud");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index", "RegistroControlador");
            }

            //Cojemos la url entera
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor del id de la solicitud
            string idIncidencia = HttpUtility.ParseQueryString(uri.Query)["idI"];

            //Cogemos la solicitud
            IncidenciaDTO incidenciaModificar = acciones.SeleccionarIncidencia(idIncidencia);

            //Comprobamos si lo encontro
            if (incidenciaModificar == null)
            {
                MostrarAlerta("¡Hubo un problema!", "No se encontro su solicitud", "error");
                Util.EscribirEnElFichero("Un usuario quizo modificar una solicitud `pero no se encontro");
                return RedirectToAction("Home", "RegistroControlador");
            }

            //Comprobamos si la incidencia es del usuario
            if (incidenciaModificar.Usuario.Id_usuario != usuario.Id_usuario)
            {
                MostrarAlerta("¡Hubo un problema!", "Esta incidencia no le pertenece", "error");
                Util.EscribirEnElFichero("Un usuario quizo modificar una incidencia pero no le pertence");
                return RedirectToAction("Home", "RegistroControlador");
            }

            //Lo metemos en el modelo
            var modelo = new ModeloModificarIncidencia
            {
                Usuario = usuario,
                incidenciaModificar=incidenciaModificar
            };

            Util.EscribirEnElFichero("Se le llevo a modificar incidencia");
            return View(modelo);
        }

        [HttpPost]
        public ActionResult ModificarIncidencia(IncidenciaDTO incidencia)
        {
            try
            {
                //Declaramos lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();
                bool cambio = false;

                //Comprobamos si escribio algo si no escribio se avisa al usuario
                if (incidencia.DescripcionTecnica == null || incidencia.DescripcionTecnica == "")
                {
                    MostrarAlerta("Campo Vacio", "No puso nada en la descripcion", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                IncidenciaDTO incidenciaBD = acciones.SeleccionarIncidencia(incidencia.IdIncidencia.ToString());

                //Comprobamos si es distinta a la anterior
                if (incidencia.DescripcionTecnica != incidenciaBD.DescripcionTecnica)
                {
                    incidenciaBD.DescripcionTecnica = incidencia.DescripcionTecnica;
                    cambio = true;
                }

                //Comprobamos si cambio los valores
                if (cambio)
                {
                    Util.EscribirEnElFichero("Un usuario actualizo una incidencia");
                    acciones.ActualizarIncidencia(incidenciaBD);
                    MostrarAlerta("Incidencia Modificada", "Se ha actualizado la incidencia correctamente", "success");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Si no cambio la descripcion se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Un usuario quizo cambiar una incidencia pero no cambio la descripcion");
                    MostrarAlerta("Es igual", "No modifico la descripcion", "info");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en modificar incidencia " + e.Message);
                try
                {
                    return RedirectToAction("Home", "RegistroControlador");
                }
                catch (IOException e1)
                {
                }
            }
            return RedirectToAction("Home", "RegistroControlador");

        }
        [HttpPost]
        public ActionResult AsignarIncidencia(string id,string idI)  
        {
            try
            {
                //Declaramos lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();

                //Cogemos los valores del formulario y los buscamos en al  bd
                IncidenciaDTO incidenciaBD = acciones.SeleccionarIncidencia(idI);
                UsuarioDTO usuarioBD = acciones.SeleccionarUsuario(id);

                //Comprobamos si  se encontro
                if (incidenciaBD == null | usuarioBD == null)
                {
                    MostrarAlerta("No se Encontro", "No se encontro el usuario o la incidencia", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Le asignamos el usuario y la fecha de inicio
                incidenciaBD.Usuario = usuarioBD;
                incidenciaBD.FechaInicio = DateTime.Now;

                //Comprobamos si cambio los valores
                if (acciones.ActualizarIncidencia(incidenciaBD))
                {
                    Util.EscribirEnElFichero("Un usuario se asigno una incidencia");
                    MostrarAlerta("Incidencia Modificada", "Se ha actualizado la incidencia correctamente", "success");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Si no cambio la descripcion se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Un usuario quizo asignarse una incidencia pero no se pudo actualizar");
                    MostrarAlerta("No Se Asigno", "No se pudo asignar la incidencia", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }

            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en asignar incidencia " + e.Message);
                try
                {
                    return RedirectToAction("Home", "RegistroControlador");
                }
                catch (IOException e1)
                {
                }
            }
            return RedirectToAction("Home", "RegistroControlador");


        }

        [HttpPost]
        public ActionResult FinalizarIncidencia(IncidenciaDTO incidencia)
        {
            try
            {
                //Declaramos lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();

                //Cogemos el trabajo completo
                incidencia = acciones.SeleccionarIncidencia(incidencia.IdIncidencia.ToString());

                //Comprobamos si es null algo
                if (incidencia == null)
                {
                    MostrarAlerta("No Se Finalizo", "No se encontro al usuario o la incidencia", "error");
                    Util.EscribirEnElFichero("Un usuario quiso finalizar una incidencia pero no se encontro el usuario o la incidencia");
                    return RedirectToAction("Home", "RegistroControlador");
                    
                }

                //Cogemos todos los trabajos y los filtramos por los de la incidencia
                List<TrabajoDTO> trabajos = acciones.HacerGetLista<TrabajoDTO>("api/Trabajo");
                incidencia.Trabajos=new List<TrabajoDTO>();

                foreach (TrabajoDTO tra in trabajos)
                {
                    if (tra.incidencia.IdIncidencia == incidencia.IdIncidencia)
                    {
                        incidencia.Trabajos.Add(tra);
                    }
                }


                //Comprobamos si tienes trabajos asignados
                if (incidencia.Trabajos == null || incidencia.Trabajos.Count==0)
                {
                    MostrarAlerta("No Se Finalizo", "La incidencia indicada no tiene ningun trabajo hecho", "error");
                    Util.EscribirEnElFichero("Un usuario quiso finalizar una incidencia pero la incidencia no tiene ningun trabajo hecho");
                    return RedirectToAction("Home", "RegistroControlador");
                    
                }

                implementacionIncidencias impl= new implementacionIncidencias();

                //Comprobamos si lo finaliza bien
                if (impl.FinalizarIncidencia(incidencia))
                {
                    MostrarAlerta("Incidencia Finalizada", "La incidencia se finalizo correctamente", "success");
                    Util.EscribirEnElFichero("Un usuario finalizo un incidencia");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Si no conseigue finalizarlo se avisa al usuario
                else
                {
                    MostrarAlerta("No Se Finalizo", "No se pudo finalizar la incidencia", "error");
                    Util.EscribirEnElFichero("Un usuario quiso finalizar ua incdencia pero no se pudo finalizar");
                    return RedirectToAction("Home", "RegistroControlador");
                }
               

            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en finalizar trabajo " + e.Message);
                try
                {
                    return RedirectToAction("Home", "RegistroControlador");
                }
                catch (IOException e1)
                {
                }
            }
            return RedirectToAction("Home", "RegistroControlador");
        }

    }
}
