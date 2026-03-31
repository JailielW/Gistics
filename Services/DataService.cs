using Gistics.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.Data;
using System.Security.Cryptography;

namespace Gistics.Services
{
    public class DataService : IDataService
    {
        private readonly string _connectionString;

        public DataService(IConfiguration configuration)
        {
            // Expects a connection string named "DefaultConnection" in configuration.
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            // Example stored procedure name. Replace with your actual SP name.
            const string storedProcedureName = "SP_GetAllEmployees";

            var results = new List<Employees>();

            if (string.IsNullOrWhiteSpace(_connectionString))
                return results;

            // Use Task.Run because SqlDataAdapter.Fill is synchronous. For a high-throughput app
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(storedProcedureName, conn)
            {

                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            //Initialize parameters with DBNull.Value to avoid "Must declare the scalar variable" errors if SP expects them.
            //var titleIdParam = new SqlParameter("@TitleID", SqlDbType.Int) { Value = DBNull.Value };
            //cmd.Parameters.Add(titleIdParam);

            await conn.OpenAsync();

            // Execute reader asynchronously and stream rows
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await reader.ReadAsync())
            {
                var emp = new Employees
                {
                    // Ids / primitives
                    EmpId = (int)reader["EmpID"],
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    BadgeNumber = reader["BadgeNumber"].ToString() ?? string.Empty,

                    Title = new EmployeeTitles { Name = reader["Title"].ToString() ?? string.Empty },
                    BirthDate = DateOnly.FromDateTime((DateTime)reader["BirthDate"]),
                    StartDate = DateOnly.FromDateTime((DateTime)reader["StartDate"])
                };

                emp.BirthDate = DateOnly.FromDateTime((DateTime)reader["BirthDate"]);

                emp.StartDate = DateOnly.FromDateTime((DateTime)reader["StartDate"]);

                //This Value will determine if the field, correlated to this attribute, will appear in the UI
                if (reader["EndDate"] != DBNull.Value)
                {
                    emp.EndDate = DateOnly.FromDateTime((DateTime)reader["EndDate"]);
                }
                else
                {
                    emp.EndDate = DateOnly.MinValue;
                }

                results.Add(emp);
            }

            return results;
        }

        public async Task<Employees> GetEmployeeAsync(int id)
        {
            // Example stored procedure name. Replace with your actual SP name.
            const string storedProcedureName = "SP_GetEmployee";

            var emp = new Employees
            {
                EmpId = 0, 
                FirstName = string.Empty,
                LastName = string.Empty,
                BadgeNumber = string.Empty,

                Title = new EmployeeTitles { Name = string.Empty },
                BirthDate = DateOnly.MinValue,
                StartDate = DateOnly.MinValue,
                EndDate = DateOnly.MinValue
            };

            if (string.IsNullOrWhiteSpace(_connectionString))
                return emp;

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            //Initialize parameters with DBNull.Value to avoid "Must declare the scalar variable" errors if SP expects them.
            var titleIdParam = new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = id };
            cmd.Parameters.Add(titleIdParam);

            await conn.OpenAsync();

            // Execute reader asynchronously and stream rows
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await reader.ReadAsync())
            {
                emp = new Employees
                {
                    // Ids / primitives
                    EmpId = id,
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    BadgeNumber = reader["BadgeNumber"].ToString() ?? string.Empty,

                    Title = new EmployeeTitles { Name = reader["Title"].ToString() ?? string.Empty },
                    BirthDate = reader["BirthDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["BirthDate"]): DateOnly.MinValue,
                    StartDate = reader["StartDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["StartDate"]) : DateOnly.MinValue,
                    EndDate = reader["EndDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["EndDate"]) : DateOnly.MinValue
                };
            }

            return emp;
        }

        public async Task<bool> Insert_EmployeeAsync(string firstname, string lastname, string badgeNum, int titleid,
            DateOnly birthdate)
        {
            // Example stored procedure name. Replace with your actual SP name.
            const string storedProcedureName = "SP_InsertEmployee";

            if (string.IsNullOrWhiteSpace(_connectionString))
                return false;

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            //Initialize parameters with DBNull.Value to avoid "Must declare the scalar variable" errors if SP expects them.
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@BadgeNumber", badgeNum);
            cmd.Parameters.AddWithValue("@TitleID", titleid);
            cmd.Parameters.AddWithValue("@BirthDate", birthdate);

            await conn.OpenAsync();

            // Execute reader asynchronously and stream rows
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAsync(int eID,string firstname, string lastname, string badgeNum, int titleid,
            DateOnly birthdate, DateOnly startdate, DateOnly enddate)
        {
            // Example stored procedure name. Replace with your actual SP name.
            const string storedProcedureName = "SP_UpdateEmployee";

            if (string.IsNullOrWhiteSpace(_connectionString))
                return false;

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            //Initialize parameters with DBNull.Value to avoid "Must declare the scalar variable" errors if SP expects them.
            cmd.Parameters.AddWithValue("@EmployeeID", eID);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@BadgeNumber", badgeNum);
            cmd.Parameters.AddWithValue("@TitleID", titleid);
            cmd.Parameters.AddWithValue("@StartDate", startdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            cmd.Parameters.AddWithValue("@BirthDate", birthdate);

            await conn.OpenAsync();

            // Execute reader asynchronously and stream rows
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> TerminateEmployeeAsync(int empid)
        {
            // Example stored procedure name. Replace with your actual SP name.
            const string storedProcedureName = "SP_TerminateEmployee";

            if (string.IsNullOrWhiteSpace(_connectionString))
                return false;

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            //Initialize parameters with DBNull.Value to avoid "Must declare the scalar variable" errors if SP expects them.
            cmd.Parameters.AddWithValue("@EmployeeID", empid);

            await conn.OpenAsync();

            // Execute reader asynchronously and stream rows
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        // New: read Title lookup values for the dropdown
        public async Task<List<EmployeeTitles>> GetTitlesAsync()
        {
            var results = new List<EmployeeTitles>();

            if (string.IsNullOrWhiteSpace(_connectionString))
                return results;

            const string sql = "SELECT EmpTitleID, Title FROM Employee_Titles";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, conn)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 60
            };

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await reader.ReadAsync())
            {
                var title = new EmployeeTitles
                {
                    Id = reader["EmpTitleID"] != DBNull.Value ? Convert.ToInt32(reader["EmpTitleID"]) : 0,
                    Name = reader["Title"]?.ToString() ?? string.Empty
                };
                results.Add(title);
            }

            return results;
        }
    }
}
