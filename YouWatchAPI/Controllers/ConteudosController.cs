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
 * para a entidade "Conteudo" na aplicação YouWatchAPI. Ele permite listar 
 * conteúdos, visualizar detalhes, criar novos, editar e excluir conteúdos, 
 * utilizando o Entity Framework para interagir com o banco de dados.
 * O controlador também lida com relacionamentos, como a associação de um conteúdo a um criador, 
 * e realiza validações para garantir a integridade dos dados durante essas operações.
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
    public class ConteudosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConteudosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* 
         * Método: Index
         * Descrição: 
         * Lista todos os conteúdos armazenados no banco de dados, incluindo o criador associado 
         * a cada conteúdo. Este método é público e pode ser acessado por qualquer usuário.
         * Retorno:
         *   - Uma lista JSON de todos os conteúdos disponíveis.
         */
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var conteudos = await _context.Conteudos.Include(c => c.Criador).ToListAsync();
            return Ok(conteudos); // Retorna os conteúdos em formato JSON
        }

        /* 
         * Método: Details
         * Descrição: 
         * Exibe os detalhes de um conteúdo específico com base no seu ID. Também inclui 
         * o criador associado ao conteúdo. Este método é público e pode ser acessado por qualquer usuário.
         * Parâmetros:
         *   - id: O ID do conteúdo a ser exibido.
         * Retorno:
         *   - O conteúdo específico em formato JSON ou NotFound se o conteúdo não for encontrado.
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var conteudo = await _context.Conteudos.Include(c => c.Criador).FirstOrDefaultAsync(c => c.Id == id);

            if (conteudo == null)
            {
                return NotFound();
            }

            return Ok(conteudo);
        }

        /* 
         * Método: Create
         * Descrição: 
         * Permite que criadores autenticados criem novos conteúdos no sistema. O conteúdo será associado
         * a um criador e validado antes de ser armazenado no banco de dados.
         * Parâmetros:
         *   - conteudo: O objeto Conteudo que será criado.
         * Retorno:
         *   - O conteúdo criado, ou BadRequest se os dados forem inválidos.
         */
        [HttpPost]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem criar conteúdos
        public async Task<IActionResult> Create([FromBody] Conteudo conteudo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Mantendo validação
            }

            _context.Conteudos.Add(conteudo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = conteudo.Id }, conteudo);
        }

        /* 
         * Método: Edit
         * Descrição: 
         * Permite que criadores autenticados editem um conteúdo existente. O conteúdo será
         * atualizado no banco de dados após a validação.
         * Parâmetros:
         *   - id: O ID do conteúdo a ser editado.
         *   - conteudo: O objeto Conteudo contendo os novos dados.
         * Retorno:
         *   - NoContent se a atualização for bem-sucedida, ou NotFound se o conteúdo não for encontrado.
         */
        [HttpPut("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem editar conteúdos
        public async Task<IActionResult> Edit(int id, [FromBody] Conteudo conteudo)
        {
            if (id != conteudo.Id)
            {
                return BadRequest();
            }

            _context.Entry(conteudo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Conteudos.Any(e => e.Id == id))
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
         * Permite que criadores autenticados excluam um conteúdo do sistema. O conteúdo será removido 
         * permanentemente do banco de dados.
         * Parâmetros:
         *   - id: O ID do conteúdo a ser excluído.
         * Retorno:
         *   - NoContent se a exclusão for bem-sucedida, ou NotFound se o conteúdo não for encontrado.
         */
        [HttpDelete("{id}")]
        [Authorize(Roles = "Criador")]  // Somente criadores autenticados podem excluir conteúdos
        public async Task<IActionResult> Delete(int id)
        {
            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo == null)
            {
                return NotFound();
            }

            _context.Conteudos.Remove(conteudo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
