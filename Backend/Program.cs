using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Backend.DataAccess;
using Backend.Models;
using Backend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configuración de servicios principales
// ==========================================
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // JSON indentado y legible
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // Fechas legibles
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
    });

builder.Services.AddEndpointsApiExplorer();

// ==========================================
// 2. Swagger con autenticación JWT
// ==========================================
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

// ==========================================
// 3. Configurar JWT desde appsettings.json
// ==========================================
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// ==========================================
// 4. Obtener cadena de conexión
// ==========================================
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ==========================================
// 5. Registrar DAL y Services usando Scoped
// ==========================================
// Usuarios
builder.Services.AddScoped<UsuarioDAL>(sp => new UsuarioDAL(connectionString));
builder.Services.AddScoped<UsuarioServices>();

// Productos
builder.Services.AddScoped<ProductoDAL>(sp => new ProductoDAL(connectionString));
builder.Services.AddScoped<ProductoService>();

// Compra
builder.Services.AddScoped<CompraDAL>(sp => new CompraDAL(connectionString));
builder.Services.AddScoped<CompraService>();


// Categorías
builder.Services.AddScoped<CategoriaDAL>(sp => new CategoriaDAL(connectionString));
builder.Services.AddScoped<CategoriaService>();






// ==========================================
// 6. Configurar autenticación JWT
// ==========================================
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

// ==========================================
// 7. Configurar CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins(
            "http://192.168.1.76", // IP del frontend
            "http://localhost"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ==========================================
// 8. Construcción de la aplicación
// ==========================================
var app = builder.Build();

// ==========================================
// 9. Middleware
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar CORS
app.UseCors("PermitirFrontend");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// ==========================================
// 10. Escuchar en todas las interfaces de red
// ==========================================
app.Run("http://0.0.0.0:5138");
