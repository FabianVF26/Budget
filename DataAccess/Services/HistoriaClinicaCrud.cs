using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class HistoriaClinicaCrud : CrudFactory<HistoriaClinicaDTO>
    {
        public HistoriaClinicaCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(HistoriaClinicaDTO historia)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_HISTORIA_CLINICA_PR"
            };

            sqlOperation.AddIntParam("P_PacienteId", historia.PacienteId);
            sqlOperation.AddIntParam("P_DoctorId", historia.DoctorId);
            sqlOperation.AddStringParam("P_Diagnostico", historia.Diagnostico);
            sqlOperation.AddStringParam("P_Tratamiento", historia.Tratamiento);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(HistoriaClinicaDTO historia)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_HISTORIA_CLINICA_PR"
            };

            sqlOperation.AddIntParam("P_HistoriaId", historia.HistoriaId);
            sqlOperation.AddIntParam("P_PacienteId", historia.PacienteId);
            sqlOperation.AddIntParam("P_DoctorId", historia.DoctorId);
            sqlOperation.AddStringParam("P_Diagnostico", historia.Diagnostico);
            sqlOperation.AddStringParam("P_Tratamiento", historia.Tratamiento);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_HISTORIA_CLINICA_PR"
            };

            sqlOperation.AddIntParam("P_HistoriaId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override HistoriaClinicaDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_HISTORIA_CLINICA_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_HistoriaId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildHistoria(row);
            }

            return null;
        }

        public override List<HistoriaClinicaDTO> RetrieveAll()
        {
            var lstHistorias = new List<HistoriaClinicaDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_HISTORIAS_CLINICAS_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var historia = BuildHistoria(row);
                lstHistorias.Add(historia);
            }

            return lstHistorias;
        }

        private HistoriaClinicaDTO BuildHistoria(Dictionary<string, object> row)
        {
            return new HistoriaClinicaDTO
            {
                HistoriaId = row.ContainsKey("HistoriaId") ? (int)row["HistoriaId"] : 0,
                PacienteId = row.ContainsKey("PacienteId") ? (int)row["PacienteId"] : 0,
                DoctorId = row.ContainsKey("DoctorId") ? (int)row["DoctorId"] : 0,
                Diagnostico = row.ContainsKey("Diagnostico") ? row["Diagnostico"].ToString() : string.Empty,
                Tratamiento = row.ContainsKey("Tratamiento") ? row["Tratamiento"].ToString() : string.Empty
            };
        }
    }
}