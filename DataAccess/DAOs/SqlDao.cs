using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataAccess.DAOs
{
    public class SqlDao
    {
        private static SqlDao _instance;
        private string _connectionString;

        private SqlDao()
        {
            _connectionString =
                "Server=localhost;Database=Clinica;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public static SqlDao GetInstance()
        {
            if (_instance == null)
                _instance = new SqlDao();
            return _instance;
        }

        public string ConnectionString => _connectionString;

        public void UpdateConnectionString(string username, string password)
        {
            _connectionString = $"Server=localhost;Database=Clinica;User Id={username};Password={password};TrustServerCertificate=True;";
        }

        public void ExecuteProcedure(SqlOperation sqlOperation)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    foreach (var param in sqlOperation.Parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Dictionary<string, object>> ExecuteQueryProcedure(SqlOperation sqlOperation)
        {
            var listResults = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    foreach (var param in sqlOperation.Parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var rowDictionary = new Dictionary<string, object>();
                                for (var index = 0; index < reader.FieldCount; ++index)
                                {
                                    var key = reader.GetName(index);
                                    var value = reader.GetValue(index);
                                    rowDictionary[key] = value;
                                }

                                listResults.Add(rowDictionary);
                            }
                        }
                    }
                }
            }

            return listResults;
        }
        public void CreateDatabaseUser(string newUsername, string newPassword)
        {
            string connectionStringMaster = _connectionString.Replace("Clinica", "master");

            string sqlCreateLogin = $@"
    CREATE LOGIN [{newUsername}] WITH PASSWORD = '{newPassword}';
";

            try
            {
                using (var conn = new SqlConnection(connectionStringMaster))
                {
                    conn.Open();

                    using (var command = new SqlCommand(sqlCreateLogin, conn))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Login creado exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el login: {ex.Message}");
                return;
            }

            string connectionStringClinica = _connectionString;

            string sqlCreateUser = $@"
                IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{newUsername}')
                BEGIN
                    CREATE USER [{newUsername}] FOR LOGIN [{newUsername}];
                    PRINT 'Usuario creado exitosamente.';
                END
                ELSE
                BEGIN
                    PRINT 'El usuario ya existe.';
                END
            ";

            try
            {
                using (var conn = new SqlConnection(connectionStringClinica))
                {
                    conn.Open();

                    using (var command = new SqlCommand(sqlCreateUser, conn))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Usuario creado exitosamente en la base de datos 'Clinica'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el usuario: {ex.Message}");
                return;
            }

            string sqlAssignRole = $@"
                ALTER ROLE rol_aplicacion ADD MEMBER [{newUsername}];
                ";

            try
            {
                using (var conn = new SqlConnection(connectionStringClinica))
                {
                    conn.Open();

                    using (var command = new SqlCommand(sqlAssignRole, conn))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Rol asignado exitosamente al usuario.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al asignar el rol: {ex.Message}");
            }
        }
    }
}