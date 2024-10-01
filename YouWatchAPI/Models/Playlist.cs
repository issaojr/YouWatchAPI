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
        public int Id { get; set; }
        public string Nome { get; set; }

        // Relação com o Usuário (muitas playlists pertencem a um usuário)
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relação de muitos-para-muitos: Uma playlist pode ter vários conteúdos
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}
