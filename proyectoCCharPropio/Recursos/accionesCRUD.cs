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
                var url = new Uri(BASE_URL + endpoint);
                var request = WebRequest.Create(url);
                request.Method = "GET";

                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var jsonResponse = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }
            }
            catch (Exception e)
            {
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
                var url = new Uri(BASE_URL + endpoint);
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var jsonEntidad = JsonConvert.SerializeObject(entidad,jsonSerializerSettings);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonEntidad);
                }

                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.Created;
            }
            catch (Exception e)
            {
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
                var url = new Uri(BASE_URL + endpoint);
                var request = WebRequest.Create(url);
                request.Method = "DELETE";

                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
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
                var url = new Uri(BASE_URL + endpoint);
                var request = WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";

                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var jsonEntidad = JsonConvert.SerializeObject(entidad,jsonSerializerSettings);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonEntidad);
                }

                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerPut] Error al realizar la solicitud PUT. | {e}");
            }

            return false;
        }

        //List<UsuarioDTO> listaUsuarios = HacerGetLista<UsuarioDTO>("api/Usuario")
        internal List<T> HacerGetLista<T>(string endpoint)
        {
            try
            {
                var url = new Uri(BASE_URL + endpoint);
                var request = WebRequest.Create(url);
                request.Method = "GET";

                using (var response = request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var jsonResponse = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(jsonResponse);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"[ERROR-InteraccionEntidad-HacerGet] Error al realizar la solicitud GET. | {e}");
            }

            return new List<T>();
        }

    }
}
