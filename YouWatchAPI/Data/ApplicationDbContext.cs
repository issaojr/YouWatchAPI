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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Criador> Criadores { get; set; }
        public DbSet<ItemPlaylist> ItemPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da chave composta para ItemPlaylist (associativa)
            modelBuilder.Entity<ItemPlaylist>()
                .HasKey(ip => new { ip.PlaylistId, ip.ConteudoId });

            // Configuração das relações muitos-para-muitos
            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Playlist)
                .WithMany(p => p.ItemPlaylists)
                .HasForeignKey(ip => ip.PlaylistId);

            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Conteudo)
                .WithMany(c => c.ItemPlaylists)
                .HasForeignKey(ip => ip.ConteudoId);
        }
    }
}
