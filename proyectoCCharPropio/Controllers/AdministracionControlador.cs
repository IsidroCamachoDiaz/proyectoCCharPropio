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
        }

        //Modelo de administracion
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
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "3")
                {
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
            //Cogemos todos los usuario y eliminamos de la lista el propio usuario paarq ue no se pueda modificar solo
            List<UsuarioDTO> usuarios =acciones.HacerGetLista<UsuarioDTO>("api/Usuario");

            for (int i = 0; i < usuarios.Count; i++)
            {
                if (usuarios[i].Id_usuario == usuario.Id_usuario)
                {
                    usuarios.Remove(usuarios[i]);
                    break;
                }
            }
            //Metemos todo en el modelo
            var modelo = new ModeloAdministracion
            {
                Usuario = usuario,
                ListaUsuarios = usuarios
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
                string idUsuario = HttpContext.Session.GetString("usuario");
                usuario = acciones.SeleccionarUsuario(idUsuario);

                if (acceso != "3")
                {
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
            //Cojo el valor tk
            string idUsuarioModificar = HttpUtility.ParseQueryString(uri.Query)["id"];
            //Cogemos el usuario que queremos modificar
            UsuarioDTO usuarioModificar = acciones.SeleccionarUsuario(idUsuarioModificar);

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
            //Declaramos lo que encesitemos
            accionesCRUD acciones= new accionesCRUD();

            //Cogemos el usuario a borrar
            usuarioDTO=acciones.SeleccionarUsuario(usuarioDTO.Id_usuario.ToString());

            implementacionAdminsitracion impl = new implementacionAdminsitracion();
            //Usamos el metodo para borrar
            if (impl.eliminarUsuario(usuarioDTO))
            {
                MostrarAlerta("Registro Completo", "Se le ha enviado un correo para verificar su identidad", "success");
                return RedirectToAction("AdministracionUsuarios");
            }
            //Si no se pudo se avisa al usuario
            else
            {
                MostrarAlerta("Error", "No se pudo borrar el usuario", "error");
                return RedirectToAction("AdministracionUsuarios");
            }

            
        }

        [HttpPost]
        public ActionResult ActualizarUsuario(UsuarioDTO usuarioDTO, IFormFile archivo)
        {
            //Declaramos lo que encesitemos
            accionesCRUD acciones = new accionesCRUD();
            implementacionAdminsitracion inter = new implementacionAdminsitracion();
            bool cambio = false;

            try
            {

                UsuarioDTO usuarioCambiar = acciones.SeleccionarUsuario(usuarioDTO.Id_usuario.ToString());

                if (usuarioCambiar == null)
                {
                    MostrarAlerta("No pudimos encontrar el Usuario", "No se encontro al usuario", "error");
                    return RedirectToAction("Index", "RegistroControlador");
                }

                if (usuarioDTO.Nombre_usuario!=usuarioCambiar.Nombre_usuario || 
                    usuarioDTO.Telefono_usuario!=usuarioCambiar.Telefono_usuario ||
                    usuarioDTO.Id_acceso!=usuarioCambiar.Id_acceso)
                {
                    if (usuarioDTO.Id_acceso!=usuarioCambiar.Id_acceso)
                    {
                        if (inter.CambiarAcceso(usuarioCambiar, usuarioDTO.Id_acceso.ToString()))
                        {
                        }
                        else
                        {
                            MostrarAlerta("Error De Acceso", "El usuario tiene asignados solicitudes o incidencias y no se puede asignar el acceso", "error");
                            return RedirectToAction("Index", "RegistroControlador");
                        }
                    }
                    usuarioCambiar.Nombre_usuario = usuarioDTO.Nombre_usuario;
                    usuarioCambiar.Telefono_usuario = usuarioDTO.Telefono_usuario;
                    cambio = true;
                }

                // Cojo la imagen
                if (archivo!=null)
                {
                    //Convierto el archivo en array de byte
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        archivo.CopyTo(memoryStream);
                        usuarioCambiar.Foto_usuario = memoryStream.ToArray();
                    }
                    cambio = true;
                }

                if (cambio)
                {
                    acciones.ActualizarUsuario(usuarioCambiar);
                    MostrarAlerta("Se cambiio el usuario Correctamente", "Se modifico correctamente el usuario " + usuarioCambiar.Nombre_usuario, "success");
                    return RedirectToAction("Index", "RegistroControlador");
                }
                // Si no ha cambiado se le avisa
                else
                {
                    MostrarAlerta("Campos Iguales", "No hizo ninguna modificacion", "warning");
                }
            }
            catch (Exception e)
            {
            }

            return RedirectToAction("Index", "RegistroControlador");

        }

    }
}
