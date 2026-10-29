using System.Text;
using System.Text.Json.Serialization;
using AQV.WebAPI.BdContext;
using AQV_API.Interfaces;
using AQV_API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Banco de dados — contexto gerado pelo scaffold em BdContext/AqvContext.cs
// ---------------------------------------------------------------------------
builder.Services.AddDbContext<AqvContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()
    )
);

// ---------------------------------------------------------------------------
// Injeção de dependência dos repositories
// ---------------------------------------------------------------------------
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<IResponsavelRepository, ResponsavelRepository>();
builder.Services.AddScoped<IOstatusRepository, OstatusRepository>();
builder.Services.AddScoped<IOcorrenciaRepository, OcorrenciaRepository>();
builder.Services.AddScoped<IEncaminhamentoRepository, EncaminhamentoRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

// ---------------------------------------------------------------------------
// Controllers
// ---------------------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita erro de ciclo de referência nas navegações das entidades
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ---------------------------------------------------------------------------
// CORS — libera o front em React/Vite (http://localhost:5173)
// ---------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AqvPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------------------------------------------------------------------------
// Autenticação JWT Bearer
// ---------------------------------------------------------------------------
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("A chave Jwt:Key não foi configurada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromMinutes(5),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        NameClaimType = System.Security.Claims.ClaimTypes.Name
    };
});

builder.Services.AddAuthorization();

// ---------------------------------------------------------------------------
// Swagger
// ---------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API AQV-OS",
        Description = "API para gestão de tratativas e ocorrências de alunos"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT:"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = Array.Empty<string>().ToList()
    });
});

var app = builder.Build();

// ---------------------------------------------------------------------------
// Pipeline
// ---------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AQV-OS v1");
        // Swagger na raiz: http://localhost:5214/
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AqvPolicy");

// A autenticação precisa vir antes da autorização.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
