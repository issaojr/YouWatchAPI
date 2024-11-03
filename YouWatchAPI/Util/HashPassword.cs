using System.Security.Cryptography;
using System.Text;

namespace YouWatchAPI.Util
{
    public class HashPassword
    {
        /* 
         * Método: GenerateHash
         * Descrição: 
         * Este método gera um hash seguro para a senha do usuário usando o algoritmo HMACSHA512. 
         * As senhas nunca são armazenadas em texto simples no banco de dados, aumentando a segurança da aplicação.
         * Parâmetros:
         *   - password: A senha em texto simples que será hasheada.
         * Retorno:
         *   - O hash da senha, que será armazenado no banco de dados.
         */
        public static string GenerateHash(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
