using System;
using System.Collections.Generic; // Asegúrate de incluir esto para usar List<>
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

class Program
{
    private static CosmosClient cosmosClient;
    private static Database database;
    private static Container container;

    // Nombre de la base de datos y el container
    private const string databaseId = "BudgetDB"; // Base de datos
    private const string containerId = "Budget"; // Container

    static async Task Main(string[] args)
    {
        // Leer configuraciones del archivo
        string endpointUri = ConfigurationManager.AppSettings["EndpointUri"];
        string primaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        // Crear el cliente de Cosmos DB
        cosmosClient = new CosmosClient(endpointUri, primaryKey);

        // Obtener la base de datos y el container
        database = cosmosClient.GetDatabase(databaseId);
        container = database.GetContainer(containerId);

        // ID del presupuesto que deseas actualizar
        string presupuestoId = "presupuesto_123";

        // Nuevo gasto a agregar
        var nuevoGasto = new Gasto
        {
            id_gasto = "gasto_003",
            nombre = "cine",
            monto = 30
        };

        // Actualizar el documento
        await ActualizarPresupuestoAsync(presupuestoId, nuevoGasto);
    }

    private static async Task ActualizarPresupuestoAsync(string presupuestoId, Gasto nuevoGasto)
    {
        try
        {
            // Leer el documento existente como un tipo explícito
            ItemResponse<Presupuesto> response = await container.ReadItemAsync<Presupuesto>(presupuestoId, new PartitionKey(presupuestoId));

            // Obtener el documento
            Presupuesto presupuesto = response.Resource;

            // Agregar el nuevo gasto al array de gastos
            presupuesto.gastos.Add(nuevoGasto);

            // Actualizar el documento en el container
            await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuestoId));
            Console.WriteLine("Presupuesto actualizado correctamente.");
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error al actualizar el presupuesto: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }
    }
}

// Clase Gasto
public class Gasto
{
    public string id_gasto { get; set; }
    public string nombre { get; set; }
    public int monto { get; set; }

    // Constructor público sin parámetros
    public Gasto() { }
}

// Clase Presupuesto
public class Presupuesto
{
    public string id { get; set; }
    public decimal salario { get; set; }
    public decimal css { get; set; }
    public decimal ahorro { get; set; }
    public decimal dinero_restante { get; set; }
    public List<Gasto> gastos { get; set; }

    // Constructor público sin parámetros
    public Presupuesto()
    {
        gastos = new List<Gasto>(); // Inicializar la lista de gastos
    }
}