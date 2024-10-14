/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Playlist" representa uma entidade na aplicação YouWatchAPI 
 * que contém informações sobre uma playlist, como "Id" e "Nome". A classe 
 * define uma relação de um-para-muitos com a entidade "Usuario", indicando 
 * que várias playlists pertencem a um usuário. Além disso, ela estabelece 
 * uma relação de muitos-para-muitos com a entidade "Conteudo", utilizando 
 * a classe "ItemPlaylist" para mapear os conteúdos associados a uma playlist.
 */

namespace YouWatchAPI.Models
{
    public class Playlist
    {
        /* 
         * Propriedade: Id
         * Descrição: 
         * O identificador único da playlist no banco de dados. Esta é a chave primária da entidade.
         */
        public int Id { get; set; }

        /* 
         * Propriedade: Nome
         * Descrição: 
         * O nome da playlist. Este campo armazena o nome descritivo da playlist criada pelo usuário.
         */
        public string Nome { get; set; }

        /* 
         * Propriedade: UsuarioId
         * Descrição: 
         * O identificador do usuário ao qual esta playlist pertence. Esta propriedade é uma chave estrangeira 
         * que mapeia a relação de um-para-muitos entre "Playlist" e "Usuario".
         */
        public int UsuarioId { get; set; }

        /* 
         * Propriedade: Usuario
         * Descrição: 
         * A entidade "Usuario" à qual esta playlist pertence. Representa o proprietário da playlist, 
         * onde um usuário pode ter várias playlists (relação de um-para-muitos).
         */
        public Usuario Usuario { get; set; }

        /* 
         * Propriedade: ItemPlaylists
         * Descrição: 
         * Uma lista de "ItemPlaylist" que mapeia a relação muitos-para-muitos entre playlists e conteúdos.
         * Uma playlist pode conter vários conteúdos, e essa relação é gerenciada por "ItemPlaylist".
         */
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}
