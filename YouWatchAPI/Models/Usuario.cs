/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Usuario" representa uma entidade na aplicação YouWatchAPI que 
 * contém as informações de um usuário, como "Id", "Nome" e "Email". Ela também 
 * define uma relação de um-para-muitos com a entidade "Playlist", indicando que 
 * um usuário pode ter várias playlists associadas a ele.
 */


namespace YouWatchAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        // Relação de um-para-muitos: Um usuário pode ter várias playlists
        public List<Playlist> Playlists { get; set; }
    }
}
