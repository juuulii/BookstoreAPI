using Application.Services;
using Domain.Interfaces;
using Infraestructure; //necesito esto para la inyeccion de dependencia (base de datos a nuestra API)
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(
builder.Configuration["ConnectionStrings:DBConnectionString"], b => b.MigrationsAssembly("BookstoreAPI")));

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac? pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definici?n
                }, new List<string>()
            }
    });
});

builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticaci?n que tenemos que elegir despu?s en PostMan para pasarle el token
    .AddJwtBearer(options => //Ac? definimos la configuraci?n de la autenticaci?n. le decimos qu? cosas queremos comprobar. La fecha de expiraci?n se valida por defecto.
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
);


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<PublisherService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();




var app = builder.Build();

// 🔹 Middleware global de errores
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            var response = new
            {
                message = "Ocurrió un error inesperado en el servidor.",
                detail = error.Error.Message // ⚠️ en producción podés quitar esto para no mostrar detalles internos
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// 🔹 Manejo global de códigos de estado (401, 403, 404, etc.)
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    response.ContentType = "application/json";

    // Respuestas personalizadas
    switch (response.StatusCode)
    {
        case 401:
            await response.WriteAsJsonAsync(new { message = "No autorizado. Debes iniciar sesión." });
            break;
        case 403:
            await response.WriteAsJsonAsync(new { message = "Acceso denegado. No tienes permisos suficientes." });
            break;
        case 404:
            await response.WriteAsJsonAsync(new { message = "Recurso no encontrado." });
            break;
        default:
            await response.WriteAsJsonAsync(new { message = $"Error {response.StatusCode}" });
            break;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();