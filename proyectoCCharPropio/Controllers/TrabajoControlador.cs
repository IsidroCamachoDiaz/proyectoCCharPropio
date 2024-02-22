using System.Net.Mail;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;
using proyectoCCharPropio.Servicios;
using pruebaRazor.DTOs;
using static proyectoCCharPropio.Controllers.ControladorIncidencias;
using static proyectoCCharPropio.Controllers.RegistroControlador;

namespace proyectoCCharPropio.Controllers
{
    [Controller]
    public class TrabajoControlador : Controller
    {
        //Modelo de administracion
        public class ModeloMostrarTrabajos
        {
            public UsuarioDTO Usuario { get; set; }

            public List<IncidenciaDTO> incidencias { get; set; }
        }
        public class ModeloCrearTrabajo
        {
            public UsuarioDTO Usuario { get; set; }

            public List<TipoTrabajoDTO> tipos {get;set;}

            public IncidenciaDTO incidencia { get; set; }
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
        public IActionResult CrearTrabajo()
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

                if (accesoO.CodigoAcceso1 != "Empleado"&& accesoO.CodigoAcceso1 != "Administrador")
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

            //Cojemos la url entera
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor del id de la incidencia
            string idSincidencia = HttpUtility.ParseQueryString(uri.Query)["idI"];

            IncidenciaDTO incidencia=acciones.SeleccionarIncidencia(idSincidencia);

            //Filtramos por las activas
            List<TipoTrabajoDTO> tipos = acciones.HacerGetLista<TipoTrabajoDTO>("api/TiposIncidencia");
            tipos=tipos.Where(x=>x.FechaExpiracion==null).ToList();

            //Metemos en el modelo de creacion de trabajo
            var modelo = new ModeloCrearTrabajo
            {
                Usuario = usuario,
                tipos = tipos,
                incidencia = incidencia                                       
            };

            Util.EscribirEnElFichero("Se le llevo a crear trabajo");
            return View(modelo);
        }



        [HttpGet]
        public IActionResult ListaTrabajos()
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

                if (accesoO.CodigoAcceso1 != "Administrador"&& accesoO.CodigoAcceso1 != "Empleado")
                {
                    Util.EscribirEnElFichero("Un usuario intento acceder a un lugar que no puede de la web");
                    MostrarAlerta("¡Alerta De Seguridad!", "Usted no tiene acceso para entrar aqui", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Una persona que no estaba registrada intento acceder");
                MostrarAlerta("¡Alerta De Seguridad!", "Usted tiene que iniciar Sesion Para Poder acceder", "error");
                return RedirectToAction("Index", "RegistroControlador");
            }

            //Cogemos las incidencias y los trabajos
            List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");
            List <TrabajoDTO> trabajos= acciones.HacerGetLista<TrabajoDTO>("api/Trabajo");

            //Filtramos por las incidencias del usuario
            incidencias=incidencias.Where(x=>x.Usuario!=null).Where(x=>x.Usuario.Id_usuario==usuario.Id_usuario).Where(x=>x.EstadoIncidencia==false).ToList();

            //Filtramos por cada incidencia sus trabajos
            foreach(IncidenciaDTO incidencia in incidencias)
            {
                foreach (TrabajoDTO t in trabajos)
                {
                    if (t.incidencia.IdIncidencia == incidencia.IdIncidencia)
                    {
                        incidencia.Trabajos.Add(t);
                    }
                }
            }
            
            //Metemos todo en el modelo
            var modelo = new ModeloMostrarTrabajos
            {
                Usuario=usuario,
                incidencias= incidencias,

            };

            Util.EscribirEnElFichero("Se le llevo a mostrar trabajos");
            return View(modelo);
        }


        [HttpPost]
        public ActionResult CrearTrabajo(TrabajoDTO trabajo,string id_tipo,string id_incidencia)
        {
            try
            {   
                accionesCRUD acciones = new accionesCRUD();

                IncidenciaDTO inc = acciones.SeleccionarIncidencia(id_incidencia);
                TipoTrabajoDTO tipo = acciones.SeleccionarTipoDeTrabajo(id_tipo);
                //Comprobamos que no haya valores nulos
                if (trabajo.DescripcionTrabajo==null|| trabajo.DescripcionTrabajo==""|| trabajo.HorasTrabajo==0
                   ||inc==null||tipo==null )
                {
                    MostrarAlerta("Valores vacios","No puso todos los campos", "error");
                    Util.EscribirEnElFichero("Un usuario quiso crear un trabajo pero puso valores nulos");
                    return RedirectToAction("Home", "RegistroControlador");
                }

                //Asignamos los objetos y el estado
                trabajo.incidencia = inc;
                trabajo.tipo = tipo;
                trabajo.EstadoTrabajo = false;

                //Declaramos la implementacion
                implementacionTrabajo impl = new implementacionTrabajo();

                try
                {
                    //Comprobamos si se creo bien el trabajo
                    if (impl.crearTrabajo(trabajo))
                    {
                        MostrarAlerta("Trabajo Creado", "El trabajo se creo correctamente", "success");
                        Util.EscribirEnElFichero("Un usuario creo un trabajo");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                    //Si no se creo bien avisamos al usuario
                    else
                    {
                        MostrarAlerta("No se pudo crear", "No se pudo crear el  trabajo", "error");
                        Util.EscribirEnElFichero("Un usuario quiso crear  trabajo pero no se pudo insertar");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                }
                catch (IOException e)
                {
                    MostrarAlerta("Hubo un error", "Hubo un error intentelo de nuevo mas tarde", "error");
                    Util.EscribirEnElFichero("Hubo un error en crear trabajo " + e.Message);
                    return RedirectToAction("Home", "RegistroControlador");
                }

            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en crear trabajo " + e.Message);
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
        public ActionResult FinalizarTrabajo(TrabajoDTO trabajo)
        {
            try
            {
                //Declaramos lo que necesitemos
                accionesCRUD acciones = new accionesCRUD();

                //COgemos el trabajo completo
                trabajo = acciones.SeleccionarTrabajo(trabajo.IdTrabajo.ToString());


                //Comprobamos que no haya valores nulos
                if (trabajo==null)
                {
                    MostrarAlerta("Trabajo No Encontrado", "No se encontro el trabajo en la base de datos", "error");
                    Util.EscribirEnElFichero("Un usuario quiso finalizar un trabajo pero no se encontro el trabajo");
                    return RedirectToAction("Home", "RegistroControlador");
                }

                //La incidencia con el trabajo viene con algunos valores nulso asi que lo cogeremos entero de la base de datos
                trabajo.incidencia = acciones.SeleccionarIncidencia(trabajo.incidencia.IdIncidencia.ToString());

                //Cambio el estado
                trabajo.EstadoTrabajo = true;
                //Le asigno las horas a la incidencia
                trabajo.incidencia.HorasIncidencia = trabajo.incidencia.HorasIncidencia + trabajo.HorasTrabajo;
                
                //Calculo el coste
                float precioTrabajo = (trabajo.HorasTrabajo * trabajo.tipo.PrecioTipo);
                precioTrabajo = (float)Math.Round(precioTrabajo, 2);
                trabajo.incidencia.CosteIncidencia = trabajo.incidencia.CosteIncidencia + precioTrabajo;

                //Declaramos la implementacion
                implementacionTrabajo impl = new implementacionTrabajo();

                try
                {
                    //Comprobamos si se finalizo bien
                    if (impl.finalizarTrabajo(trabajo))
                    {
                        MostrarAlerta("Trabajo Finalizado", "El trabajo se finalizo correctamente", "success");
                        Util.EscribirEnElFichero("Un usuario finalizo un trabajo");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                    //Si no se finaliza bien avisamos al usuario
                    else
                    {
                        MostrarAlerta("No se pudo finalizar", "No se pudo finalizar el  trabajo", "error");
                        Util.EscribirEnElFichero("Un usuario quiso finalizar  trabajo pero no se pudo actualizar");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                }
                catch (IOException e)
                {
                    MostrarAlerta("Hubo un error", "Hubo un error intentelo de nuevo mas tarde", "error");
                    Util.EscribirEnElFichero("Hubo un error en finalizar trabajo " + e.Message);
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
