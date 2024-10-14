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
        /* 
         * Propriedade: Id
         * Descrição: 
         * O identificador único do conteúdo no banco de dados. Esta é a chave primária da entidade.
         */
        public int Id { get; set; }

        /* 
         * Propriedade: Titulo
         * Descrição: 
         * O título do conteúdo, representando seu nome ou descrição.
         */
        public string Titulo { get; set; }

        /* 
         * Propriedade: Tipo
         * Descrição: 
         * O tipo do conteúdo (por exemplo, vídeo, áudio, artigo, etc.).
         */
        public string Tipo { get; set; }

        /* 
         * Propriedade: CriadorId
         * Descrição: 
         * O identificador do criador associado a este conteúdo. Representa uma chave estrangeira 
         * que estabelece a relação entre "Conteudo" e "Criador".
         */
        public int CriadorId { get; set; }

        /* 
         * Propriedade: Criador
         * Descrição: 
         * A entidade Criador à qual este conteúdo pertence. Um conteúdo é sempre criado por um criador.
         * Relação de um-para-muitos: Um criador pode ter vários conteúdos, mas um conteúdo pertence a um criador.
         */
        public Criador Criador { get; set; }

        /* 
         * Propriedade: ItemPlaylists
         * Descrição: 
         * Uma lista de "ItemPlaylist" que associa este conteúdo a várias playlists.
         * Relação de muitos-para-muitos: Um conteúdo pode estar em várias playlists e 
         * uma playlist pode conter vários conteúdos.
         */
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}

