using Autofac;
using Autofac.Extensions.DependencyInjection;
using CrossCutting;
using Demo.GestaoEscolar.Domain.Services.Alunos;
using Demo.GestaoEscolar.Infra.Dapper;
using Demo.GestaoEscolar.Infra.EF;
using Demo.GestaoEscolar.WebApplication.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using DomainServicesAssembly = Demo.GestaoEscolar.Domain.Bar;
using HandlersAssembly = Demo.GestaoEscolar.Handlers.Foo;
using InfraDapperAssembly = Demo.GestaoEscolar.Infra.Dapper.Foo;
using InfraEFAssembly = Demo.GestaoEscolar.Infra.EF.Foo;

namespace Demo.GestaoEscolar.WebApplication
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IContainer Container { get; private set; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(x =>
			{
				x.EnableEndpointRouting = false;

			}).AddControllersAsServices();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
			});

			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("AppDatabase"));
				options.UseLazyLoadingProxies();
			});

			var builder = new ContainerBuilder();

			builder.Populate(services);

			ConfigureContainer(builder);

			Container = builder.Build();

			DomainEvents.Init(Container.BeginLifetimeScope());
		}

		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Gest�o Escolar API V1 {env.EnvironmentName}");
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterType<PessoaFisicaController>().PropertiesAutowired();
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
			builder.RegisterType<MessageBus>().As<IMessageBus>();

			builder.RegisterType<MatriculaService>().As<IMatriculaService>();

			builder.RegisterAssemblyTypes(typeof(InfraEFAssembly).Assembly)
					.Where(t => t.Name.EndsWith("Repository"))
					.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(typeof(DomainServicesAssembly).Assembly)
					.Where(t => t.Name.EndsWith("Service"))
					.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(typeof(InfraDapperAssembly).Assembly)
					.Where(t => t.Name.EndsWith("Finder"))
					.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(typeof(HandlersAssembly).Assembly)
					.AsClosedTypesOf(typeof(IHandler<>));

			builder.RegisterType<AppConnectionString>()
				.AsSelf()
				.WithParameter(new TypedParameter(typeof(string), Configuration.GetConnectionString("AppDatabase")));

		}
	}
}
