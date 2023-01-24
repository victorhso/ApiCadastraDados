using ApiCadastraDados.Application;
using ApiCadastraDados.Application.HttpFactory;
using ApiCadastraDados.Domain.Dtos;
using ApiCadastraDados.Domain.Model;
using ApiCadastraDados.Domain.Repository;
using ApiCadastraDados.Domain.Services;
using ApiCadastraDados.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ApiCadastraDados
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registro do pacote Swagger

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CadastraDadosAPI", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            // Classe de conexão
            // Acesse os valores do arquivo por meio de AppConnections.ConnectionsReader
            string cnn = this.Configuration.GetConnectionString("SqlConnectionString");

            services.AddDbContext<Context>(options => options.UseSqlServer(cnn));

            //Instaciar configurações do RabbitMq
            services.Configure<RabbitMQConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddSingleton<IRabbitMQRepository, RabbitMQRepository>();
            services.AddSingleton<IPessoaRepository, PessoaRepository>();
            services.AddSingleton<ITelefoneRepository, TelefoneRepository>();
            services.AddSingleton<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<IHttpFactoryService, HttpFactoryService>();
            services.AddTransient<IRegistroRepository, RegistroLogErroRepository>();
            services.AddTransient<IDadosService, DadosService>();

            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            //Adicionado campo para se evitar que campos nulos sejam retornado como resposa na API
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.IgnoreNullValues = true);

            var config = new MapperConfiguration(conf =>
            {
                conf.CreateMap<DadosCadastroDto, DadosCadastro>();
                
                conf.CreateMap<DadosCadastroDtoPost, DadosCadastro>();

                conf.CreateMap<PessoaDto, Pessoa>();
                conf.CreateMap<Pessoa, PessoaDto>();

                conf.CreateMap<EnderecoDto, Endereco>();
                conf.CreateMap<Endereco, EnderecoDto>();

                conf.CreateMap<EnderecoDtoPost, Endereco>();
                conf.CreateMap<Endereco, EnderecoDtoPost>();

                conf.CreateMap<TelefoneDto, Telefone>();
                conf.CreateMap<Telefone, TelefoneDto>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<ConnStrings>(Configuration.GetSection("ConnectionStrings"));
            ConnStrings connStrings = Configuration.GetSection("ConnectionStrings").Get<ConnStrings>();
            services.AddSingleton<ConnStrings>(connStrings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Swagger
            app.UseSwagger();

            // Indica o endpoint do swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCadastraDados v1");
                c.RoutePrefix = "swagger";
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
