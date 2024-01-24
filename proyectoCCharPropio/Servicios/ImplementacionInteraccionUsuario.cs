using Newtonsoft.Json;
using proyectoCCharPropio.Recursos;
using pruebaRazor.DTOs;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;

namespace pruebaRazor.DTOs
{
	public class ImplementacionInteraccionUsuario : InterfazIteraccionUsuario
	{
		public bool RegistrarUsuario(UsuarioDTO usu)
		{
			try
			{
				accionesCRUD a = new accionesCRUD();
				usu.Id_acceso = 1;
				if(a.InsertarUsuario(usu))
				{
					Console.WriteLine("bien");
				}
				else
				{
					Console.WriteLine("mal");
				}
				JsonSerializerSettings jsonSettings = new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				};

				// Encriptamos la contraseña
				usu.Contrasenia_usuario = Util.EncriptarContra(usu.Contrasenia_usuario);

				string usuarioJson = JsonConvert.SerializeObject(usu, jsonSettings);
				Uri url = new Uri("https://localhost:7079/api/Usuarios");

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/json";
				request.Timeout = 10000; // Tiempo de espera en milisegundos

				using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(usuarioJson);
					streamWriter.Flush();
					streamWriter.Close();
				}

				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						if (response.StatusCode == HttpStatusCode.Created)
						{
							// Si la respuesta es HTTP_CREATED (201), se ha creado correctamente
							Console.WriteLine("Usuario creado exitosamente");
							return true;
						}
						else
						{
							// Si no es HTTP_CREATED, imprime la respuesta para depurar
							Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.StatusDescription}");
						}
					}
				}
				catch (WebException ex)
				{
					if (ex.Response is HttpWebResponse errorResponse)
					{
						// Imprime el código de error si se produce una excepción en la solicitud HTTP
						Console.WriteLine($"1Error en la solicitud HTTP: {errorResponse.StatusCode} {errorResponse.StatusDescription}");
					}
					else
					{
						Console.WriteLine($"2Error en la solicitud HTTP: {ex.Message}");
					}
				}
			}
			catch (JsonException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RegistrarUsuario] " + e.Message);
			}
			catch (SecurityException s)
			{
				Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
				Console.Error.WriteLine(io.Message);
			}

			return false;
		}

		public async Task<bool> LoginUsuario(UsuarioDTO usuario)
		{
            Console.WriteLine("Ha entrado en Login");
			Console.WriteLine(usuario.Contrasenia_usuario);

			// Encriptamos la contraseña del usuario para poder comparar con las de la base de datos
			usuario.Contrasenia_usuario = Util.EncriptarContra(usuario.Contrasenia_usuario);

			// URL de la API que deseas consultar
			string apiUrl = "https://localhost:7079/api/Usuarios";

			try
			{
				// Realiza la consulta GET
				string responseData;
				using (HttpClient client = new HttpClient())
				{
					// Realiza la solicitud GET a la API
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Verifica si la solicitud fue exitosa
					if (response.IsSuccessStatusCode)
					{
						// Lee y devuelve el contenido de la respuesta como cadena
						responseData = await response.Content.ReadAsStringAsync();
					}
					else
					{
						// En caso de error, lanza una excepción o maneja el error según tus necesidades
						throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
					}
				}

				// Deserializa la respuesta JSON a un objeto C#
				List<UsuarioDTO> usuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(responseData);

                // Ahora puedes trabajar con 'usuarios', que es una lista con los datos de la API
                foreach (UsuarioDTO aux in usuarios)
                {
					if(aux.Correo_usuario == usuario.Correo_usuario && aux.Contrasenia_usuario == usuario.Contrasenia_usuario)
					{
						Console.WriteLine("Hay coincidencia");
						return true;
					}
                }

				Console.WriteLine("No hay coincidencia");
				return false;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public bool RecuperarContrasenya(UsuarioDTO usu)
		{
			bool ok = false;
			try
			{
				JsonSerializerSettings jsonSettings = new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				};

				// Creamos el token
				// Primero creamos la fecha limite
				DateTime fechaLimite = DateTime.Now.AddMinutes(1);
				Console.WriteLine(fechaLimite);

				// Ahora creamos el token
				Guid guid = Guid.NewGuid();

				// Convertir el GUID a una cadena (string)
				string token = guid.ToString();

				// Aqui llamamos a los nuevos metodos

				Correo c = new Correo();
				String mensaje = c.MensajeCorreo(token, "https://localhost:7016/RegistroControlador/VistaRecuperar");
				ok = c.EnviarMensaje(mensaje, usu.Correo_usuario, true, "Recuperar Contraseña", "infolentos@frangallegodorado.es", true);

				Console.WriteLine(token);

				TokenDTO tokenDto = new TokenDTO(token, usu.Id_usuario, fechaLimite);
				string usuarioJson = JsonConvert.SerializeObject(tokenDto, jsonSettings);
				Uri url = new Uri("https://localhost:7079/api/Token");

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/json";
				request.Timeout = 10000; // Tiempo de espera en milisegundos

				using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(usuarioJson);
					streamWriter.Flush();
					streamWriter.Close();
				}

				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						if (response.StatusCode == HttpStatusCode.Created)
						{
							// Si la respuesta es HTTP_CREATED (201), se ha creado correctamente
							Console.WriteLine("Usuario creado exitosamente");
							return true;
						}
						else
						{
							// Si no es HTTP_CREATED, imprime la respuesta para depurar
							Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.StatusDescription}");
						}
					}
				}
				catch (WebException ex)
				{
					if (ex.Response is HttpWebResponse errorResponse)
					{
						// Imprime el código de error si se produce una excepción en la solicitud HTTP
						Console.WriteLine($"1Error en la solicitud HTTP: {errorResponse.StatusCode} {errorResponse.StatusDescription}");
					}
					else
					{
						Console.WriteLine($"2Error en la solicitud HTTP: {ex.Message}");
					}
				}
			}
			catch (JsonException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] El objeto UsuarioDto no se pudo convertir a JSON. |" + e);
			}
			catch (IOException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Se produjo un error al crear el flujo de salida. |" + e);
			}
			catch (InvalidOperationException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] Ya se ha utilizado el método para insertar el usuario. |" + e);
			}
			catch (ArgumentNullException e)
			{
				Console.Error.WriteLine("[ERROR-ImplentacionIntereaccionUsuario-RecuperarContrasenya] " + e.Message);
			}
			catch (SecurityException s)
			{
				Console.Error.WriteLine(s.Message);
			}
			catch (IndexOutOfRangeException io)
			{
				Console.Error.WriteLine(io.Message);
			}

			return false;
		}

		public async Task<UsuarioDTO> ObtieneUsuarioPorGmail(UsuarioDTO usu)
		{
			Console.WriteLine("Email: " + usu.Correo_usuario);
			// URL de la API que deseas consultar
			string apiUrl = "https://localhost:7079/api/Usuarios";

			try
			{
				// Realiza la consulta GET
				string responseData;
				using (HttpClient client = new HttpClient())
				{
					// Realiza la solicitud GET a la API
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Verifica si la solicitud fue exitosa
					if (response.IsSuccessStatusCode)
					{
						// Lee y devuelve el contenido de la respuesta como cadena
						responseData = await response.Content.ReadAsStringAsync();
					}
					else
					{
						// En caso de error, lanza una excepción o maneja el error según tus necesidades
						throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
					}
				}

				// Deserializa la respuesta JSON a un objeto C#
				List<UsuarioDTO> usuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(responseData);

				// Ahora puedes trabajar con 'result', que es el objeto C# con los datos de la API
				foreach (UsuarioDTO aux in usuarios)
				{
					if (aux.Correo_usuario == usu.Correo_usuario)
					{
						Console.WriteLine("Hay coincidencia");
						return aux;
					}
				}

				Console.WriteLine("No hay coincidencia");
				return null;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return null;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return null;
			}
		}

	

		// Método cambiar
		public async Task<bool> CambiarContrasenia(String contrasenia, TokenDTO to)
		{


			// URL de la API que deseas consultar
			string apiUrl = "https://localhost:7079/api/Usuarios";

			try
			{
				// Realiza la consulta GET
				string responseData;
				using (HttpClient client = new HttpClient())
				{
					// Realiza la solicitud GET a la API
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Verifica si la solicitud fue exitosa
					if (response.IsSuccessStatusCode)
					{
						// Lee y devuelve el contenido de la respuesta como cadena
						responseData = await response.Content.ReadAsStringAsync();
					}
					else
					{
						// En caso de error, lanza una excepción o maneja el error según tus necesidades
						throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
					}
				}

				// Deserializa la respuesta JSON a un objeto C#
				List<UsuarioDTO> usuarios = JsonConvert.DeserializeObject<List<UsuarioDTO>>(responseData);
				string apiU = "";
				// Ahora puedes trabajar con 'result', que es el objeto C# con los datos de la API
				foreach (UsuarioDTO aux in usuarios)
				{
					if (aux.Id_usuario == to.Id_usuario)
					{
						aux.Contrasenia_usuario = contrasenia;
						JsonSerializerSettings jsonSettings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore
						};

						apiUrl = "https://localhost:7079/api/Usuarios/" + aux.Id_usuario;
						string usuarioJson = JsonConvert.SerializeObject(aux, jsonSettings);
						HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
						request.Method = "PUT";
						request.ContentType = "application/json";
						request.Timeout = 10000; // Tiempo de espera en milisegundos

						using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
						{
							streamWriter.Write(usuarioJson);
							streamWriter.Flush();
							streamWriter.Close();
						}

						try
						{
							using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
							{
								if (response.StatusCode == HttpStatusCode.Created)
								{
									// Si la respuesta es HTTP_CREATED (201), se ha creado correctamente
									Console.WriteLine("Usuario Actualizado exitosamente");
									return true;
								}
								else
								{
									// Si no es HTTP_CREATED, imprime la respuesta para depurar
									Console.WriteLine($"Respuesta del servidor: {response.StatusCode} {response.StatusDescription}");
								}
							}
						}
						catch (WebException ex)
						{
							if (ex.Response is HttpWebResponse errorResponse)
							{
								// Imprime el código de error si se produce una excepción en la solicitud HTTP
								Console.WriteLine($"1Error en la solicitud HTTP: {errorResponse.StatusCode} {errorResponse.StatusDescription}");
							}
							else
							{
								Console.WriteLine($"2Error en la solicitud HTTP: {ex.Message}");
							}
						}

						return true;
					}
				}

				Console.WriteLine("No hay coincidencia");
				return false;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return false;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return false;
			}
		}

		public async Task<TokenDTO> ObtenerToken(UsuarioDTO usuario)
		{


			// URL de la API que deseas consultar
			string apiUrl = "https://localhost:7079/api/Token/token/" + usuario.Nombre_usuario;

			try
			{
				// Realiza la consulta GET
				string responseData;
				using (HttpClient client = new HttpClient())
				{
					// Realiza la solicitud GET a la API
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Verifica si la solicitud fue exitosa
					if (response.IsSuccessStatusCode)
					{
						// Lee y devuelve el contenido de la respuesta como cadena
						responseData = await response.Content.ReadAsStringAsync();
					}
					else
					{
						// En caso de error, lanza una excepción o maneja el error según tus necesidades
						throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
					}
				}

				// Deserializa la respuesta JSON a un objeto C#
				TokenDTO token = JsonConvert.DeserializeObject<TokenDTO>(responseData);
				return token;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Error de solicitud HTTP: {e.Message}");
				return null;
			}
			catch (JsonException e)
			{
				Console.WriteLine($"Error al deserializar JSON: {e.Message}");
				return null;
			}
		}
	}
}
