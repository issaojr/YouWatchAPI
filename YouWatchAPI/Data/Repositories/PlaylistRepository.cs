/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "PlaylistRepository" fornece a camada de acesso a dados para a entidade 
 * "Playlist" na aplicação YouWatchAPI. Ela utiliza o Entity Framework para realizar operações 
 * CRUD (Create, Read, Update, Delete) de forma assíncrona no banco de dados. O repositório 
 * inclui métodos para obter todas as playlists, buscar uma playlist por ID, adicionar, atualizar 
 * e deletar playlists. Além disso, a classe lida com o carregamento das relações entre playlists 
 * e conteúdos associados através da entidade "ItemPlaylist".
 */

using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Models;

namespace YouWatchAPI.Data.Repositories
{
    public class PlaylistRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /* 
         * Método: GetAllPlaylistsAsync
         * Descrição: 
         * Retorna todas as playlists do banco de dados, incluindo os itens associados 
         * e os conteúdos através da entidade "ItemPlaylist". O método é assíncrono para 
         * garantir melhor desempenho em operações de leitura do banco de dados.
         * Retorno:
         *   - Uma lista de todas as playlists com seus itens e conteúdos.
         */
        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .ToListAsync(); // Use ToListAsync() para retornos assíncronos
        }

        /* 
         * Método: GetPlaylistById
         * Descrição: 
         * Retorna uma playlist específica com base no seu ID. Também carrega os itens 
         * associados e os conteúdos da playlist. O método é assíncrono para melhorar a 
         * performance em operações de leitura.
         * Parâmetros:
         *   - id: O ID da playlist a ser retornada.
         * Retorno:
         *   - A playlist com seus itens e conteúdos, ou null se não encontrada.
         */
        public async Task<Playlist> GetPlaylistById(int id)
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .FirstOrDefaultAsync(p => p.Id == id);
        }

        /* 
         * Método: AddPlaylist
         * Descrição: 
         * Adiciona uma nova playlist ao banco de dados. O método é assíncrono para que 
         * a operação de inserção ocorra de maneira não bloqueante, garantindo melhor 
         * desempenho em operações de escrita.
         * Parâmetros:
         *   - playlist: O objeto Playlist que será adicionado ao banco de dados.
         */
        public async Task AddPlaylist(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist); // Assíncrono
            await _context.SaveChangesAsync(); // Salva as mudanças de forma assíncrona
        }

        /* 
         * Método: UpdatePlaylist
         * Descrição: 
         * Atualiza uma playlist existente no banco de dados. O método altera os dados da 
         * playlist e os salva de maneira assíncrona, garantindo que o estado atualizado seja 
         * persistido no banco de dados.
         * Parâmetros:
         *   - playlist: O objeto Playlist contendo os novos dados a serem atualizados.
         */
        public async Task UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist); // Marca a playlist como modificada
            await _context.SaveChangesAsync(); // Salva as mudanças de forma assíncrona
        }

        /* 
         * Método: DeletePlaylist
         * Descrição: 
         * Remove uma playlist do banco de dados com base no seu ID. A operação é assíncrona 
         * para garantir que a exclusão ocorra sem bloquear a execução de outras operações. 
         * A playlist será removida se for encontrada no banco de dados.
         * Parâmetros:
         *   - id: O ID da playlist a ser removida.
         */
        public async Task DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id); // Localiza a playlist
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist); // Remove a playlist
                await _context.SaveChangesAsync(); // Salva as mudanças de forma assíncrona
            }
        }
    }
}
