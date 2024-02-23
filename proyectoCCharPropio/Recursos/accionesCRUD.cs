using Newtonsoft.Json;
using proyectoCCharPropio.DTOS;
using System.Net;

namespace proyectoCCharPropio.Recursos
{
    /// <summary>
    /// Metodos para la conexion CRUD con la api 
    /// </summary>
    ///<autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class accionesCRUD
    {
        //URL de la api
        private const string BASE_URL = "http://localhost:5299/";

        public UsuarioDTO SeleccionarUsuario(string queDar)
        {
            return HacerGet<UsuarioDTO>("api/Usuario/" + queDar);
        }

        public AccesoDTO SeleccionarAcceso(string queDar)
        {
            return HacerGet<AccesoDTO>("api/Acceso/" + queDar);
        }

        public IncidenciaDTO SeleccionarIncidencia(string queDar)
        {
            return HacerGet<IncidenciaDTO>("api/Incidencia/" + queDar);
        }

        public SolicitudDTO SeleccionarSolicitud(string queDar)
        {
            return HacerGet<SolicitudDTO>("api/Solicitud/" + queDar);
        }

        public TipoTrabajoDTO SeleccionarTipoDeTrabajo(string queDar)
        {
            return HacerGet<TipoTrabajoDTO>("api/TiposIncidencia/" + queDar);
        }

        public TokenDTO SeleccionarToken(string queDar)
        {
            return HacerGet<TokenDTO>("api/Token/" + queDar);
        }

        public TrabajoDTO SeleccionarTrabajo(string queDar)
        {
            return HacerGet<TrabajoDTO>("api/Trabajo/" + queDar);
        }

        private T HacerGet<T>(string endpoint)
        {
            try
            {
                //Juntamos toda la url
                var url = new Uri(BASE_URL + endpoint);

                //Lo mandamos a la peticion
                var request = WebRequest.Create(url);

                //Indicamos que se hara un GET
                request.Method = "GET";

                // Realiza una solicitud HTTP y obtiene la respuesta del servidor
                using (var response = request.GetResponse())

                // Crea un lector para leer el contenido de la respuesta
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // Lee todo el contenido de la respuesta como una cadena de texto JSON
                    var jsonResponse = reader.ReadToEnd();
                    // Lee todo el contenido de la respuesta como una cadena de texto JSON
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero($"[ERROR-InteraccionEntidad-HacerGet] Error al realizar la solicitud GET. | {e}");
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerGet] Error al realizar la solicitud GET. | {e}");
            }

            return default(T);
        }

        public bool InsertarUsuario(UsuarioDTO usuarioMeter)
        {
            return HacerPost("api/Usuario", usuarioMeter);
        }

        public bool InsertarAcceso(AccesoDTO nuevoAcceso)
        {
            return HacerPost("api/Acceso", nuevoAcceso);
        }

        public bool InsertarIncidencia(IncidenciaDTO nuevaIncidencia)
        {
            return HacerPost("api/Incidencia", nuevaIncidencia);
        }

        public bool InsertarSolicitud(SolicitudDTO nuevaSolicitud)
        {
            return HacerPost("api/Solicitud", nuevaSolicitud);
        }

        public bool InsertarTipoDeTrabajo(TipoTrabajoDTO nuevoTipoTrabajo)
        {
            return HacerPost("api/TiposIncidencia", nuevoTipoTrabajo);
        }

        public bool InsertarToken(TokenDTO nuevoToken)
        {
            return HacerPost("api/Token", nuevoToken);
        }

        public bool InsertarTrabajo(TrabajoDTO nuevoTrabajo)
        {
            return HacerPost("api/Trabajo", nuevoTrabajo);
        }

        private bool HacerPost(string endpoint, object entidad)
        {
            try
            {
                //Juntamos toda la url
                var url = new Uri(BASE_URL + endpoint);

                //Lo mandamos a la peticion
                var request = WebRequest.Create(url);

                //Indicamos que haremos una peticion POST
                request.Method = "POST";
                //Indicamos que sera en JSON
                request.ContentType = "application/json";

                // Crea una instancia de JsonSerializerSettings para personalizar la serialización JSON
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    // Establece cómo manejar los bucles de referencia durante la serialización
                    // Ignore indica que se deben omitir los bucles de referencia
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                //Creamos la entidad en un json e indicamos que omita la refencia circular
                var jsonEntidad = JsonConvert.SerializeObject(entidad,jsonSerializerSettings);

                // Crea un lector para leer el contenido de la respuesta
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonEntidad);
                }

                //Cogemos la respuesta de la peticion
                var response = (HttpWebResponse)request.GetResponse();

                //Comprobamos si se creo bien
                return response.StatusCode == HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero($"[ERROR-InteraccionEntidad-HacerPost] Error al realizar la solicitud POST. | {e}");
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerPost] Error al realizar la solicitud POST. | {e}");
            }

            return false;
        }

        public bool EliminarAcceso(string idAcceso)
        {
            return HacerDelete("api/Acceso/" + idAcceso);
        }

        public bool EliminarIncidencia(string idIncidencia)
        {
            return HacerDelete("api/Incidencia/" + idIncidencia);
        }

        public bool EliminarSolicitud(string idSolicitud)
        {
            return HacerDelete("api/Solicitud/" + idSolicitud);
        }

        public bool EliminarTipoDeTrabajo(string idTipoTrabajo)
        {
            return HacerDelete("api/TipoIncidencia/" + idTipoTrabajo);
        }

        public bool EliminarToken(string idToken)
        {
            return HacerDelete("api/Token/" + idToken);
        }

        public bool EliminarTrabajo(string idTrabajo)
        {
            return HacerDelete("api/Trabajo/" + idTrabajo);
        }

        public bool EliminarUsuario(string idUsuario)
        {
            return HacerDelete("api/Usuario/" + idUsuario);
        }

        private bool HacerDelete(string endpoint)
        {
            try
            {
                //Juntamos la url
                var url = new Uri(BASE_URL + endpoint);
                //Se lo mandamos a la peticion
                var request = WebRequest.Create(url);
                //Indicamos que es una peticion DELETE
                request.Method = "DELETE";

                //Cogemos el estado de la peticion
                var response = (HttpWebResponse)request.GetResponse();
                //Comprobamso si se hizo bien
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero($"[ERROR-InteraccionEntidad-HacerDelete] Error al realizar la solicitud DELETE. | {e}");
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerDelete] Error al realizar la solicitud DELETE. | {e}");
            }

            return false;
        }

        public bool ActualizarIncidencia(IncidenciaDTO nuevaIncidencia)
        {
            return HacerPut($"api/Incidencia/{nuevaIncidencia.IdIncidencia}", nuevaIncidencia);
        }

        public bool ActualizarSolicitud(SolicitudDTO nuevaSolicitud)
        {
            return HacerPut("api/Solicitud/"+nuevaSolicitud.IdSolicitud2.ToString(), nuevaSolicitud);
        }

        public bool ActualizarTipoDeTrabajo(TipoTrabajoDTO nuevoTipoTrabajo)
        {
            return HacerPut("api/TiposIncidencia/"+nuevoTipoTrabajo.IdTipo, nuevoTipoTrabajo);
        }

        public bool ActualizarTrabajo(TrabajoDTO nuevoTrabajo)
        {
            return HacerPut("api/Trabajo/"+nuevoTrabajo.IdTrabajo, nuevoTrabajo);
        }

        public bool ActualizarUsuario(UsuarioDTO nuevoUsuario)
        {
            return HacerPut($"api/Usuario/"+nuevoUsuario.Id_usuario, nuevoUsuario);
        }

        private bool HacerPut(string endpoint, object entidad)
        {
            try
            {
                //Juntamos la url
                var url = new Uri(BASE_URL + endpoint);
                //Lo metemos en la peticion
                var request = WebRequest.Create(url);
                //Indicamos a la peticion que haremos un PUT y que sera en formato JSON
                request.Method = "PUT";
                request.ContentType = "application/json";

                // Crea una instancia de JsonSerializerSettings para personalizar la serialización JSON
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    // Establece cómo manejar los bucles de referencia durante la serialización
                    // Ignore indica que se deben omitir los bucles de referencia
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                //Convertimos la entidad en formato JSON y le añadimos que omita la referencia circular
                var jsonEntidad = JsonConvert.SerializeObject(entidad,jsonSerializerSettings);

                // Crea un lector para leer el contenido de la respuesta
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonEntidad);
                }

                //Cogemos el estado de la peticion
                var response = (HttpWebResponse)request.GetResponse();
                //Comprobamos si se hizo bien
                return response.StatusCode == HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero($"[ERROR-InteraccionEntidad-HacerPut] Error al realizar la solicitud PUT. | {e}");
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerPut] Error al realizar la solicitud PUT. | {e}");
            }

            return false;
        }

        //List<UsuarioDTO> listaUsuarios = HacerGetLista<UsuarioDTO>("api/Usuario")
        internal List<T> HacerGetLista<T>(string endpoint)
        {
            try
            {
                //Juntamos la url
                var url = new Uri(BASE_URL + endpoint);
                //Se lo asignamos a la peticion
                var request = WebRequest.Create(url);
                //Indicamos que es un metodo GET
                request.Method = "GET";

                // Realiza una solicitud HTTP y obtiene la respuesta del servidor
                using (var response = request.GetResponse())

                // Crea un lector para leer el contenido de la respuesta
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // Lee todo el contenido de la respuesta como una cadena de texto JSON
                    var jsonResponse = reader.ReadToEnd();
                    // Deserializa la cadena JSON en una lista del objeto del tipo especificado
                    return JsonConvert.DeserializeObject<List<T>>(jsonResponse);
                }
            }
            catch (Exception e)
            {
                Util.EscribirEnElFichero($"[ERROR-InteraccionEntidad-HacerGet] Error al realizar la solicitud GET. | {e}");
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerGet] Error al realizar la solicitud GET. | {e}");
            }

            return new List<T>();
        }

    }
}
