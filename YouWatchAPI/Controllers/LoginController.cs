/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Este controlador gerencia o processo de autenticação na aplicação YouWatchAPI. 
 * Ele permite que tanto "Usuarios" quanto "Criadores" façam login com suas credenciais, 
 * e em caso de sucesso, um token JWT é gerado e retornado. O token JWT inclui 
 * informações como o email e o papel (role) do usuário, que pode ser "Usuario" ou "Criador". 
 * Este token será usado em requisições subsequentes para validar a identidade e as permissões 
 * do usuário. O controlador também lida com a verificação do hash da senha durante o processo 
 * de login.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YouWatchAPI.Data;
using YouWatchAPI.Models;

namespace YouWatchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /* 
         * Método: Login
         * Descrição: 
         * Este método recebe um objeto do tipo LoginModel contendo o email e a senha do usuário. 
         * Primeiro, tenta autenticar o usuário como "Usuario". Se o usuário for encontrado 
         * e a senha estiver correta, um token JWT será gerado com a role "Usuario". 
         * Caso contrário, tenta autenticar como "Criador" e, se for bem-sucedido, 
         * o token é gerado com a role "Criador". Caso nenhuma das tentativas tenha sucesso, 
         * é retornado um status 401 Unauthorized.
         * Parâmetros: 
         *   - request: Objeto LoginModel contendo o email e a senha.
         * Retorno:
         *   - Um token JWT em caso de sucesso, ou 401 Unauthorized em caso de falha.
         */
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            // Tenta autenticar como Usuário
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario != null && VerifyPasswordHash(request.Senha, usuario.Senha))
            {
                var token = GenerateJwtToken(usuario.Email, "Usuario");
                return Ok(new { Token = token });
            }

            // Tenta autenticar como Criador
            var criador = await _context.Criadores.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (criador != null && VerifyPasswordHash(request.Senha, criador.Senha))
            {
                var token = GenerateJwtToken(criador.Email, "Criador");
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciais inválidas.");
        }

        /* 
         * Método: GenerateJwtToken
         * Descrição: 
         * Este método gera um token JWT contendo as claims do email e da role do usuário. 
         * O token é assinado com uma chave secreta e tem uma expiração definida de 2 horas. 
         * Parâmetros:
         *   - email: O email do usuário autenticado.
         *   - role: O papel (role) do usuário, que pode ser "Usuario" ou "Criador".
         * Retorno:
         *   - Um token JWT como string.
         */
        private string GenerateJwtToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /* 
         * Método: VerifyPasswordHash
         * Descrição: 
         * Este método verifica se a senha fornecida pelo usuário, quando hasheada, 
         * corresponde ao hash da senha armazenado no banco de dados.
         * Parâmetros:
         *   - password: A senha fornecida pelo usuário.
         *   - storedHash: O hash da senha armazenada no banco de dados.
         * Retorno:
         *   - true se a senha for válida, false caso contrário.
         */
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return storedHash == computedHash;
            }
        }
    }
}
