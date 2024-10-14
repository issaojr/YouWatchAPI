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
    public class PlaylistService(PlaylistRepository playlistRepository)
    {
        private readonly PlaylistRepository _playlistRepository = playlistRepository;

        /* 
         * Método: GetAllPlaylistsAsync
         * Descrição: 
         * Retorna todas as playlists do banco de dados de forma assíncrona.
         * Este método delega a operação ao repositório e retorna uma lista de playlists.
         */
        public async Task<List<Playlist>> GetAllPlaylistsAsync()
        {
            return await _playlistRepository.GetAllPlaylistsAsync();
        }

        /* 
         * Método: GetPlaylistByIdAsync
         * Descrição: 
         * Retorna uma playlist específica com base no ID informado. 
         * A operação é assíncrona e delegada ao repositório.
         * Parâmetros:
         *   - id: O identificador da playlist que será retornada.
         * Retorno:
         *   - A playlist correspondente ao ID, ou null se não encontrada.
         */
        public async Task<Playlist> GetPlaylistByIdAsync(int id)
        {
            return await _playlistRepository.GetPlaylistById(id);
        }

        /* 
         * Método: AddPlaylistAsync
         * Descrição: 
         * Adiciona uma nova playlist ao banco de dados. A operação é assíncrona 
         * e o método delega o processo de inserção ao repositório.
         * Parâmetros:
         *   - playlist: O objeto Playlist que será adicionado ao banco de dados.
         * Retorno:
         *   - A playlist recém-adicionada.
         */
        public async Task<Playlist> AddPlaylistAsync(Playlist playlist)
        {
            await _playlistRepository.AddPlaylist(playlist);
            return playlist;
        }

        /* 
         * Método: UpdatePlaylistAsync
         * Descrição: 
         * Atualiza uma playlist existente no banco de dados. Primeiro, verifica se a playlist 
         * existe. Se encontrada, delega a atualização ao repositório. Se não for encontrada, 
         * retorna falso.
         * Parâmetros:
         *   - playlist: O objeto Playlist que contém os dados atualizados.
         * Retorno:
         *   - true se a playlist foi atualizada, false se a playlist não foi encontrada.
         */
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

        /* 
         * Método: DeletePlaylistAsync
         * Descrição: 
         * Deleta uma playlist do banco de dados com base no ID informado. Primeiro, verifica se 
         * a playlist existe. Se encontrada, delega a remoção ao repositório. Se não for encontrada, 
         * retorna falso.
         * Parâmetros:
         *   - id: O identificador da playlist a ser deletada.
         * Retorno:
         *   - true se a playlist foi deletada, false se a playlist não foi encontrada.
         */
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
