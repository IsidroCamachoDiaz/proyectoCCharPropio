using Microsoft.AspNetCore.Mvc;
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


    }
}
