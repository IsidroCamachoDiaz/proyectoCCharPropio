﻿using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementacionTrabajo : interfazTrabajos
    {
        public bool crearTrabajo(TrabajoDTO trabajo)
        {
            //Declaramos loq ue necesitemos
            accionesCRUD acciones = new accionesCRUD();
            //Insertamos el trabajo
            return acciones.InsertarTrabajo(trabajo);
        }
    }
}