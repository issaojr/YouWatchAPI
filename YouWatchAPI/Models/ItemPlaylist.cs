/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "ItemPlaylist" representa a relação entre "Playlist" e "Conteudo" 
 * na aplicação YouWatchAPI. Ela mapeia os itens de uma playlist, associando 
 * cada conteúdo a uma playlist específica. Cada instância desta classe contém 
 * as chaves estrangeiras para "Playlist" e "Conteudo", permitindo gerenciar a 
 * relação entre essas duas entidades no banco de dados.
 */

namespace YouWatchAPI.Models
{
    public class ItemPlaylist
    {
        /* 
         * Propriedade: PlaylistId
         * Descrição: 
         * O identificador da playlist à qual este item pertence. Esta é uma chave estrangeira 
         * que mapeia a relação entre "ItemPlaylist" e "Playlist".
         */
        public int PlaylistId { get; set; }

        /* 
         * Propriedade: Playlist
         * Descrição: 
         * A entidade Playlist associada a este item. Representa a playlist específica 
         * à qual este item (conteúdo) pertence.
         */
        public Playlist Playlist { get; set; }

        /* 
         * Propriedade: ConteudoId
         * Descrição: 
         * O identificador do conteúdo associado a este item. Esta é uma chave estrangeira 
         * que mapeia a relação entre "ItemPlaylist" e "Conteudo".
         */
        public int ConteudoId { get; set; }

        /* 
         * Propriedade: Conteudo
         * Descrição: 
         * A entidade Conteudo associada a este item. Representa o conteúdo específico 
         * que está incluído na playlist.
         */
        public Conteudo Conteudo { get; set; }
    }
}
