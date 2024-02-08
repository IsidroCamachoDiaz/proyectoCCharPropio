using System.Globalization;
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
            if (accesoPropio.CodigoAcceso1==("Administrador")|| accesoPropio.CodigoAcceso1 == ("Empleado"))
            {
                //Cogemos todas las incidencias
                List<IncidenciaDTO> incidencias = acciones.HacerGetLista<IncidenciaDTO>("api/Incidencia");
                //Cogemos todos los tokens
                List<TokenDTO> tokens = acciones.HacerGetLista<TokenDTO>("api/Token");
                //Cogemos los tokens del usuario
                tokens = tokens.Where(x => x.Id_usuario == usu.Id_usuario).ToList();
                //Cogemos las incidencias del usuario
                incidencias=incidencias.Where(x => x.IdSolicitud1.IdSolicitud2 == usu.Id_usuario).ToList();
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
                solicitudes = solicitudes.Where(x=> x.IdUsuario2.Id_usuario==usu.Id_usuario).ToList();
                //Si no tiene solicitudes borramos el usuario entero
                if(solicitudes.Count==0){
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
                    usu.Fecha_baja=DateTime.Now;
                    acciones.ActualizarUsuario(usu);
                    Util.EscribirEnElFichero("Un usuario se dio de baja en la web " + usu.Nombre_usuario);
                    return true;
                }
            }
            return false;
        }
    }
}
