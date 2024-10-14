/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Este controlador gerencia as operações CRUD (Create, Read, Update, Delete) 
 * para a entidade "Criador" na aplicação YouWatchAPI. Ele permite listar 
 * criadores, visualizar detalhes, criar novos, editar e excluir criadores, 
 * utilizando o Entity Framework para interagir com o banco de dados.
 * O controlador também lida com o hash da senha de criadores e garante 
 * a integridade dos dados durante essas operações.
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using YouWatchAPI.Data;
using YouWatchAPI.Models;

namespace YouWatchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CriadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* 
         * Método: Index
         * Descrição: 
         * Lista todos os criadores armazenados no banco de dados. Este método 
         * é protegido e somente criadores autenticados com a role "Criador" podem acessá-lo.
         * Retorno:
         *   - Uma lista JSON de todos os criadores disponíveis.
         */
        [HttpGet]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem listar criadores
        public async Task<IActionResult> Index()
        {
            var criadores = await _context.Criadores.ToListAsync();
            return Ok(criadores);
        }

        /* 
         * Método: Details
         * Descrição: 
         * Exibe os detalhes de um criador específico com base no seu ID. Somente criadores 
         * autenticados podem acessar esse método.
         * Parâmetros:
         *   - id: O ID do criador a ser exibido.
         * Retorno:
         *   - O criador específico em formato JSON ou NotFound se o criador não for encontrado.
         */
        [HttpGet("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem acessar detalhes
        public async Task<IActionResult> Details(int id)
        {
            var criador = await _context.Criadores.FindAsync(id);

            if (criador == null)
            {
                return NotFound();
            }

            return Ok(criador);
        }

        /* 
         * Método: Create
         * Descrição: 
         * Permite que criadores autenticados criem novos registros de criador no sistema. 
         * A senha fornecida é hasheada antes de ser armazenada no banco de dados.
         * Parâmetros:
         *   - criador: O objeto Criador que será criado.
         * Retorno:
         *   - O criador criado, ou BadRequest se os dados forem inválidos.
         */
        [HttpPost]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem criar novos criadores
        public async Task<IActionResult> Create(Criador criador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            criador.Senha = HashPassword(criador.Senha); // Hash de senha adicionado
            _context.Criadores.Add(criador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = criador.Id }, criador);
        }

        /* 
         * Método: Edit
         * Descrição: 
         * Permite que criadores autenticados editem os dados de um criador existente. 
         * O criador é validado e atualizado no banco de dados após a validação.
         * Parâmetros:
         *   - id: O ID do criador a ser editado.
         *   - criador: O objeto Criador contendo os novos dados.
         * Retorno:
         *   - NoContent se a atualização for bem-sucedida, ou NotFound se o criador não for encontrado.
         */
        [HttpPut("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem editar dados de criadores
        public async Task<IActionResult> Edit(int id, Criador criador)
        {
            if (id != criador.Id)
            {
                return BadRequest();
            }

            _context.Entry(criador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Criadores.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /* 
         * Método: Delete
         * Descrição: 
         * Permite que criadores autenticados excluam um criador do sistema. O criador será removido 
         * permanentemente do banco de dados.
         * Parâmetros:
         *   - id: O ID do criador a ser excluído.
         * Retorno:
         *   - NoContent se a exclusão for bem-sucedida, ou NotFound se o criador não for encontrado.
         */
        [HttpDelete("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem excluir criadores
        public async Task<IActionResult> Delete(int id)
        {
            var criador = await _context.Criadores.FindAsync(id);
            if (criador == null)
            {
                return NotFound();
            }

            _context.Criadores.Remove(criador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /* 
         * Método: HashPassword
         * Descrição: 
         * Este método aplica um algoritmo de hash SHA512 para proteger a senha fornecida. 
         * A senha hasheada é armazenada no banco de dados, e o hash é verificado durante o login.
         * Parâmetros:
         *   - password: A senha fornecida pelo criador.
         * Retorno:
         *   - A senha hasheada como string.
         */
        private string HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return passwordHash;
            }
        }
    }
}
