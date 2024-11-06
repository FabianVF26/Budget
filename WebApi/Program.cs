using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuraci�n desde un archivo JSON
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configuraci�n del cliente de Cosmos DB
builder.Services.AddScoped<CosmosClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string endpointUri = configuration["EndpointUri"];
    string primaryKey = configuration["PrimaryKey"];

    // Imprimir valores en la consola para verificar
    Console.WriteLine($"EndpointUri: {endpointUri}");
    Console.WriteLine($"PrimaryKey: {primaryKey}");

    // Comprobar si son nulos o vac�os
    if (string.IsNullOrEmpty(endpointUri))
    {
        throw new ArgumentNullException(nameof(endpointUri), "EndpointUri cannot be null or empty.");
    }

    if (string.IsNullOrEmpty(primaryKey))
    {
        throw new ArgumentNullException(nameof(primaryKey), "PrimaryKey cannot be null or empty.");
    }

    return new CosmosClient(endpointUri, primaryKey);
});

// Registrar el servicio de presupuesto
builder.Services.AddScoped<PresupuestoService>();

// A�adir controladores
builder.Services.AddControllers();

// A�adir Swagger para la documentaci�n de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n del pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();