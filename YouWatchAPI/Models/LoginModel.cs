/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "LoginModel" é utilizada para a autenticação de usuários na aplicação YouWatchAPI. 
 * Ela define as propriedades básicas que um usuário deve fornecer ao realizar login, como "Email" e "Senha". 
 * A classe também contém anotações de validação de dados para garantir que os campos fornecidos 
 * estejam no formato adequado, como o formato de e-mail correto e o comprimento mínimo da senha.
 */

using System.ComponentModel.DataAnnotations;

namespace YouWatchAPI.Models
{
    public class LoginModel
    {
        /* 
         * Propriedade: Email
         * Descrição: 
         * O endereço de e-mail do usuário. Este campo é obrigatório e deve ser informado no formato de e-mail válido.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [EmailAddress]: Valida que o valor informado esteja no formato correto de e-mail.
         */
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /* 
         * Propriedade: Senha
         * Descrição: 
         * A senha do usuário. Este campo é obrigatório e deve ter no mínimo 6 caracteres.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [MinLength(6)]: Exige que a senha tenha pelo menos 6 caracteres.
         */
        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
