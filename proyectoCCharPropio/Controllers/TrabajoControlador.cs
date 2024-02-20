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
        public IActionResult CrearTipo()
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

            //Metemos en el modelo el usuario
            var modelo = new ModeloUsuario
            {
                Usuario = usuario,
            };

            Util.EscribirEnElFichero("Se le llevo a crear tipo de trabajo");
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
        public ActionResult FinalizarTipo(TipoTrabajoDTO tipoDTO)
        {
            try
            {
                //Declaramos lo que encesitemos
                accionesCRUD acciones = new accionesCRUD();

                //Cogemos el tipo de la base de datos
                tipoDTO = acciones.SeleccionarTipoDeTrabajo(tipoDTO.IdTipo.ToString());

                //Le asignamos la fecha
                tipoDTO.FechaExpiracion = DateTime.Now;

                //Usamos el metodo para actualizar
                if (acciones.ActualizarTipoDeTrabajo(tipoDTO))
                {
                    Util.EscribirEnElFichero("Se finalizo un tipo de trabajo");
                    MostrarAlerta("Tipo Finalizado", "El tipo de trabajo se finalizo correctamente", "success");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                //Si no se pudo se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Se intento finalizar un tipo pero no se pudo");
                    MostrarAlerta("Error", "No se pudo finalizar el tipo de trabajo", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }

            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en Finalizar tupo de trabajo " + e.Message);
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
        public ActionResult CrearTipo(TipoTrabajoDTO tipoDTO)
        {
            try
            {
                //Comprobamos que no haya valores nulos
                if (tipoDTO.DescripcionTipo == null || tipoDTO.DescripcionTipo=="" || tipoDTO.PrecioTipo == null || tipoDTO.PrecioTipo == 0)
                {
                    MostrarAlerta("Valores vacios","No puso todos los campos", "error");
                    Util.EscribirEnElFichero("Un usuario quiso crear un tipo de trabajo pero puso valores nulos");
                    return RedirectToAction("Home", "RegistroControlador");
                }


                //Declaramos la implementacion
                implementaciontipos impl = new implementaciontipos();

                try
                {
                    //Comprobamos si se creo bien la incidencia
                    if (impl.CrearTipo(tipoDTO))
                    {
                        MostrarAlerta("Tipo Creado", "El tipo de trabajo se creo correctamente", "success");
                        Util.EscribirEnElFichero("Un usuario creo un tipo de trabajo");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                    //Si no se creo bien avisamos al usuario
                    else
                    {
                        MostrarAlerta("No se pudo crear", "No se pudo crear el tipo de trabajo", "error");
                        Util.EscribirEnElFichero("Un usuario quiso crear un tipo de trabajo pero no se pudo insertar");
                        return RedirectToAction("Home", "RegistroControlador");
                    }
                }
                catch (IOException e)
                {
                    MostrarAlerta("Hubo un error", "Hubo un error intentelo de nuevo mas tarde", "error");
                    Util.EscribirEnElFichero("Hubo un error en crear tipo de trabajo " + e.Message);
                    return RedirectToAction("Home", "RegistroControlador");
                }

            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en crear tipo de trabajo " + e.Message);
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
