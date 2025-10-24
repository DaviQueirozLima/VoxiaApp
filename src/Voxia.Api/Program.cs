using Microsoft.EntityFrameworkCore;
using Voxia.Application.Services;
using Voxia.Application.UseCases.Auth;
using Voxia.Domain.Repositories.GoogleRepositories;
using Voxia.Infrastructure.Data;
using Voxia.Infrastructure.Repositories.GoogleRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger (documentação e testes da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly("Voxia.Infrastructure"))
);

// Injeções de dependência
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddSingleton(new GoogleAuthService("9415405413-kv69kvl6ieqq7gvjmsh8dd33aknkfsnk.apps.googleusercontent.com"));
builder.Services.AddScoped<IGoogleLoginUseCase, GoogleLoginUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



