using System;
using System.Globalization;
using NuGet.Common;
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

        public bool FinalizarIncidencia(IncidenciaDTO incidencia)
        {
            //Declaramos lo que necesitemos
            accionesCRUD acciones = new accionesCRUD();
            Correo c = new Correo();
           
            //Se crea un booleano para ver que cada trabajo se termino
            bool terminado = true;

            //Se va comprobando que todos los trabajos esten terminados
            foreach (TrabajoDTO tra in incidencia.Trabajos)
            {
                if (tra.EstadoTrabajo == false)
                {
                    terminado = false;
                }
            }
            //Si esta terminado se actualiza los campos
            if (terminado)
            {
                incidencia.FechaFin=DateTime.Now;
                incidencia.EstadoIncidencia = true;
                incidencia.solicitud.Estado2 = true;

                incidencia.solicitud.IdUsuario2 = acciones.SeleccionarUsuario(incidencia.Solicitud.idUsuarioNumero.ToString());
                //Se manda un correo para avisar al usuario
                String mensaje = c.MensajeCorreoConfirmacionTerminada(incidencia.Solicitud.IdUsuario2.Nombre_usuario);

                //Se comprueba si se envio bien
                if (c.EnviarMensaje(mensaje, incidencia.solicitud.IdUsuario2.Correo_usuario, true, "Incidencia Resuelta", "isidro@isidrocamachodiaz.es", true))
                {

                    //Volvemos aponer la lista como nulo para que no de problemas
                    incidencia.Trabajos = null;

                    //Se actualiza la incidencia
                    acciones.ActualizarIncidencia(incidencia);
                    acciones.ActualizarSolicitud(incidencia.solicitud);
                    Util.EscribirEnElFichero("Un usuario finalizo una incidencia");
                    return true;
                }
                //Si no se pudo finalizar se avisa al usuario
                else
                {
                    Util.EscribirEnElFichero("Se termino una incidencia pero no se le pudo enviar el correo al usuario");                   
                    return false;
                }
            }
            //Si hay trabajos pendientes se avisa al empleado
            else
            {
                Util.EscribirEnElFichero("Un usuario usuario intento finalizar una incidencia pero la incidencia tienen trabajos pendientes");
                return false;
            }
        }
    }
}
