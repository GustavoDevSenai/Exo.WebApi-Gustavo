using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();

builder.Services.AddControllers();


// Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida o emissor
        ValidateIssuer = true,

        // Valida o destinatário
        ValidateAudience = true,

        // Valida a validade do token
        ValidateLifetime = true,

        // Valida a assinatura do token
        ValidateIssuerSigningKey = true,

        // Busca a chave no appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]
            )
        ),

        ClockSkew = TimeSpan.FromMinutes(30),

        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});


builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();

builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();


var app = builder.Build();


app.UseRouting();


// Habilita autenticação
app.UseAuthentication();

// Habilita autorização
app.UseAuthorization();


app.MapControllers();


app.Run();