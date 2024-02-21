using System.Globalization;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementacionIncidencias : interfazIncidencias
    {
        public bool crearSolicitud(SolicitudDTO solicitud)
        {
            accionesCRUD acciones = new accionesCRUD();
            return acciones.InsertarSolicitud(solicitud);            
        }
    }
}
