using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Panda.SEOTracker.API.Extensions;
using Panda.SEOTracker.BusinessLogic.SearchTermHistoryLogic;
using Panda.SEOTracker.BusinessLogic.Services;
using Panda.SEOTracker.BusinessLogic.TrackedUrlLogic;
using Panda.SEOTracker.DataAccess;
using Panda.SEOTracker.Infrastructure.Repositories;
using Panda.SEOTracker.Infrastructure.Services;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
try
{
	AddSerilog();
	ConfigureServices();
	CreateApplication();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
	throw;
}
finally
{
	Log.CloseAndFlush();
}

void ConfigureServices()
{
	builder.Services.Configure<ApiBehaviorOptions>(options =>
		options.SuppressMapClientErrors = true);

	builder.Services.Configure<IISServerOptions>(options =>
		options.MaxRequestBodySize = int.MaxValue);

	AddDataRepositories();
	builder.Services.AddTransient<ISearchEngineService, SearchEngineService>();
	builder.Services.AddControllers();
	builder.Services.AddOpenApiDocument(document =>
	{
		document.DocumentName = "SEO Tracker API";
		document.Title = "SEO Tracker API";
	}); // add OpenAPI v3 document
}
void CreateApplication()
{
	var app = builder.Build();
	app.UseCors(x =>
	{
		x.AllowAnyOrigin();
		x.AllowAnyHeader();
		x.AllowAnyMethod();
	});
	app.UseGlobalErrorHandler();
	app.UseHttpsRedirection();

	app.UseOpenApi(config => config.Path = "/swagger/{documentName}/swagger.json");
	app.UseSwaggerUi3(config => config.ValidateSpecification = true);
	app.UseReDoc();

	app.MapControllers();

	app.Run();
}
void AddDataRepositories()
{
	builder.Services.AddDbContext<SEOTrackerDbContext>(
		options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("SEOTrackerDb")));

	builder.Services.AddScoped<ITrackedUrlRepository, TrackedUrlRepository>();
	builder.Services.AddScoped<ISearchTermHistoryRepository, SearchTermHistoryRepository>();
}
void AddSerilog()
{
	var configBuilder = new ConfigurationBuilder();
	var isDevelopment = builder.Environment.IsDevelopment();

	configBuilder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile($"appsettings{(isDevelopment ? ".Development" : "")}.json",
					optional: isDevelopment,
					reloadOnChange: true)
				.AddEnvironmentVariables();

	var logger = new LoggerConfiguration()
	  .ReadFrom.Configuration(configBuilder.Build())
	  .Enrich.FromLogContext()
	  .CreateLogger();

	Log.Logger = logger;

	builder.Logging.ClearProviders();
	builder.Logging.AddSerilog(logger);
}