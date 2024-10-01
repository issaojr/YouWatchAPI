/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Conteudo" representa uma entidade na aplicação YouWatchAPI 
 * que contém informações sobre um conteúdo, como "Id", "Título" e "Tipo". 
 * Ela define uma relação de um-para-muitos com a entidade "Criador", 
 * onde um conteúdo pertence a um criador, e também uma relação de 
 * muitos-para-muitos com playlists, utilizando a classe "ItemPlaylist" 
 * para mapear essa associação.
 */


namespace YouWatchAPI.Models
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }

        // Relação com Criador: Um conteúdo pertence a um criador
        public int CriadorId { get; set; }
        public Criador Criador { get; set; }

        // Relação de muitos-para-muitos: Um conteúdo pode estar em várias playlists
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}
