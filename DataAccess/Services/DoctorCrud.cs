using DataAccess.DAOs;
using DTOs;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class DoctorCrud : CrudFactory<DoctorDTO>
    {
        public DoctorCrud()
        {
            _sqlDao = SqlDao.GetInstance();
        }

        public override void Create(DoctorDTO doctor)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "CRE_DOCTOR_PR"
            };

            sqlOperation.AddStringParam("P_Nombre", doctor.Nombre);
            sqlOperation.AddStringParam("P_Apellido", doctor.Apellido);
            sqlOperation.AddStringParam("P_Especializacion", doctor.Especializacion);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Update(DoctorDTO doctor)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "UPD_DOCTOR_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", doctor.DoctorId);
            sqlOperation.AddStringParam("P_Nombre", doctor.Nombre);
            sqlOperation.AddStringParam("P_Apellido", doctor.Apellido);
            sqlOperation.AddStringParam("P_Especializacion", doctor.Especializacion);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "DEL_DOCTOR_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override DoctorDTO Retrieve(int id)
        {
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_DOCTOR_BY_ID_PR"
            };

            sqlOperation.AddIntParam("P_DoctorId", id);
            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResults.Count > 0)
            {
                var row = lstResults[0];
                return BuildDoctor(row);
            }

            return null;
        }

        public override List<DoctorDTO> RetrieveAll()
        {
            var lstDoctores = new List<DoctorDTO>();
            var sqlOperation = new SqlOperation
            {
                ProcedureName = "RET_ALL_DOCTORES_PR"
            };

            var lstResults = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            foreach (var row in lstResults)
            {
                var doctor = BuildDoctor(row);
                lstDoctores.Add(doctor);
            }

            return lstDoctores;
        }

        private DoctorDTO BuildDoctor(Dictionary<string, object> row)
        {
            return new DoctorDTO
            {
                DoctorId = (int)row["doctor_id"],
                Nombre = row["nombre"].ToString(),
                Apellido = row["apellido"].ToString(),
                Especializacion = row["especialización"].ToString()
            };
        }
    }
}