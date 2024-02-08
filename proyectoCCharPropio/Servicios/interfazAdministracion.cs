using proyectoCCharPropio.DTOS;
using System;

namespace proyectoCCharPropio.Servicios
{
    public interface interfazAdministracion
    {
        public bool eliminarUsuario(UsuarioDTO usu);

        public bool CambiarAcceso(UsuarioDTO usuario, string idAcceso);
    }
}
