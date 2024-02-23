using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementacionTrabajo : interfazTrabajos
    {
        public bool crearTrabajo(TrabajoDTO trabajo)
        {
            //Declaramos lo que necesitemos
            accionesCRUD acciones = new accionesCRUD();
            //Insertamos el trabajo
            return acciones.InsertarTrabajo(trabajo);
        }

        public bool finalizarTrabajo(TrabajoDTO trabajo)
        {
            //Declaramos lo que necesitemos
            accionesCRUD acciones = new accionesCRUD();

            //Actualizamos el trabajo y la incidencia
            acciones.ActualizarTrabajo(trabajo);
            return acciones.ActualizarIncidencia(trabajo.incidencia);
        }
    }
}
