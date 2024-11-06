using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class PruebaLaboratorioCrud
    {
        private readonly SqlDao _sqlDao;

        public PruebaLaboratorioCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public void Create(PruebaLaboratorioDTO prueba)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_PRUEBA_LABORATORIO_PR"
            };

            sqlOperation.AddIntParam("P_CitaId", prueba.CitaId);
            sqlOperation.AddStringParam("P_Nombre", prueba.Nombre);
            sqlOperation.AddStringParam("P_Resultado", prueba.Resultado);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public void Update(PruebaLaboratorioDTO prueba)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_PRUEBA_LABORATORIO_PR"
            };

            sqlOperation.AddIntParam("P_PruebaId", prueba.PruebaId);
            sqlOperation.AddIntParam("P_CitaId", prueba.CitaId);
            sqlOperation.AddStringParam("P_Nombre", prueba.Nombre);
            sqlOperation.AddStringParam("P_Resultado", prueba.Resultado);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public void Delete(int pruebaId)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_PRUEBA_LABORATORIO_PR"
            };

            sqlOperation.AddIntParam("P_PruebaId", pruebaId);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public PruebaLaboratorioDTO Retrieve(int pruebaId)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_PRUEBA_LABORATORIO_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_PruebaId", pruebaId);
            var result = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (result.Count > 0)
            {
                var row = result[0];
                return BuildPruebaLaboratorio(row);
            }

            return null;
        }

        public List<PruebaLaboratorioDTO> RetrieveAll()
        {
            var lstPruebas = new List<PruebaLaboratorioDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_PRUEBAS_LABORATORIO_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var prueba = BuildPruebaLaboratorio(row);
                lstPruebas.Add(prueba);
            }

            return lstPruebas;
        }

        private PruebaLaboratorioDTO BuildPruebaLaboratorio(Dictionary<string, object> row)
        {
            return new PruebaLaboratorioDTO
            {
                PruebaId = (int)row["prueba_id"],
                CitaId = (int)row["cita_id"],
                Nombre = (string)row["nombre"],
                Resultado = (string)row["resultado"]
            };
        }
    }
}