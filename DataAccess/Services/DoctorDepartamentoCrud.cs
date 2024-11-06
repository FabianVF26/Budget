using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class DoctorDepartamentoCrud
    {
        private readonly SqlDao _sqlDao;

        public DoctorDepartamentoCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public void Create(DoctorDepartamentoDTO doctorDepartamento)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_DOCTOR_DEPARTAMENTO_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", doctorDepartamento.DoctorId);
            sqlOperation.AddIntParam("P_DepartamentoId", doctorDepartamento.DepartamentoId);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public void Update(DoctorDepartamentoDTO doctorDepartamento)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_DOCTOR_DEPARTAMENTO_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", doctorDepartamento.DoctorId);
            sqlOperation.AddIntParam("P_DepartamentoId", doctorDepartamento.DepartamentoId);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public void Delete(int doctorId, int departamentoId)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_DOCTOR_DEPARTAMENTO_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", doctorId);
            sqlOperation.AddIntParam("P_DepartamentoId", departamentoId);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public DoctorDepartamentoDTO Retrieve(int doctorId, int departamentoId)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_DOCTOR_DEPARTAMENTO_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", doctorId);
            sqlOperation.AddIntParam("P_DepartamentoId", departamentoId);
            var result = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (result.Count > 0)
            {
                var row = result[0];
                return BuildDoctorDepartamento(row);
            }

            return null;
        }

        public List<DoctorDepartamentoDTO> RetrieveAll()
        {
            var lstDoctorDepartamentos = new List<DoctorDepartamentoDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_DOCTOR_DEPARTAMENTO_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var doctorDepartamento = BuildDoctorDepartamento(row);
                lstDoctorDepartamentos.Add(doctorDepartamento);
            }

            return lstDoctorDepartamentos;
        }

        private DoctorDepartamentoDTO BuildDoctorDepartamento(Dictionary<string, object> row)
        {
            return new DoctorDepartamentoDTO
            {
                DoctorId = (int)row["doctor_id"],
                DepartamentoId = (int)row["departamento_id"]
            };
        }
    }
}