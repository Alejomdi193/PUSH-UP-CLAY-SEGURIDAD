using System.Reflection;
using Api.Helpers;
using Api.Extensions;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistencia;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
					.ReadFrom.Configuration(builder.Configuration)
					.Enrich.FromLogContext()
					.CreateLogger();

builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAplicacionServices();
builder.Services.ConfigureRatelimiting();
builder.Services.ConfigureApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Garden's - DbFirst", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ConfigureCors();
builder.Services.AddDbContext<PushContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("ConexMysql");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddJwt(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	try
	{
		var context = services.GetRequiredService<PushContext>();
		await context.Database.MigrateAsync();

	}
	catch (Exception ex)
	{
		var _logger = loggerFactory.CreateLogger<Program>();
		_logger.LogError(ex, "Ocurrio un error durante la migracion!");
	}
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
