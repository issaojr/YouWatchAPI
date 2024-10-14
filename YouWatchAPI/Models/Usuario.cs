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

using System.ComponentModel.DataAnnotations;

namespace YouWatchAPI.Models
{
    public class Usuario
    {
        /* 
         * Propriedade: Id
         * Descrição: 
         * O identificador único do usuário no banco de dados. Esta é a chave primária da entidade.
         */
        public int Id { get; set; }

        /* 
         * Propriedade: Nome
         * Descrição: 
         * O nome completo do usuário. Este campo é obrigatório e possui um limite máximo de 100 caracteres.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [MaxLength(100)]: Limita o nome a no máximo 100 caracteres.
         */
        [Required]
        [MaxLength(100)] // Nome não pode exceder 100 caracteres
        public string Nome { get; set; }

        /* 
         * Propriedade: Email
         * Descrição: 
         * O endereço de e-mail do usuário. Este campo é obrigatório e deve ser informado no formato de e-mail válido.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [EmailAddress]: Valida que o valor informado esteja no formato correto de e-mail.
         */
        [Required]
        [EmailAddress] // Validação de e-mail
        public string Email { get; set; }

        /* 
         * Propriedade: Senha
         * Descrição: 
         * A senha do usuário para autenticação. Este campo é obrigatório e deve ter no mínimo 6 caracteres.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [MinLength(6)]: Exige que a senha tenha pelo menos 6 caracteres.
         */
        [Required]
        [MinLength(6)] // Definir uma senha com comprimento mínimo
        public string Senha { get; set; }

        /* 
         * Propriedade: Playlists
         * Descrição: 
         * Uma lista de playlists associadas ao usuário. Um usuário pode ter várias playlists.
         * Relação de um-para-muitos: Um usuário pode ser dono de várias playlists.
         */
        public List<Playlist> Playlists { get; set; }
    }
}
