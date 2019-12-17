using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using TesteAcesso.Handler;
using TesteAcesso.Handler.Behaviors;
using TesteAcesso.HttpService.Acesso;
using TesteAcesso.Model.Interfaces;
using TesteAcesso.Repository.Entity;
using TesteAcesso.Repository.Infrastructure;
using TesteAcesso.Repository.Initialize;

namespace TesteAcesso.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            //SERILOG
            var builder = new ConfigurationBuilder()
             .SetBasePath(hostingEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();

            var elasticUri = Configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()               
            .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //REPOSITORY INJECTION
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IAcessoAccountService, AcessoAccountService>();

            //DATABASE INJECTION
            var connection = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<Context>(options =>
                options.UseMySQL(connection)
            );

            //MEDIATR
            AddMediatr(services);

            //SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Teste Acesso",
                        Version = "v1",
                        Description = "",
                        Contact = new Contact
                        {
                            Name = "Fabio Guedes dos Santos",
                            Url = "https://github.com/fabioguitar83"
                        }
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Context contexto, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //ENABLING MIDDLEWARE FOR USE BY SWAGGER 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Teste Acesso");
            });

            InicializaBD.Initialize(contexto);

            //SERILOG
            loggerFactory.AddSerilog();         

        }

        private static void AddMediatr(IServiceCollection services)
        {
            //VALIDATORS TO MEDIATR
            const string applicationAssemblyName = "TesteAcesso.Handler";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //MEDIATR
            services.AddMediatR(typeof(Startup), typeof(TransactionHandler));
        }

    }
}
