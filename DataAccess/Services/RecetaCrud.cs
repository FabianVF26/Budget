using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class RecetaCrud : CrudFactory<RecetaDTO>
    {
        public RecetaCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(RecetaDTO receta)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_RECETA_PR"
            };

            sqlOperation.AddIntParam("P_HistoriaId", receta.HistoriaId);
            sqlOperation.AddStringParam("P_Medicamento", receta.Medicamento);
            sqlOperation.AddStringParam("P_Dosis", receta.Dosis);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(RecetaDTO receta)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_RECETA_PR"
            };

            sqlOperation.AddIntParam("P_RecetaId", receta.RecetaId);
            sqlOperation.AddIntParam("P_HistoriaId", receta.HistoriaId);
            sqlOperation.AddStringParam("P_Medicamento", receta.Medicamento);
            sqlOperation.AddStringParam("P_Dosis", receta.Dosis);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_RECETA_PR"
            };

            sqlOperation.AddIntParam("P_RecetaId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override RecetaDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_RECETA_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_RecetaId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildReceta(row);
            }

            return null;
        }

        public override List<RecetaDTO> RetrieveAll()
        {
            var lstRecetas = new List<RecetaDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_RECETAS_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var receta = BuildReceta(row);
                lstRecetas.Add(receta);
            }

            return lstRecetas;
        }

        private RecetaDTO BuildReceta(Dictionary<string, object> row)
        {
            return new RecetaDTO
            {
                RecetaId = row.ContainsKey("RecetaId") ? (int)row["RecetaId"] : 0,
                HistoriaId = row.ContainsKey("HistoriaId") ? (int)row["HistoriaId"] : 0,
                Medicamento = row.ContainsKey("Medicamento") ? row["Medicamento"].ToString() : string.Empty,
                Dosis = row.ContainsKey("Dosis") ? row["Dosis"].ToString() : string.Empty
            };
        }
    }
}