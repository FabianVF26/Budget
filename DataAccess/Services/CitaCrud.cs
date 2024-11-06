using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class CitaCrud : CrudFactory<CitaDTO>
    {
        public CitaCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(CitaDTO cita)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_CITA_PR"
            };

            sqlOperation.AddIntParam("P_PacienteId", cita.PacienteId);
            sqlOperation.AddIntParam("P_DoctorId", cita.DoctorId);
            sqlOperation.AddDateTimeParam("P_Fecha", cita.Fecha);
            sqlOperation.AddTimeParam("P_Hora", cita.Hora.HasValue ? TimeOnly.FromTimeSpan(cita.Hora.Value) : default);
            sqlOperation.AddStringParam("P_Estado", cita.Estado);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(CitaDTO cita)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_CITA_PR"
            };

            sqlOperation.AddIntParam("P_CitaId", cita.CitaId);
            sqlOperation.AddIntParam("P_PacienteId", cita.PacienteId);
            sqlOperation.AddIntParam("P_DoctorId", cita.DoctorId);
            sqlOperation.AddDateTimeParam("P_Fecha", cita.Fecha);
            sqlOperation.AddTimeParam("P_Hora", cita.Hora.HasValue ? TimeOnly.FromTimeSpan(cita.Hora.Value) : default);
            sqlOperation.AddStringParam("P_Estado", cita.Estado);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_CITA_PR"
            };

            sqlOperation.AddIntParam("P_CitaId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override CitaDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_CITA_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_CitaId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildCita(row);
            }

            return null;
        }

        public override List<CitaDTO> RetrieveAll()
        {
            var lstCitas = new List<CitaDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_CITAS_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var cita = BuildCita(row);
                lstCitas.Add(cita);
            }

            return lstCitas;
        }

        private CitaDTO BuildCita(Dictionary<string, object> row)
        {
            return new CitaDTO
            {
                CitaId = row.ContainsKey("CitaId") ? (int)row["CitaId"] : 0,
                PacienteId = row.ContainsKey("PacienteId") ? (int)row["PacienteId"] : 0, 
                DoctorId = row.ContainsKey("DoctorId") ? (int)row["DoctorId"] : 0, 
                Fecha = row.ContainsKey("Fecha") ? (DateTime)row["Fecha"] : DateTime.MinValue, 
                Hora = row.ContainsKey("Hora") ? (TimeSpan?)row["Hora"] : null, 
                Estado = row.ContainsKey("Estado") ? row["Estado"].ToString() : string.Empty 
            };
        }
    }
}