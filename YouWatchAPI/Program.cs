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

namespace YouWatchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar os serviços ao contêiner.

            /* 
             * Serviço: AddControllers
             * Descrição:
             * Adiciona o suporte para os controladores da API. Permite o uso do padrão MVC sem views (para APIs),
             * onde os controladores processam as requisições HTTP.
             */
            builder.Services.AddControllers();

            /* 
             * Serviço: Swagger/OpenAPI
             * Descrição:
             * Adiciona suporte ao Swagger, permitindo a geração automática da documentação da API
             * e o acesso à interface Swagger UI para testar endpoints.
             */
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /* 
             * Serviço: AddDbContext
             * Descrição:
             * Configura o Entity Framework para usar o SQL Server como o banco de dados. 
             * O contexto do banco de dados (ApplicationDbContext) é injetado e configurado 
             * com a string de conexão armazenada no arquivo de configuração.
             */
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* 
             * Serviço: Injeção de dependência
             * Descrição:
             * Injeção de dependência do PlaylistService e PlaylistRepository, 
             * tornando-os disponíveis para outros serviços e controladores que precisarem.
             */
            builder.Services.AddScoped<PlaylistService>();
            builder.Services.AddScoped<PlaylistRepository>();

            /* 
             * Configuração: Autenticação JWT
             * Descrição:
             * Configura o esquema de autenticação JWT (JSON Web Token). O JWT é utilizado para 
             * autenticar e autorizar usuários, onde um token é emitido após o login e incluído 
             * nos cabeçalhos das requisições subsequentes para verificar a identidade do usuário.
             * A chave secreta usada para assinar o token e outros parâmetros são configurados.
             */
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Valida o emissor do token
                        ValidateAudience = true, // Valida a audiência do token
                        ValidateLifetime = true, // Valida o tempo de expiração do token
                        ValidateIssuerSigningKey = true, // Valida a chave de assinatura
                        ValidIssuer = builder.Configuration["Jwt:Issuer"], // O emissor permitido
                        ValidAudience = builder.Configuration["Jwt:Audience"], // A audiência permitida
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // A chave de assinatura
                    };
                });

            var app = builder.Build();

            // Configuração do pipeline de requisição HTTP.

            /* 
             * Ambiente de desenvolvimento: Swagger
             * Descrição:
             * Se a aplicação estiver no ambiente de desenvolvimento, o Swagger será habilitado.
             * Isso permite testar os endpoints da API e visualizar a documentação gerada automaticamente.
             */
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /* 
             * Middleware: HTTPS Redirection
             * Descrição:
             * Garante que todas as requisições HTTP sejam redirecionadas para HTTPS, 
             * aumentando a segurança das requisições na aplicação.
             */
            app.UseHttpsRedirection();

            /* 
             * Middleware: Autenticação e Autorização
             * Descrição:
             * Configura o middleware de autenticação (UseAuthentication) e autorização (UseAuthorization).
             * A autenticação verifica a identidade do usuário com base no token JWT, e a autorização
             * garante que apenas usuários com permissões adequadas possam acessar determinados recursos.
             */
            app.UseAuthentication(); // Verifica a identidade do usuário
            app.UseAuthorization();  // Verifica as permissões do usuário

            /* 
             * Mapeamento de controladores
             * Descrição:
             * Habilita o roteamento de requisições para os controladores da API.
             */
            app.MapControllers();

            // Executa a aplicação
            app.Run();
        }
    }
}
