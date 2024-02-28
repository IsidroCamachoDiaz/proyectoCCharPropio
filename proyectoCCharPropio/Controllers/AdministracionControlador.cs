using System.Net.Mail;
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
    public class AdministracionControlador : Controller
    {
        //Modelo de administracion
        public class ModeloAdministracion
        {
            public List<UsuarioDTO> ListaUsuarios { get; set; }
            public UsuarioDTO Usuario { get; set; }

            public List<AccesoDTO> Acessos { get; set; }
        }

        //Modelo para la creacion de usuarios
        public class ModeloCreacion
        {
            public UsuarioDTO Usuario { get; set; }
            public List <AccesoDTO> accesos { get; set; }
        }

        //Modelo de modificacion de usuarios
        public class ModeloModificar
        {
            public UsuarioDTO Usuario { get; set; }

            public UsuarioDTO UsuarioModificar { get; set; }
            public List<AccesoDTO> Accesos { get; set; }
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
        public IActionResult CrearUsuario()
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

                if (accesoO.CodigoAcceso1 != "Administrador")
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
            //Cogemos todos los accesos para que el administrador añada al usuario el que quiera
            List<AccesoDTO> accesos = acciones.HacerGetLista<AccesoDTO>("api/Acceso");

            //Metemos todo en el modelo
            var modelo = new ModeloCreacion
            {
                Usuario= usuario,
                accesos = accesos
            };

            Util.EscribirEnElFichero("Se le llevo a la creacion de usuarios");
            return View(modelo);
        }


        [HttpGet]
        public IActionResult AdministracionUsuarios()
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

                if (accesoO.CodigoAcceso1 != "Administrador")
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
            //Cogemos todos los usuario y eliminamos de la lista el propio usuario para que no se pueda modificar solo
            List<UsuarioDTO> usuarios =acciones.HacerGetLista<UsuarioDTO>("api/Usuario");

            //Quitamos los usuarios que esten de baja
            usuarios =usuarios.Where(x => x.Fecha_baja == null).ToList();

            for (int i = 0; i < usuarios.Count; i++)
            {
                if (usuarios[i].Id_usuario == usuario.Id_usuario)
                {
                    usuarios.Remove(usuarios[i]);
                    break;
                }
            }

            //Cogemos todos los acceso paar mostar cada aceeso de usuario
            List<AccesoDTO> accesos = acciones.HacerGetLista<AccesoDTO>("api/Acceso");

            //Metemos todo en el modelo
            var modelo = new ModeloAdministracion
            {
                Usuario = usuario,
                ListaUsuarios = usuarios,
                Acessos=accesos
            };

            Util.EscribirEnElFichero("Se le llevo a la administracion de usuarios");
            return View(modelo);
        }

        [HttpGet]
        public IActionResult ModificarUsuario()
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

                if (accesoO.CodigoAcceso1 != "Administrador")
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

            //Cojo la url del navegador
            string urlCompleta = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Uri uri = new Uri(urlCompleta);
            //Cojo el valor id
            string idUsuarioModificar = HttpUtility.ParseQueryString(uri.Query)["id"];

            //Cogemos el usuario que queremos modificar
            UsuarioDTO usuarioModificar = acciones.SeleccionarUsuario(idUsuarioModificar);

            //Comprobamos si lo encontro
            if (usuarioModificar == null)
            {
                Util.EscribirEnElFichero("Se intento acceder a modificar usuario pero no se encontro");
                MostrarAlerta("¡Hubo Un Error!", "No se encontro el usuario", "error");
                return RedirectToAction("Home", "RegistroControlador");
            }

            //Cogemos todos los accesos
            List<AccesoDTO> accesos = acciones.HacerGetLista<AccesoDTO>("api/Acceso");


            //Metemos todo en el modelo
            var modelo = new ModeloModificar
            {
                Usuario = usuario,
                UsuarioModificar = usuarioModificar,
                Accesos=accesos
            };

            Util.EscribirEnElFichero("Se le llevo a modificar Usuario");
            return View(modelo);
        }


        [HttpPost]
        public ActionResult BorrarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                //Declaramos lo que encesitemos
                accionesCRUD acciones = new accionesCRUD();

                //Cogemos el usuario a borrar
                usuarioDTO = acciones.SeleccionarUsuario(usuarioDTO.Id_usuario.ToString());

                implementacionAdminsitracion impl = new implementacionAdminsitracion();
                //Usamos el metodo para borrar
                if (impl.eliminarUsuario(usuarioDTO))
                {
                    Util.EscribirEnElFichero("Se booro el usuario: " + usuarioDTO.Nombre_usuario);
                    MostrarAlerta("Usuario Eliminado", "Se borro el usuario" + usuarioDTO.Nombre_usuario, "success");
                    return RedirectToAction("AdministracionUsuarios");
                }
                //Si no se pudo se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Se intento borrar un usuario pero no se pudo");
                    MostrarAlerta("Error", "No se pudo borrar el usuario", "error");
                    return RedirectToAction("AdministracionUsuarios");
                }
            }catch(Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en Borrar Usuario " + e.Message);
            }
            return RedirectToAction("AdministracionUsuarios");

        }

        [HttpPost]
        public ActionResult ActualizarUsuario(UsuarioDTO usuarioDTO, IFormFile archivo)
        {
            //Declaramos lo que encesitemos
            accionesCRUD acciones = new accionesCRUD();
            implementacionAdminsitracion inter = new implementacionAdminsitracion();
            //Creamos una variable booleana para comprobar
            bool cambio = false;

            try
            {
                //Cogemos el usuario de la base de datos
                UsuarioDTO usuarioCambiar = acciones.SeleccionarUsuario(usuarioDTO.Id_usuario.ToString());

                 //Comprobamos si lo encontro si no se avisa al usuario
                if (usuarioCambiar == null)
                {
                    Util.EscribirEnElFichero("Se intento actualizar un usuario pero no se pudo");
                    MostrarAlerta("No pudimos encontrar el Usuario", "No se encontro al usuario", "error");
                    return RedirectToAction("Home", "RegistroControlador");
                }

                //Comprobamos si alguna campo es distinto con los modificados
                if (usuarioDTO.Nombre_usuario!=usuarioCambiar.Nombre_usuario || 
                    usuarioDTO.Telefono_usuario!=usuarioCambiar.Telefono_usuario ||
                    usuarioDTO.Id_acceso!=usuarioCambiar.Id_acceso)
                {
                    //Comprobamos si quiera cambiar el acceso
                    if (usuarioDTO.Id_acceso!=usuarioCambiar.Id_acceso)
                    {
                        //Si lo quiera cambiar comprobamos si lo hace bien
                        if (inter.CambiarAcceso(usuarioCambiar, usuarioDTO.Id_acceso.ToString()))
                        {
                        }
                        //Si no se le avisa al usuario
                        else
                        {
                            Util.EscribirEnElFichero("Se intento cambiar el acceso de un usuario pero no se pudo");
                            MostrarAlerta("Error De Acceso", "El usuario tiene asignados solicitudes o incidencias y no se puede asignar el acceso", "error");
                            return RedirectToAction("Home", "RegistroControlador");
                        }
                    }
                    //Cambiamos lo campos y ponemos a true el cambio
                    usuarioCambiar.Nombre_usuario = usuarioDTO.Nombre_usuario;
                    usuarioCambiar.Telefono_usuario = usuarioDTO.Telefono_usuario;
                    cambio = true;
                }

                // Cojo la imagen
                if (archivo!=null)
                {
                    //Usamos la clase para convertir el archivo en u array de bytes
                    ComprobacionImagen ci = new ComprobacionImagen();
                    usuarioCambiar.Foto_usuario = ci.ConvertirImagenEnBytes(archivo);
                    cambio = true;
                }

                //Comprobamos si cambio algo
                if (cambio)
                {
                    //Actualizamos el usuario y avisamos al usuario
                    acciones.ActualizarUsuario(usuarioCambiar);
                    Util.EscribirEnElFichero("Se actualizo el usuario: "+usuarioCambiar.Nombre_usuario);
                    MostrarAlerta("Se cambio el usuario Correctamente", "Se modifico correctamente el usuario " + usuarioCambiar.Nombre_usuario, "success");
                    return RedirectToAction("Home", "RegistroControlador");
                }
                // Si no ha cambiado se le avisa
                else
                {
                    Util.EscribirEnElFichero("Se intento actualizar un usuario pero no se pudo");
                    MostrarAlerta("Campos Iguales", "No hizo ninguna modificacion", "warning");
                    return RedirectToAction("Home", "RegistroControlador");
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero("Hubo un error en Actualizar Usuario "+e.Message);
            }

            return RedirectToAction("Index", "RegistroControlador");

        }

        // Acción HTTP POST para el registro de usuarios
        [HttpPost]
        public ActionResult CrearUsuario(UsuarioDTO usuarioDTO, IFormFile archivo)
        {
            // Verificar si algún parámetro requerido está vacío
            if (usuarioDTO.Nombre_usuario == null || usuarioDTO.Telefono_usuario == null || usuarioDTO.Correo_usuario == null || usuarioDTO.Contrasenia_usuario == null)
            {
                Util.EscribirEnElFichero("Un administrador quiso crear un usuario pero puso campos vacios");
                MostrarAlerta("¡Campos Incompletos!", "Hay Campos Vacios", "error");
                return RedirectToAction("CrearUsuario", "AdministracionControlador");
            }
            else
            {
                //Creamos un usuario para comprobar que el correo introducido esta disponible
                accionesCRUD acciones = new accionesCRUD();
                UsuarioDTO usuarioSiHay = acciones.SeleccionarUsuario("correo/" + usuarioDTO.Correo_usuario);

                //Comprobamos si esta sociada a una cuenta
                if (usuarioSiHay != null)
                {
                    Util.EscribirEnElFichero("Un administardor quiso crear un usuario pero el correo puesto ya esta asoicada a una cuenta");
                    MostrarAlerta("¡Ya existe una cuenta con ese correo!", "El correo especificado ya esta asociada a una cuenta", "error");
                    return RedirectToAction("CrearUsuario", "AdministracionControlador");
                }

                //Usamos la clase para convertir el archivo en u array de bytes
                ComprobacionImagen ci= new ComprobacionImagen();
                usuarioDTO.Foto_usuario = ci.ConvertirImagenEnBytes(archivo);

                // Crear instancia de la implementación de Administracion
                implementacionAdminsitracion implAd = new implementacionAdminsitracion();


                // Llamar al método para crear el usuario
                if (implAd.CrearUsuario(usuarioDTO))
                {
                    Util.EscribirEnElFichero("Se creo un usuario: "+usuarioDTO.Nombre_usuario);
                    MostrarAlerta("Usuario Creado", "Se le ha creado el usuario nuevo se le envio un correo", "success");
                }
                //Si no se crea bien se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Se intento crear un usuario pero hubo un error en la creacion");
                    MostrarAlerta("Error", "Hubo un error intentelo mas tarde", "error");
                }

                // Redireccionar a la vista de index
                return RedirectToAction("AdministracionUsuarios");
            }
        }

    }
}
