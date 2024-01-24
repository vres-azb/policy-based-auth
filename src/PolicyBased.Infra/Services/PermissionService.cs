using Microsoft.EntityFrameworkCore;
using PolicyBased.Contracts;
using PolicyBased.Contracts.Services;
using PolicyBased.Infra.Persistence.Context;
using Microsoft.Data.SqlClient;
using System.Data;
namespace PolicyBased.Infra.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly PolicyTestDBContext _dbContext;

        public PermissionService(PolicyTestDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PolicyResult> GetPermissions(string userId)
        {
            string permSP = "EXEC [dbo].[GetUserPermissions] @UserId";
            List<SqlParameter> parms = new()
            {
                 new SqlParameter { ParameterName = "@UserId", Value = userId },
             };
            var db = _dbContext.Database;
            var dbCon = db.GetDbConnection();
            var cmd = dbCon.CreateCommand();
            cmd.CommandText = permSP;
            cmd.Parameters.AddRange(parms.ToArray());
            List<PolicyRow> polList = new();
            try
            {
                await db.OpenConnectionAsync();
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    var pol = new PolicyRow()
                    {
                        UserId = rdr.GetInt32("userid"),
                        Permission = rdr.GetString("PermissionName"),
                        Role = rdr.GetString("RoleName"),
                    };
                    polList.Add(pol);
                }
            }
            finally
            {
                await dbCon.CloseAsync();
            }
            var roles = polList.Select(a => a.Role).Distinct().AsEnumerable();
            var permissions = polList.Select(a => a.Permission).Distinct().AsEnumerable();
            return new PolicyResult() { Roles = roles, Permissions = permissions };
        }

        public class PolicyRow
        {
            public int UserId { get; set; } = default!;
            public string Permission { get; set; } = default!;
            public string Role { get; set; } = default!;

        }
    }
}
