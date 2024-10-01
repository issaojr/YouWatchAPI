/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "PlaylistService" fornece a camada de serviço para manipular 
 * as operações relacionadas a "Playlist" na aplicação YouWatchAPI. Ela 
 * utiliza o repositório "PlaylistRepository" para realizar operações 
 * assíncronas como obter todas as playlists, recuperar uma playlist por ID, 
 * adicionar, atualizar e deletar playlists. O uso do serviço abstrai a 
 * lógica de negócio e facilita a manutenção e reutilização do código.
 */


using System.Collections.Generic;
using System.Threading.Tasks;
using YouWatchAPI.Data.Repositories;
using YouWatchAPI.Models;

namespace YouWatchAPI.Services
{
    public class PlaylistService
    {
        private readonly PlaylistRepository _playlistRepository;

        // Injeção de dependência do repositório
        public PlaylistService(PlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        // Retornar todas as playlists
        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _playlistRepository.GetAllPlaylistsAsync();
        }

        // Retornar uma playlist por ID
        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            return await _playlistRepository.GetPlaylistById(id);
        }

        // Adicionar uma nova playlist
        public async Task<Playlist> AddPlaylistAsync(Playlist playlist)
        {
            await _playlistRepository.AddPlaylist(playlist);
            return playlist;
        }

        // Atualizar uma playlist existente
        public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
        {
            var existingPlaylist = await _playlistRepository.GetPlaylistById(playlist.Id);
            if (existingPlaylist == null)
            {
                return false; // Retornar falso se a playlist não for encontrada
            }

            await _playlistRepository.UpdatePlaylist(playlist);
            return true;
        }

        // Deletar uma playlist
        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var existingPlaylist = await _playlistRepository.GetPlaylistById(id);
            if (existingPlaylist == null)
            {
                return false; // Retornar falso se a playlist não for encontrada
            }

            await _playlistRepository.DeletePlaylist(id);
            return true;
        }
    }
}
