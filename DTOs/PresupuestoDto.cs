using DTOs;

public class PresupuestoDto
{
    public string id { get; set; } // id del presupuesto
    public decimal Salario { get; set; }
    public decimal Css { get; set; }
    public decimal Ahorro { get; set; }
    public decimal DineroRestante { get; set; }
    public List<GastoDto> Gastos { get; set; } = new List<GastoDto>();
}