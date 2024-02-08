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
            IncidenciaDTO inc=new IncidenciaDTO(solicitud.DescripcionSolicitud2,solicitud.Estado2,solicitud);
            solicitud.Incidencia2 = inc;
            return acciones.InsertarSolicitud(solicitud);
        }
    }
}
