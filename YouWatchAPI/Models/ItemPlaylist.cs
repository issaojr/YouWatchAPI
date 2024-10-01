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
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int ConteudoId { get; set; }
        public Conteudo Conteudo { get; set; }
    }
}
