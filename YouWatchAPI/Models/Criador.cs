/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Criador" representa a entidade responsável pela criação de conteúdos 
 * na aplicação YouWatchAPI. A classe define as propriedades básicas de um criador, 
 * como "Id" e "Nome", e estabelece uma relação de um-para-muitos com a entidade 
 * "Conteudo", indicando que um criador pode ser responsável por vários conteúdos.
 */

using System.ComponentModel.DataAnnotations;

namespace YouWatchAPI.Models
{
    public class Criador
    {
        /* 
         * Propriedade: Id
         * Descrição: 
         * O identificador único do criador no banco de dados. Esta é a chave primária da entidade.
         */
        public int Id { get; set; }

        /* 
         * Propriedade: Nome
         * Descrição: 
         * O nome do criador. Este campo é obrigatório e tem um limite máximo de 100 caracteres.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [MaxLength(100)]: Limita o nome a no máximo 100 caracteres.
         */
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        /* 
         * Propriedade: Email
         * Descrição: 
         * O endereço de e-mail do criador. Este campo é obrigatório e deve seguir o formato de um endereço de e-mail válido.
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
         * A senha do criador, usada para autenticação. A senha deve ter no mínimo 6 caracteres.
         * Validações:
         *   - [Required]: Indica que este campo é obrigatório.
         *   - [MinLength(6)]: Exige que a senha tenha pelo menos 6 caracteres.
         */
        [Required]
        [MinLength(6)]
        public string Senha { get; set; } // Campo de senha adicionado
    }
}
