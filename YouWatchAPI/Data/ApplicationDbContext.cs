/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "ApplicationDbContext" é responsável por gerenciar o acesso ao banco de dados 
 * na aplicação YouWatchAPI utilizando o Entity Framework Core. Ela define DbSets para as 
 * entidades principais, como "Usuario", "Playlist", "Conteudo", "Criador" e "ItemPlaylist", 
 * permitindo que essas entidades sejam mapeadas para tabelas no banco de dados.
 * O método "OnModelCreating" configura uma chave composta para a tabela associativa 
 * "ItemPlaylist" e define as relações muitos-para-muitos entre playlists e conteúdos.
 */

using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Models;

namespace YouWatchAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /* 
         * DbSets: 
         * Cada DbSet representa uma tabela no banco de dados. As entidades "Usuario", "Playlist", 
         * "Conteudo", "Criador" e "ItemPlaylist" serão mapeadas para suas respectivas tabelas.
         * O Entity Framework utiliza esses DbSets para executar consultas e operações CRUD no banco de dados.
         */
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Criador> Criadores { get; set; }
        public DbSet<ItemPlaylist> ItemPlaylists { get; set; }

        /* 
         * Método: OnModelCreating
         * Descrição: 
         * Configura o mapeamento das entidades e as relações entre elas. Neste método, 
         * configuramos as chaves compostas e definimos as relações muitos-para-muitos 
         * entre playlists e conteúdos usando a entidade associativa "ItemPlaylist".
         * Parâmetros:
         *   - modelBuilder: Utilizado para configurar as propriedades das entidades e as chaves.
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da chave composta para ItemPlaylist (tabela associativa)
            modelBuilder.Entity<ItemPlaylist>()
                .HasKey(ip => new { ip.PlaylistId, ip.ConteudoId });

            /* 
             * Configuração da relação entre Playlist e ItemPlaylist:
             * Uma playlist pode ter muitos itens (ItemPlaylists), e cada ItemPlaylist 
             * está associado a uma playlist específica (chave estrangeira PlaylistId).
             */
            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Playlist)
                .WithMany(p => p.ItemPlaylists)
                .HasForeignKey(ip => ip.PlaylistId);

            /* 
             * Configuração da relação entre Conteudo e ItemPlaylist:
             * Um conteúdo pode estar associado a muitos itens (ItemPlaylists), e cada 
             * ItemPlaylist está associado a um conteúdo específico (chave estrangeira ConteudoId).
             */
            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Conteudo)
                .WithMany(c => c.ItemPlaylists)
                .HasForeignKey(ip => ip.ConteudoId);
        }
    }
}
