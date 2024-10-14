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

namespace YouWatchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar os servi�os ao cont�iner.

            /* 
             * Servi�o: AddControllers
             * Descri��o:
             * Adiciona o suporte para os controladores da API. Permite o uso do padr�o MVC sem views (para APIs),
             * onde os controladores processam as requisi��es HTTP.
             */
            builder.Services.AddControllers();

            /* 
             * Servi�o: Swagger/OpenAPI
             * Descri��o:
             * Adiciona suporte ao Swagger, permitindo a gera��o autom�tica da documenta��o da API
             * e o acesso � interface Swagger UI para testar endpoints.
             */
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* 
             * Servi�o: AddDbContext
             * Descri��o:
             * Configura o Entity Framework para usar o SQL Server como o banco de dados. 
             * O contexto do banco de dados (ApplicationDbContext) � injetado e configurado 
             * com a string de conex�o armazenada no arquivo de configura��o.
             */
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* 
             * Servi�o: Inje��o de depend�ncia
             * Descri��o:
             * Inje��o de depend�ncia do PlaylistService e PlaylistRepository, 
             * tornando-os dispon�veis para outros servi�os e controladores que precisarem.
             */
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            /* 
             * Configura��o: Autentica��o JWT
             * Descri��o:
             * Configura o esquema de autentica��o JWT (JSON Web Token). O JWT � utilizado para 
             * autenticar e autorizar usu�rios, onde um token � emitido ap�s o login e inclu�do 
             * nos cabe�alhos das requisi��es subsequentes para verificar a identidade do usu�rio.
             * A chave secreta usada para assinar o token e outros par�metros s�o configurados.
             */
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Valida o emissor do token
                        ValidateAudience = true, // Valida a audi�ncia do token
                        ValidateLifetime = true, // Valida o tempo de expira��o do token
                        ValidateIssuerSigningKey = true, // Valida a chave de assinatura
                        ValidIssuer = builder.Configuration["Jwt:Issuer"], // O emissor permitido
                        ValidAudience = builder.Configuration["Jwt:Audience"], // A audi�ncia permitida
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // A chave de assinatura
                    };
                });

            var app = builder.Build();

            // Configura��o do pipeline de requisi��o HTTP.

            /* 
             * Ambiente de desenvolvimento: Swagger
             * Descri��o:
             * Se a aplica��o estiver no ambiente de desenvolvimento, o Swagger ser� habilitado.
             * Isso permite testar os endpoints da API e visualizar a documenta��o gerada automaticamente.
             */
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /* 
             * Middleware: HTTPS Redirection
             * Descri��o:
             * Garante que todas as requisi��es HTTP sejam redirecionadas para HTTPS, 
             * aumentando a seguran�a das requisi��es na aplica��o.
             */
            app.UseHttpsRedirection();

            /* 
             * Middleware: Autentica��o e Autoriza��o
             * Descri��o:
             * Configura o middleware de autentica��o (UseAuthentication) e autoriza��o (UseAuthorization).
             * A autentica��o verifica a identidade do usu�rio com base no token JWT, e a autoriza��o
             * garante que apenas usu�rios com permiss�es adequadas possam acessar determinados recursos.
             */
            app.UseAuthentication(); // Verifica a identidade do usu�rio
            app.UseAuthorization();  // Verifica as permiss�es do usu�rio

            /* 
             * Mapeamento de controladores
             * Descri��o:
             * Habilita o roteamento de requisi��es para os controladores da API.
             */
            app.MapControllers();

            // Executa a aplica��o
            app.Run();
        }
    }
}
