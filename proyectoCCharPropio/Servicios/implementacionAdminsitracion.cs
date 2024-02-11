using System.Globalization;
using System.Security;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementacionAdminsitracion : interfazAdministracion
    {
        public bool eliminarUsuario(UsuarioDTO usu)
        {
            //Declaramos loq ue necesitemos
            accionesCRUD acciones = new accionesCRUD();
            //Cogemos su acceso para saber que te3nemos que comprobar
            AccesoDTO accesoPropio = acciones.SeleccionarAcceso(usu.Id_acceso.ToString());

            //Comprobamos que tipo de usuario es
            if (accesoPropio.CodigoAcceso1 == ("Administrador") || accesoPropio.CodigoAcceso1 == ("Empleado"))
            {
                //Cogemos todas las incidencias
                List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");
                //Cogemos todos los tokens
                List<TokenDTO> tokens = acciones.HacerGetLista<TokenDTO>("api/Token");
                //Cogemos los tokens del usuario
                tokens = tokens.Where(x => x.Id_usuario == usu.Id_usuario).ToList();
                //Cogemos las incidencias del usuario
                incidencias = incidencias.Where(x => x.Solicitud.IdSolicitud2 == usu.Id_usuario).ToList();
                //Si no tiene borramos todo lo que tenemos del usuario 
                if (incidencias.Count == 0)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        acciones.EliminarToken(tokens[i].Id_usuario.ToString());
                    }

                    acciones.EliminarUsuario(usu.Id_usuario.ToString());
                    Util.EscribirEnElFichero("Se elimino un usuario por completo " + usu.Nombre_usuario);
                    return true;
                }
                //Si tiene incidencias lo damos de baja
                else
                {
                    usu.Fecha_baja = DateTime.Now;
                    acciones.ActualizarUsuario(usu);
                    Util.EscribirEnElFichero("Un usuario se dio de baja en la web " + usu.Nombre_usuario);
                    return true;
                }
            }
            //Es usuario
            else
            {
                //Cogemos las solicitudes
                List<SolicitudDTO> solicitudes = acciones.HacerGetLista<SolicitudDTO>("api/Solicitud");
                //Cogemso los tokens
                List<TokenDTO> tokens = acciones.HacerGetLista<TokenDTO>("api/Token");
                //Cogemos lo del usuario
                tokens = tokens.Where(x => x.Id_usuario == usu.Id_usuario).ToList();
                solicitudes = solicitudes.Where(x => x.IdUsuario2.Id_usuario == usu.Id_usuario).ToList();
                //Si no tiene solicitudes borramos el usuario entero
                if (solicitudes.Count == 0)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        acciones.EliminarToken(tokens[i].Id_usuario.ToString());
                    }

                    acciones.EliminarUsuario(usu.Id_usuario.ToString());
                    Util.EscribirEnElFichero("Se elimino un usuario por completo " + usu.Nombre_usuario);
                    return true;
                }
                //Si tiene solicitudes lo damos de baja
                else
                {
                    usu.Fecha_baja = DateTime.Now;
                    acciones.ActualizarUsuario(usu);
                    Util.EscribirEnElFichero("Un usuario se dio de baja en la web " + usu.Nombre_usuario);
                    return true;
                }
            }
            return false;
        }

        public bool CambiarAcceso(UsuarioDTO usuario, string idAcceso)
        {
            //Declaramos lo que necesitemos
            accionesCRUD acciones = new accionesCRUD();

            //Buscamos el acceszo que se le quiere dar
            AccesoDTO accesoDar = acciones.SeleccionarAcceso(idAcceso);
            AccesoDTO accesoTiene = acciones.SeleccionarAcceso(usuario.Id_acceso.ToString());

            //Cogemso todas las solicitudes
            List<SolicitudDTO> solicitudes = acciones.HacerGetLista<SolicitudDTO>("api/Solicitud");

            //Las filtramos por la del usuario
            solicitudes = solicitudes.Where(x => x.IdUsuario2.Id_usuario == usuario.Id_usuario).ToList();

            //Comprobamos que usuario se le va a dar
            if (accesoDar.CodigoAcceso1 == "Administrador")
            {
                //Si es el empleado se le da porque solo se le añade la gestion de usaurios
                if (accesoTiene.CodigoAcceso1 == "Empleado")
                {
                    usuario.Id_acceso = accesoDar.IdAcceso1;
                    Util.EscribirEnElFichero("Se le cambio el acceso a un usuario " + usuario.Nombre_usuario);
                    return true;
                }
                //Si es un usuario se comprueba si tiene solicitudes activas
                else
                {
                    //Si no tiene se le da
                    if (solicitudes.Count == 0)
                    {
                        usuario.Id_acceso = accesoDar.IdAcceso1;
                        Util.EscribirEnElFichero("Se le cambio el acceso a un usuario " + usuario.Nombre_usuario);
                        return true;
                    }
                    //Si tiene solicitudes no se le da
                    Util.EscribirEnElFichero("No se puede cambiar un usuario que tienen solicitudes de atencion al cliente");
                    return false;
                }
            }
            //Si se quiere dar empleado
            else if (accesoTiene.CodigoAcceso1 == "Empleado")
            {
                //Si es administrador se le da directamente porque el adminsitracion tiene el plus de gestion de ussuarios
                if (accesoTiene.CodigoAcceso1 == "Administrador")
                {
                    usuario.Id_acceso = accesoDar.IdAcceso1;
                    Util.EscribirEnElFichero("Se le cambio el acceso a un usuario " + usuario.Nombre_usuario);
                    return true;
                }
                //Si es un usuario se comprueba si tiene solicitudes
                else
                {
                    //Si no tiene solicitudes se le da el rol
                    if (solicitudes.Count == 0)
                    {
                        usuario.Id_acceso = accesoDar.IdAcceso1;
                        Util.EscribirEnElFichero("Se le cambio el acceso a un usuario " + usuario.Nombre_usuario);
                        return true;
                    }
                    //Si tiene solicitudes activas no se le da                  
                    Util.EscribirEnElFichero("No se puede cambiar un usuario que tienen solicitudes de atencion al cliente " + usuario.Nombre_usuario);
                    return false;
                }

            }
            //Si quiere ser usuario
            else
            {
                //Cogemos las incidencias y se filtran por las del el usuario
                List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");

                incidencias = incidencias.Where(x => x.Usuario.Id_usuario == usuario.Id_usuario).ToList();
                //Si no tiene ninguna incidencia se le da el rol de usuario
                if (incidencias.Count == 0)
                {
                    usuario.Id_acceso = accesoDar.IdAcceso1;
                    Util.EscribirEnElFichero("Se le cambio el acceso a un usuario " + usuario.Nombre_usuario);
                    return true;
                }
                //Si tiene incidencia no se le da              
                Util.EscribirEnElFichero("No se puede cambiar un usuario que tienen solicitudes de atencion al cliente " + usuario.Nombre_usuario);
                return false;
            }
        }

        public bool CrearUsuario(UsuarioDTO usu)
        {
            try
            {
                accionesCRUD acciones = new accionesCRUD();
                usu.Contrasenia_usuario = Util.EncriptarContra(usu.Contrasenia_usuario);
                usu.Alta_usuario = true;
                if (acciones.InsertarUsuario(usu))
                {
                    Correo c = new Correo();

                    String mensaje = c.MensajeCorreoConfirmacionAlta(usu.Nombre_usuario);

                    //Comprobamos si se ha enviado bien el correo
                    if (c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Bienvenido a nuestra web", "isidro@isidrocamachodiaz.es", true))
                    {
                        //Insertamos en el token
                        Util.EscribirEnElFichero("Se ha creado un usuario correctamente");
                        return true;
                    }
                    //No se pudo enviar el correo
                    else
                    {
                        Util.EscribirEnElFichero("Hubo problemas para enviar el correo");
                        return false;
                    }
                }
                //No se puede insertar el usuario
                else
                {
                    Util.EscribirEnElFichero("Hubo problemas para insertar al usuario");
                    return false;
                }

            }
            catch (IOException e)
            {
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
            }
            catch (InvalidOperationException e)
            {
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
            }
            catch (ArgumentNullException e)
            {
                Util.EscribirEnElFichero("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
                Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
            }
            catch (SecurityException s)
            {
                Util.EscribirEnElFichero(s.Message);
                Console.Error.WriteLine(s.Message);
            }
            catch (IndexOutOfRangeException io)
            {
                Util.EscribirEnElFichero(io.Message);
                Console.Error.WriteLine(io.Message);
            }
            return false;
        }
    }
}
