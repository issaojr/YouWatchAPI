/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "UsuarioService" fornece a camada de serviço para manipular 
 * as operações relacionadas a "Usuario" na aplicação YouWatchAPI. Ela inclui 
 * a lógica para hashear senhas de usuários antes de armazená-las no banco de dados 
 * e utiliza o Entity Framework para realizar a operação de criação de usuários.
 */

using Microsoft.EntityFrameworkCore;
using System.Text;
using YouWatchAPI.Data;
using YouWatchAPI.Models;

namespace YouWatchAPI.Services
{
    public class UsuarioService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        /* 
         * Método: HashPassword
         * Descrição: 
         * Este método gera um hash seguro para a senha do usuário usando o algoritmo HMACSHA512. 
         * As senhas nunca são armazenadas em texto simples no banco de dados, aumentando a segurança da aplicação.
         * Parâmetros:
         *   - password: A senha em texto simples que será hasheada.
         * Retorno:
         *   - O hash da senha, que será armazenado no banco de dados.
         */
        public string HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return passwordHash;
            }
        }

        /* 
         * Método: CreateUser
         * Descrição: 
         * Cria um novo usuário no banco de dados após hashear sua senha. Este método utiliza o 
         * Entity Framework para salvar o usuário no banco de dados de forma assíncrona.
         * Parâmetros:
         *   - usuario: O objeto Usuario contendo os dados do novo usuário a ser criado.
         * Retorno:
         *   - O objeto Usuario recém-criado com a senha hasheada.
         */
        public async Task<Usuario> CreateUser(Usuario usuario)
        {
            // Hasheia a senha do usuário antes de salvar
            usuario.Senha = HashPassword(usuario.Senha);

            // Salva o usuário no banco de dados de forma assíncrona
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
