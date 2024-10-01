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

        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .ToListAsync(); // Use ToListAsync() para retornos assíncronos
        }

        // Método para retornar uma playlist por ID
        public async Task<Playlist> GetPlaylistById(int id)
        {
            return await _context.Playlists
                             .Include(p => p.ItemPlaylists)
                             .ThenInclude(ip => ip.Conteudo)
                             .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Método para adicionar uma nova playlist
        public async Task AddPlaylist(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist); // Assíncrono
            await _context.SaveChangesAsync(); // Assíncrono
        }

        // Método para atualizar uma playlist
        public async Task UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync(); // Assíncrono
        }

        // Método para deletar uma playlist
        public async Task DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id); // Assíncrono
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync(); // Assíncrono
            }
        }
    }
}
