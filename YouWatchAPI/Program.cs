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
 * o redirecionamento HTTPS, a autentica��o, autoriza��o e o roteamento de controladores estejam habilitados.
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

            // Adicionar os servi�os ao cont�iner.

            /* Servi�o: AddControllers */
            builder.Services.AddControllers();

            /* Servi�o: Swagger/OpenAPI */
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* Servi�o: AddDbContext */
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* Servi�o: Inje��o de depend�ncia */
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            /* Servi�o: Adicionar CORS */
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            /* Configura��o: Autentica��o JWT */
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

            // Configura��o do pipeline de requisi��o HTTP.

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

            /* Middleware: Autentica��o e Autoriza��o */
            app.UseAuthentication();
            app.UseAuthorization();

            /* Mapeamento de controladores */
            app.MapControllers();

            // Executa a aplica��o
            app.Run();
        }
    }
}
