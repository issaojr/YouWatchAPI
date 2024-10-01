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


namespace YouWatchAPI.Models
{
    public class Criador
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Relação de um-para-muitos: Um criador pode ter vários conteúdos
        public List<Conteudo> Conteudos { get; set; }
    }
}
