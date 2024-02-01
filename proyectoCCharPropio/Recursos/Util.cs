using ejercicioRepaso.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using System.Text;

namespace proyectoCCharPropio.Recursos
{
	public static class Util
	{
		public static string EncriptarContra(string password)
		{
			StringBuilder hexString = new StringBuilder();

			try
			{
				using (SHA256 sha256 = SHA256.Create())
				{
					byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

					foreach (byte b in hash)
					{
						hexString.Append(b.ToString("x2"));
					}
				}
			}
			catch (ArgumentNullException e)
			{
				Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Algoritmo nulo. |" + e);
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR-Encriptado-EncriptarContra] Error al encriptar la contraseña. |" + e);
			}

			return hexString.ToString();
		}

		public static void EscribirEnElFichero(string texto)
		{
			interfazEscritura es= new implementacionEscritura();
			DateTime ahora = DateTime.Now;
			StreamWriter sw= es.AbrirOCrearFichero("C:\\Users\\Puesto3\\Desktop\\FicheroLog\\log.txt", true);
			es.escribir(sw,String.Format("[{0}]-{1}",ahora.ToString(),texto));
			es.cerrar(sw);

        }



    }
}
