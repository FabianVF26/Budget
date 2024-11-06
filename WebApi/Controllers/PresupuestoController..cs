using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DTOs;

[Route("api/[controller]")]
[ApiController]
public class PresupuestosController : ControllerBase
{
    private readonly PresupuestoService _presupuestoService;

    public PresupuestosController(PresupuestoService presupuestoService)
    {
        _presupuestoService = presupuestoService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPresupuesto(string id)
    {
        var presupuesto = await _presupuestoService.GetPresupuestoAsync(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return Ok(presupuesto);
    }

    // Nuevo método para obtener los gastos de un presupuesto específico
    [HttpGet("{presupuestoId}/gastos")]
    public async Task<IActionResult> GetGastos(string presupuestoId)
    {
        try
        {
            var gastos = await _presupuestoService.GetGastosAsync(presupuestoId);
            return Ok(gastos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{presupuestoId}/gastos")]
    public async Task<IActionResult> AddGasto(string presupuestoId, [FromBody] GastoDto nuevoGasto)
    {
        if (nuevoGasto == null)
        {
            return BadRequest("El nuevo gasto no puede ser nulo.");
        }

        // Puedes agregar aquí validaciones adicionales si es necesario

        await _presupuestoService.AddGastoAsync(presupuestoId, nuevoGasto);
        return CreatedAtAction(nameof(GetPresupuesto), new { id = presupuestoId }, nuevoGasto);
    }

    [HttpPut("{presupuestoId}/gastos")]
    public async Task<IActionResult> EditGasto(string presupuestoId, [FromBody] GastoDto gastoEditado)
    {
        try
        {
            await _presupuestoService.EditGastoAsync(presupuestoId, gastoEditado);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{presupuestoId}/gastos/{idGasto}")]
    public async Task<IActionResult> DeleteGasto(string presupuestoId, string idGasto)
    {
        try
        {
            await _presupuestoService.DeleteGastoAsync(presupuestoId, idGasto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePresupuesto([FromBody] PresupuestoDto nuevoPresupuesto)
    {
        if (nuevoPresupuesto == null)
        {
            return BadRequest("El nuevo presupuesto no puede ser nulo.");
        }

        try
        {
            var presupuestoCreado = await _presupuestoService.CreatePresupuestoAsync(nuevoPresupuesto);
            return CreatedAtAction(nameof(GetPresupuesto), new { id = presupuestoCreado.id }, presupuestoCreado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}