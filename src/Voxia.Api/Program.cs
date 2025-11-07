using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
builder.Services.AddScoped<IGenerateJwtUseCase>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new GenerateJwtUseCase(configuration["Jwt:Key"]!);
});

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



