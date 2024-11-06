using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using DTOs;
using System.Text.Json;

public class PresupuestoService
{
    private readonly CosmosClient cosmosClient;
    private readonly Container container;

    private const string databaseId = "BudgetDB";
    private const string containerId = "Budget";
    private const string endpointUri = "https://budget.documents.azure.com:443/";
    private const string primaryKey = "GgKF7BfI7f4MDgqu4ok0vpysdi9eUHL1jzY7GpKutuqdiqHgA3GcCsPUUAgZsYwG4nhpdexNWp7nACDbvQuidw==";

    public PresupuestoService()
    {
        cosmosClient = new CosmosClient(endpointUri, primaryKey);
        container = cosmosClient.GetContainer(databaseId, containerId);
    }

    public async Task<PresupuestoDto> GetPresupuestoAsync(string presupuestoId)
    {
        try
        {
            ItemResponse<PresupuestoDto> response = await container.ReadItemAsync<PresupuestoDto>(presupuestoId, new PartitionKey(presupuestoId));

            Console.WriteLine($"Presupuesto recuperado: {JsonSerializer.Serialize(response.Resource)}");

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<List<GastoDto>> GetGastosAsync(string presupuestoId)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto == null)
        {
            throw new Exception("Presupuesto no encontrado.");
        }

        return presupuesto.Gastos;
    }

    public async Task AddGastoAsync(string presupuestoId, GastoDto nuevoGasto)
    {
        if (string.IsNullOrWhiteSpace(nuevoGasto.IdGasto))
        {
            throw new ArgumentException("El IdGasto no puede ser null o vacío.");
        }

        if (nuevoGasto.Monto <= 0)
        {
            throw new ArgumentException("El Monto debe ser un valor positivo.");
        }

        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto == null)
        {
            throw new ArgumentException("El presupuesto no existe.");
        }

        if (presupuesto.Gastos.Any(g => g.IdGasto == nuevoGasto.IdGasto))
        {
            throw new ArgumentException("El IdGasto ya existe en el presupuesto.");
        }

        // Añadir el nuevo gasto
        presupuesto.Gastos.Add(nuevoGasto);

        // Recalcular el dinero restante
        ActualizarDineroRestante(presupuesto);

        try
        {
            // Guardar los cambios
            await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
            Console.WriteLine("Gasto agregado y dinero restante recalculado exitosamente.");
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error al agregar el gasto: {ex.Message}");
            throw;
        }
    }

    public async Task EditSalarioAsync(string presupuestoId, decimal nuevoSalario)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto != null)
        {
            presupuesto.Salario = nuevoSalario;
            ActualizarDineroRestante(presupuesto);

            await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
        }
        else
        {
            throw new Exception("Presupuesto no encontrado.");
        }
    }

    public async Task EditCssAsync(string presupuestoId, decimal nuevoCss)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto != null)
        {
            presupuesto.Css = nuevoCss;
            ActualizarDineroRestante(presupuesto);

            await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
        }
        else
        {
            throw new Exception("Presupuesto no encontrado.");
        }
    }

    public async Task EditAhorroAsync(string presupuestoId, decimal nuevoAhorro)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto != null)
        {
            presupuesto.Ahorro = nuevoAhorro;
            ActualizarDineroRestante(presupuesto);

            await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
        }
        else
        {
            throw new Exception("Presupuesto no encontrado.");
        }
    }

    // Método para actualizar DineroRestante
    private void ActualizarDineroRestante(PresupuestoDto presupuesto)
    {
        presupuesto.DineroRestante = presupuesto.Salario - (presupuesto.Css + presupuesto.Ahorro + presupuesto.Gastos.Sum(g => g.Monto));
    }

    // Editar gasto con recalculación de DineroRestante
    public async Task EditGastoAsync(string presupuestoId, GastoDto gastoEditado)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto != null)
        {
            var gastoExistente = presupuesto.Gastos.FirstOrDefault(g => g.IdGasto == gastoEditado.IdGasto);
            if (gastoExistente != null)
            {
                gastoExistente.Nombre = gastoEditado.Nombre;
                gastoExistente.Monto = gastoEditado.Monto;
                gastoExistente.Quincena = gastoEditado.Quincena;

                ActualizarDineroRestante(presupuesto);

                await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
            }
            else
            {
                throw new Exception("Gasto no encontrado.");
            }
        }
        else
        {
            throw new Exception("Presupuesto no encontrado.");
        }
    }

    // Eliminar gasto con recalculación de DineroRestante
    public async Task DeleteGastoAsync(string presupuestoId, string idGasto)
    {
        var presupuesto = await GetPresupuestoAsync(presupuestoId);
        if (presupuesto != null)
        {
            var gastoExistente = presupuesto.Gastos.FirstOrDefault(g => g.IdGasto == idGasto);
            if (gastoExistente != null)
            {
                presupuesto.Gastos.Remove(gastoExistente);

                ActualizarDineroRestante(presupuesto);

                await container.UpsertItemAsync(presupuesto, new PartitionKey(presupuesto.id));
            }
            else
            {
                throw new Exception("Gasto no encontrado.");
            }
        }
        else
        {
            throw new Exception("Presupuesto no encontrado.");
        }
    }

    public async Task<PresupuestoDto> CreatePresupuestoAsync(PresupuestoDto nuevoPresupuesto)
    {
        if (string.IsNullOrWhiteSpace(nuevoPresupuesto.id))
        {
            throw new ArgumentException("El Id del presupuesto no puede ser null o vacío.");
        }

        // Asegúrate de que la lista de gastos esté inicializada
        nuevoPresupuesto.Gastos = new List<GastoDto>();

        // Recalcular el dinero restante inicialmente
        ActualizarDineroRestante(nuevoPresupuesto);

        try
        {
            await container.CreateItemAsync(nuevoPresupuesto, new PartitionKey(nuevoPresupuesto.id));
            Console.WriteLine("Presupuesto creado exitosamente.");
            return nuevoPresupuesto;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error al crear el presupuesto: {ex.Message}");
            throw;
        }
    }
}