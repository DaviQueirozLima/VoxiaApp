using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Voxia.Application.Services;
using Voxia.Application.UseCases;
using Voxia.Application.UseCases.Auth;
using Voxia.Application.UseCases.Cards;
using Voxia.Domain.HttpContext;
using Voxia.Domain.Repositories.CardsRepositories;
using Voxia.Domain.Repositories.CategoriaRepositories;
using Voxia.Domain.Repositories.GoogleRepositories;
using Voxia.Infrastructure.Data;
using Voxia.Infrastructure.HttpContext;
using Voxia.Infrastructure.Repositories;
using Voxia.Infrastructure.Repositories.CategoriaRepositories;
using Voxia.Infrastructure.Repositories.GoogleRepositories;

var builder = WebApplication.CreateBuilder(args);

// ==================== Serviços ====================

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados (Supabase / PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Voxia.Infrastructure"))
);

// Google Auth
var googleClientIds = builder.Configuration
    .GetSection("GoogleAuth:ClientIds")
    .Get<string[]>();

builder.Services.AddSingleton(new GoogleAuthService(googleClientIds!));

// Injeção de dependências
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGoogleLoginUseCase, GoogleLoginUseCase>();
builder.Services.AddScoped<IGenerateJwtUseCase>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new GenerateJwtUseCase(configuration["Jwt:Key"]!);
});
builder.Services.AddScoped<ICardsRepositories, CardsRepositories>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

// JWT
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

// Swagger com JWT
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
        { securityScheme, new string[] { } }
    });
});

// CORS (para o app mobile)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobile",
        policy => policy
            .WithOrigins(
                "http://localhost:19006",
                "http://localhost:8081",
                "exp://127.0.0.1:19000",
                "https://seuappmobile.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// ==================== App ====================

var app = builder.Build();

// Middleware de arquivos estáticos (Assets)
var assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
if (!Directory.Exists(assetsPath))
{
    Directory.CreateDirectory(assetsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(assetsPath),
    RequestPath = "/assets"
});

// Swagger (Render vai rodar em Production, então deixa sempre ligado)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoxiaApp v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("AllowMobile");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//  Migração automática (sem precisar do dotnet ef)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
