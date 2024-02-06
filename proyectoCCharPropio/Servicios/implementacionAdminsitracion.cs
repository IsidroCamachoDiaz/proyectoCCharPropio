using System.Globalization;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementacionAdminsitracion : interfazAdministracion
    {
        public bool eliminarUsuario(UsuarioDTO usu)
        {
            accionesCRUD acciones = new accionesCRUD();
            AccesoDTO accesoPropio = acciones.SeleccionarAcceso(usu.Id_usuario.ToString());
            if (accesoPropio.CodigoAcceso==("Administrador")|| accesoPropio.CodigoAcceso == ("Empleado"))
            {

            }
            else
            {
                List<SolicitudDTO> solicitudes = acciones.HacerGetLista<SolicitudDTO>("api/Solicitud");
                List<TokenDTO> tokens = acciones.HacerGetLista<TokenDTO>("api/Token");
                tokens = tokens.Where(x => x.Id_usuario == usu.Id_usuario).ToList();

                solicitudes = solicitudes.Where(x=> x.IdUsuario==usu.Id_usuario).ToList();
                if(solicitudes.Count==0){
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        acciones.EliminarToken(tokens[i].Id_usuario.ToString());
                    }

                    acciones.EliminarUsuario(usu.Id_usuario.ToString());
                    Util.EscribirEnElFichero("Se elimino un usuario por completo " + usu.Nombre_usuario);
                    return true;
                }
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
