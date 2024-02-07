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

                if (acceso != "4")
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

    }
}
