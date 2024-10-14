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
 * para a entidade "Usuario" na aplicação YouWatchAPI. Ele permite listar 
 * usuários, visualizar detalhes, criar novos, editar e excluir usuários, 
 * utilizando o Entity Framework para interagir com o banco de dados.
 * O controlador também lida com a validação de dados e garante a integridade 
 * das informações ao realizar essas operações.
 */

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Data;
using YouWatchAPI.Models;

namespace YouWatchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* 
         * Método: Index
         * Descrição: 
         * Lista todos os usuários armazenados no banco de dados. Somente usuários autenticados 
         * com a role "Usuario" podem acessar este método.
         * Retorno:
         *   - Uma lista JSON de todos os usuários.
         */
        [HttpGet]
        [Authorize(Roles = "Usuario")] // Somente usuários autenticados podem listar
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        /* 
         * Método: Details
         * Descrição: 
         * Exibe os detalhes de um usuário específico com base no seu ID. 
         * Somente usuários autenticados com a role "Usuario" podem acessar este método.
         * Parâmetros:
         *   - id: O ID do usuário a ser exibido.
         * Retorno:
         *   - O usuário específico em formato JSON ou NotFound se o usuário não for encontrado.
         */
        [HttpGet("{id}")]
        [Authorize(Roles = "Usuario")] // Somente usuários autenticados podem visualizar detalhes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        /* 
         * Método: Create
         * Descrição: 
         * Permite que usuários autenticados criem novos registros de usuário no sistema.
         * O usuário é validado e salvo no banco de dados.
         * Parâmetros:
         *   - usuario: O objeto Usuario que será criado.
         * Retorno:
         *   - O usuário criado, ou BadRequest se os dados forem inválidos.
         */
        [HttpPost]
        [Authorize(Roles = "Usuario")] // Somente usuários podem criar outros usuários
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Details), new { id = usuario.Id }, usuario);
        }

        /* 
         * Método: Edit
         * Descrição: 
         * Permite que usuários autenticados editem os dados de um usuário existente. 
         * O ID do usuário deve corresponder ao ID fornecido no corpo da requisição.
         * Parâmetros:
         *   - id: O ID do usuário a ser editado.
         *   - usuario: O objeto Usuario contendo os novos dados.
         * Retorno:
         *   - NoContent se a atualização for bem-sucedida, ou NotFound se o usuário não for encontrado.
         */
        [HttpPut("{id}")]
        [Authorize(Roles = "Usuario")] // Somente usuários autenticados podem editar
        public async Task<IActionResult> Edit(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id))
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
         * Permite que usuários autenticados excluam um usuário do sistema. 
         * O usuário será removido permanentemente do banco de dados.
         * Parâmetros:
         *   - id: O ID do usuário a ser excluído.
         * Retorno:
         *   - NoContent se a exclusão for bem-sucedida, ou NotFound se o usuário não for encontrado.
         */
        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario")] // Somente usuários autenticados podem excluir
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /* 
         * Método: UsuarioExists
         * Descrição: 
         * Verifica se um usuário com o ID fornecido existe no banco de dados.
         * Parâmetros:
         *   - id: O ID do usuário a ser verificado.
         * Retorno:
         *   - true se o usuário existir, false caso contrário.
         */
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
