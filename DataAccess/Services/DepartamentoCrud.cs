using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class DepartamentoCrud : CrudFactory<DepartamentoDTO>
    {
        public DepartamentoCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(DepartamentoDTO departamento)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_DEPARTAMENTO_PR"
            };

            sqlOperation.AddStringParam("P_Nombre", departamento.Nombre);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(DepartamentoDTO departamento)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_DEPARTAMENTO_PR"
            };

            sqlOperation.AddIntParam("P_DepartamentoId", departamento.DepartamentoId);
            sqlOperation.AddStringParam("P_Nombre", departamento.Nombre);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_DEPARTAMENTO_PR"
            };

            sqlOperation.AddIntParam("P_DepartamentoId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override DepartamentoDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_DEPARTAMENTO_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_DepartamentoId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildDepartamento(row);
            }

            return null;
        }

        public override List<DepartamentoDTO> RetrieveAll()
        {
            var lstDepartamentos = new List<DepartamentoDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_DEPARTAMENTOS_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var departamento = BuildDepartamento(row);
                lstDepartamentos.Add(departamento);
            }

            return lstDepartamentos;
        }

        private DepartamentoDTO BuildDepartamento(Dictionary<string, object> row)
        {
            return new DepartamentoDTO
            {
                DepartamentoId = (int)row["DepartamentoId"],
                Nombre = row["Nombre"].ToString()
            };
        }
    }
}