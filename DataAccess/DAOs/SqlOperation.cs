using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataAccess.DAOs
{
    public class SqlOperation
    {
        public SqlOperation()
        {
            Parameters = new List<SqlParameter>();
        }

        public string ProcedureName { get; set; }
        public List<SqlParameter> Parameters { get; set; }

        public void AddStringParam(string paramName, string paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddIntParam(string paramName, int? paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDateTimeParam(string paramName, DateTime paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDoubleParam(string paramName, double paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddTimeParam(string paramName, TimeOnly paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddCharParam(string paramName, char paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDecimalParam(string paramName, decimal paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }
    }
}