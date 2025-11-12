using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using Voxia.Application.Services;
using Voxia.Application.UseCases.Auth;
using Voxia.Application.UseCases.Cards;
using Voxia.Domain.HttpContext;
using Voxia.Domain.Repositories.CardsRepositories;
using Voxia.Domain.Repositories.GoogleRepositories;
using Voxia.Infrastructure.Data;
using Voxia.Infrastructure.HttpContext;
using Voxia.Infrastructure.Repositories;
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
builder.Services.AddScoped<IGenerateJwtUseCase>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new GenerateJwtUseCase(configuration["Jwt:Key"]!);
});
builder.Services.AddScoped<ICardsRepositories, CardsRepositories>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddHttpContextAccessor(); // necessário para IHttpContextAccessor
builder.Services.AddScoped<IUserContext, UserContext>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Voxia API",
        Version = "v1",
        Description = "API do Voxia — sistema de apoio para pessoas autistas"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT no formato: **Bearer {seu_token}**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme, new string[] { }
        }
    });
});

var app = builder.Build();

// Configurar arquivos estáticos para a pasta Assets
app.UseStaticFiles(); // mantém wwwroot padrão (se existir)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
    RequestPath = "/assets"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoxiaApp v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
