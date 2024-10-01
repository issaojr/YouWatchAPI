/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Program" é o ponto de entrada principal da aplicação YouWatchAPI. 
 * Ela configura os serviços necessários no contêiner de dependência, incluindo 
 * o Entity Framework para acessar o banco de dados, o serviço de API PlaylistService, 
 * o repositório PlaylistRepository e o Swagger para a documentação da API.
 * Além disso, o código define o pipeline de requisição HTTP, garantindo que 
 * o redirecionamento HTTPS, a autorização e o roteamento de controladores estejam habilitados.
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

            // Adicionar os serviços ao contêiner.

            builder.Services.AddControllers();
            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Adicionando o contexto do banco de dados (Entity Framework)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Injeção de dependência do PlaylistService e PlaylistRepository
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            var app = builder.Build();

            // Configure o pipeline de requisição HTTP.
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
