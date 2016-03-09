using System;
using System.Text;

namespace Dominio.Tools
{
    public static class Converter
    {
        public static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return String.Empty;
            
            var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
