using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class PagoCrud : CrudFactory<PagoDTO>
    {
        public PagoCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(PagoDTO pago)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_PAGO_PR"
            };

            sqlOperation.AddIntParam("P_CitaId", pago.CitaId);
            sqlOperation.AddDecimalParam("P_Monto", pago.Monto);
            sqlOperation.AddDateTimeParam("P_Fecha", pago.Fecha);
            sqlOperation.AddStringParam("P_Metodo", pago.Metodo);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(PagoDTO pago)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_PAGO_PR"
            };

            sqlOperation.AddIntParam("P_PagoId", pago.PagoId);
            sqlOperation.AddIntParam("P_CitaId", pago.CitaId);
            sqlOperation.AddDecimalParam("P_Monto", pago.Monto);
            sqlOperation.AddDateTimeParam("P_Fecha", pago.Fecha);
            sqlOperation.AddStringParam("P_Metodo", pago.Metodo);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_PAGO_PR"
            };

            sqlOperation.AddIntParam("P_PagoId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override PagoDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_PAGO_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_PagoId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildPago(row);
            }

            return null;
        }

        public override List<PagoDTO> RetrieveAll()
        {
            var lstPagos = new List<PagoDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_PAGOS_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var pago = BuildPago(row);
                lstPagos.Add(pago);
            }

            return lstPagos;
        }

        private PagoDTO BuildPago(Dictionary<string, object> row)
        {
            return new PagoDTO
            {
                PagoId = row.ContainsKey("PagoId") ? (int)row["PagoId"] : 0,
                CitaId = row.ContainsKey("CitaId") ? (int)row["CitaId"] : 0,
                Monto = row.ContainsKey("Monto") ? (decimal)row["Monto"] : 0,
                Fecha = row.ContainsKey("Fecha") ? (DateTime)row["Fecha"] : DateTime.MinValue,
                Metodo = row.ContainsKey("Metodo") ? row["Metodo"].ToString() : string.Empty
            };
        }
    }
}