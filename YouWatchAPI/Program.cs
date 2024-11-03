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
 * o redirecionamento HTTPS, a autenticação, autorização e o roteamento de controladores estejam habilitados.
 */

using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Data;
using YouWatchAPI.Services;
using YouWatchAPI.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

namespace YouWatchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar os serviços ao contêiner.

            /* Serviço: AddControllers */
            builder.Services.AddControllers();

            /* Serviço: Swagger/OpenAPI */
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* Serviço: AddDbContext */
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* Serviço: Injeção de dependência */
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            /* Serviço: Adicionar CORS */
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            /* Configuração: Autenticação JWT */
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            var app = builder.Build();

            // Configuração do pipeline de requisição HTTP.

            /* Ambiente de desenvolvimento: Swagger */
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /* Middleware: HTTPS Redirection */
            app.UseHttpsRedirection();

            /* Middleware: CORS */
            app.UseCors("AllowAll");

            /* Middleware: Autenticação e Autorização */
            app.UseAuthentication();
            app.UseAuthorization();

            /* Mapeamento de controladores */
            app.MapControllers();

            // Executa a aplicação
            app.Run();
        }
    }
}
