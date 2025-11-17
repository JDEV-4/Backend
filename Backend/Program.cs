using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Backend.DataAccess;
using Backend.Models;
using Backend.Services;
using Backend.Services.MetricsServices;
using Backend.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios principales
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
    });

builder.Services.AddEndpointsApiExplorer();

//Swagger con autenticación JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con 'Bearer'. Ejemplo: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar JWT desde appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Cadena de conexión SQL
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  Inyección de DAL y Services
builder.Services.AddScoped<UsuarioDAL>(sp => new UsuarioDAL(connectionString));
builder.Services.AddScoped<UsuarioServices>();

builder.Services.AddScoped<ProductoDAL>(sp => new ProductoDAL(connectionString));
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<CompraDAL>(sp => new CompraDAL(connectionString));
builder.Services.AddScoped<CompraService>();

builder.Services.AddScoped<CategoriaDAL>(sp => new CategoriaDAL(connectionString));
builder.Services.AddScoped<CategoriaService>();

//builder.Services.AddScoped<IVentaService, VentaService>();

// Inyección de métricas (MongoDB)
builder.Services.AddSingleton<IMetrics, MetricService>();

// Configurar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins(
            "http://192.168.1.76",
            "http://localhost",
            "http://localhost:5138"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Construcción de la aplicación
var app = builder.Build();

//Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("PermitirFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Registrar métricas DESPUÉS de autenticación
app.UseMiddleware<RequestMetricsMiddleware>();

app.MapControllers();


// Mapear controladores
app.MapControllers();

// Ejecutar la app
app.Run("http://0.0.0.0:5138");
