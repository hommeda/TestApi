using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Resturant.Data
{
    public class ResturantDbDapperContext
    {
        #region Private Member
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        #endregion

        #region Constructor
        public ResturantDbDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        #endregion

        #region Logic

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> Execute(string commandText,DynamicParameters? parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = CreateConnection();
            return await db.ExecuteAsync(commandText, parms, commandType:commandType);
        }

        public async Task<T> GetFirstAsync<T>(string commandText, DynamicParameters? parms, CommandType commandType = CommandType.StoredProcedure)
        {

            using IDbConnection db = CreateConnection();
            return await db.QueryFirstOrDefaultAsync(commandText, parms, commandType: commandType);
        }

        public async Task<List<T>> Get<T>(string commandText, DynamicParameters? parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = CreateConnection();
            return (await db.QueryAsync<T>(commandText, parms, commandType: commandType)).ToList();
        }
        #endregion
    }
}
