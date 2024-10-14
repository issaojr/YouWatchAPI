/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Esta classe "Startup" é responsável por configurar os serviços e o pipeline de requisições HTTP
 * da aplicação YouWatchAPI. Ela utiliza o padrão de injeção de dependências para adicionar 
 * os serviços ao contêiner, como o Entity Framework e o Swagger, e também configura o pipeline
 * para tratar as requisições, garantindo que a API funcione corretamente.
 */

using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Data.Repositories;
using YouWatchAPI.Data;
using YouWatchAPI.Services;

namespace YouWatchAPI
{
    public class Startup
    {
        /* 
         * Construtor: Startup
         * Descrição: 
         * O construtor recebe a configuração da aplicação via injeção de dependência. 
         * A propriedade "Configuration" contém todas as configurações da aplicação, como 
         * as strings de conexão e as configurações do JWT.
         */
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /* 
         * Método: ConfigureServices
         * Descrição: 
         * Este método é responsável por adicionar serviços ao contêiner de injeção de dependências.
         * Aqui, configuramos o Entity Framework, Swagger para a documentação da API, e os serviços
         * da aplicação como PlaylistService e PlaylistRepository.
         * Parâmetro:
         *   - services: O contêiner de serviços onde os serviços são registrados para injeção.
         */
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração do Entity Framework e do banco de dados
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Adiciona suporte para controladores
            services.AddControllers();

            // Configuração do Swagger para documentação da API
            services.AddSwaggerGen();

            // Injeção de dependências
            services.AddScoped<PlaylistService>();
            services.AddScoped<PlaylistRepository>();
        }

        /* 
         * Método: Configure
         * Descrição: 
         * Este método é usado para configurar o pipeline de requisições HTTP da aplicação.
         * Aqui, configuramos o roteamento, redirecionamento HTTPS, e ativamos o Swagger 
         * durante o desenvolvimento para facilitar o teste e a documentação da API.
         * Parâmetros:
         *   - app: O builder de aplicativos que define como as requisições são tratadas.
         *   - env: O ambiente de execução, que permite identificar se a aplicação está em desenvolvimento ou produção.
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Se estiver no ambiente de desenvolvimento, exibe a página de exceções detalhadas
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Ativa o Swagger durante o desenvolvimento
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "YouWatchAPI v1");
                });
            }

            // Middleware que redireciona todas as requisições HTTP para HTTPS
            app.UseHttpsRedirection();

            // Middleware para configurar o roteamento
            app.UseRouting();

            // Middleware que habilita a autorização
            app.UseAuthorization();

            // Configura o mapeamento dos endpoints da API
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
