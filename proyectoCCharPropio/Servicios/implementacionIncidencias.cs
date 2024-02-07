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
            IncidenciaDTO inc=new IncidenciaDTO(solicitud.DescripcionSolicitud,solicitud.Estado,solicitud);
            solicitud.Incidencia = inc;
            return acciones.InsertarSolicitud(solicitud);
        }
    }
}
