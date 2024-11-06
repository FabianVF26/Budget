using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class PacienteCrud : CrudFactory<PacienteDTO>
    {
        public PacienteCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(PacienteDTO paciente)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_PACIENTE_PR"
            };

            sqlOperation.AddStringParam("P_Nombre", paciente.Nombre);
            sqlOperation.AddStringParam("P_Apellido", paciente.Apellido);
            sqlOperation.AddDateTimeParam("P_FechaNacimiento", paciente.FechaNacimiento ?? DateTime.MinValue);
            sqlOperation.AddStringParam("P_Genero", paciente.Genero.ToString()); 
            sqlOperation.AddStringParam("P_Telefono", paciente.Telefono);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(PacienteDTO paciente)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_PACIENTE_PR"
            };

            sqlOperation.AddIntParam("P_PacienteId", paciente.PacienteId);
            sqlOperation.AddStringParam("P_Nombre", paciente.Nombre);
            sqlOperation.AddStringParam("P_Apellido", paciente.Apellido);
            sqlOperation.AddDateTimeParam("P_FechaNacimiento", paciente.FechaNacimiento ?? DateTime.MinValue);
            sqlOperation.AddStringParam("P_Telefono", paciente.Telefono);
            sqlOperation.AddStringParam("P_Genero", paciente.Genero.ToString()); // Corregido

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_PACIENTE_PR"
            };

            sqlOperation.AddIntParam("P_PacienteId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override PacienteDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_PACIENTE_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_PacienteId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildPaciente(row);
            }

            return null;
        }

        public override List<PacienteDTO> RetrieveAll()
        {
            var lstPacientes = new List<PacienteDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_PACIENTES_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var paciente = BuildPaciente(row);
                lstPacientes.Add(paciente);
            }

            return lstPacientes;
        }

        private PacienteDTO BuildPaciente(Dictionary<string, object> row)
{
    return new PacienteDTO
    {
        PacienteId = row.ContainsKey("paciente_id") ? (int)row["paciente_id"] : 0, 
        Nombre = row["nombre"].ToString(),
        Apellido = row["apellido"].ToString(),
        FechaNacimiento = row.ContainsKey("fecha_nacimiento") ? (DateTime?)row["fecha_nacimiento"] : null, 
        Genero = row.ContainsKey("género") ? row["género"].ToString()[0] : 'M', 
        Telefono = row.ContainsKey("teléfono") ? row["teléfono"].ToString() : string.Empty 
    };
}
    }
}