using System.Globalization;
using proyectoCCharPropio.DTOS;
using proyectoCCharPropio.Recursos;

namespace proyectoCCharPropio.Servicios
{
    public class implementaciontipos : interfazTipos
    {
        public bool CrearTipo(TipoTrabajoDTO tipo)
        {
            //Declaramos lo que necesitemos
            accionesCRUD acciones = new accionesCRUD();

            //Comprobamos si se inserto bien el tipo
            if (acciones.InsertarTipoDeTrabajo(tipo))
            {     
                return true;
            }
            //Si no se inserto bien se avisa al usuario
            else
            {
                return false;
            }
        }
    }
}
