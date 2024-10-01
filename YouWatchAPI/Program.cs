/* 
 * Aluno: Issao Hanaoka Junior
 * Matr�cula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Gradua��o: Curso Superior de Tecnologia em An�lise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descri��o:
 * Esta classe "Program" � o ponto de entrada principal da aplica��o YouWatchAPI. 
 * Ela configura os servi�os necess�rios no cont�iner de depend�ncia, incluindo 
 * o Entity Framework para acessar o banco de dados, o servi�o de API PlaylistService, 
 * o reposit�rio PlaylistRepository e o Swagger para a documenta��o da API.
 * Al�m disso, o c�digo define o pipeline de requisi��o HTTP, garantindo que 
 * o redirecionamento HTTPS, a autoriza��o e o roteamento de controladores estejam habilitados.
 */


using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Data;
using YouWatchAPI.Services;
using YouWatchAPI.Data.Repositories;

namespace YouWatchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar os servi�os ao cont�iner.

            builder.Services.AddControllers();
            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Adicionando o contexto do banco de dados (Entity Framework)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Inje��o de depend�ncia do PlaylistService e PlaylistRepository
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            var app = builder.Build();

            // Configure o pipeline de requisi��o HTTP.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
