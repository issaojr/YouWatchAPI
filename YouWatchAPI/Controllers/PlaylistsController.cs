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
 * para a entidade "Playlist" na aplicação YouWatchAPI. Ele permite listar 
 * playlists, visualizar detalhes, criar novas, editar e excluir playlists, 
 * utilizando o Entity Framework para interagir com o banco de dados.
 * O controlador também lida com o relacionamento entre playlists e usuários, 
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
    public class PlaylistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* 
         * Método: Index
         * Descrição: 
         * Lista todas as playlists armazenadas no banco de dados. Somente usuários autenticados 
         * com a role "Usuario" podem acessar este método.
         * As playlists são retornadas com seus itens associados (ItemPlaylists) e seus conteúdos.
         * Retorno:
         *   - Uma lista JSON de todas as playlists disponíveis.
         */
        [HttpGet]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem acessar playlists
        public async Task<IActionResult> Index()
        {
            var playlists = await _context.Playlists.Include(p => p.ItemPlaylists).ThenInclude(ip => ip.Conteudo).ToListAsync();
            return Ok(playlists);
        }

        /* 
         * Método: Details
         * Descrição: 
         * Exibe os detalhes de uma playlist específica com base no seu ID. 
         * Somente usuários autenticados com a role "Usuario" podem acessar este método.
         * As playlists são retornadas com seus itens associados (ItemPlaylists) e seus conteúdos.
         * Parâmetros:
         *   - id: O ID da playlist a ser exibida.
         * Retorno:
         *   - A playlist específica em formato JSON ou NotFound se a playlist não for encontrada.
         */
        [HttpGet("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem acessar playlists individuais
        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _context.Playlists.Include(p => p.ItemPlaylists).ThenInclude(ip => ip.Conteudo).FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        /* 
         * Método: Create
         * Descrição: 
         * Permite que usuários autenticados criem novas playlists. O modelo de playlist será 
         * validado antes de ser salvo no banco de dados.
         * Parâmetros:
         *   - playlist: O objeto Playlist que será criado.
         * Retorno:
         *   - A playlist criada, ou BadRequest se os dados forem inválidos.
         */
        [HttpPost]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem criar playlists
        public async Task<IActionResult> Create([FromBody] Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = playlist.Id }, playlist);
        }

        /* 
         * Método: Edit
         * Descrição: 
         * Permite que usuários autenticados editem uma playlist existente. O ID da playlist
         * deve corresponder ao ID fornecido no corpo da requisição. As mudanças serão validadas 
         * antes de serem salvas no banco de dados.
         * Parâmetros:
         *   - id: O ID da playlist a ser editada.
         *   - playlist: O objeto Playlist contendo os novos dados.
         * Retorno:
         *   - NoContent se a atualização for bem-sucedida, ou NotFound se a playlist não for encontrada.
         */
        [HttpPut("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem editar playlists
        public async Task<IActionResult> Edit(int id, [FromBody] Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return BadRequest();
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Playlists.Any(e => e.Id == id))
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
         * Permite que usuários autenticados excluam uma playlist do sistema. A playlist será 
         * removida permanentemente do banco de dados.
         * Parâmetros:
         *   - id: O ID da playlist a ser excluída.
         * Retorno:
         *   - NoContent se a exclusão for bem-sucedida, ou NotFound se a playlist não for encontrada.
         */
        [HttpDelete("{id}")]
        [Authorize(Roles = "Usuario")]  // Somente usuários autenticados podem excluir playlists
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
