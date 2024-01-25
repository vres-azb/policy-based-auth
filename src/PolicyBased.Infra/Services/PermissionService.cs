using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PolicyBased.Contracts;
using PolicyBased.Contracts.Services;
using PolicyBased.Infra.Persistence.Context;

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
            string permSP = "EXEC [pbac].[GetUserPermissions] @UserId";
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
                        PolicyName = rdr.GetString("PolicyName"),
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
            var policies = polList.Select(a => a.PolicyName).Distinct().AsEnumerable();
            var roles = polList.Select(a => a.Role).Distinct().AsEnumerable();
            var permissions = polList.Select(a => a.Permission).Distinct().AsEnumerable();
            return new PolicyResult() { Policies = policies, Roles = roles, Permissions = permissions };
        }

        public async Task<LoggedInUser> GetLoggedInUserAsync(string userName)
        {
            LoggedInUser result;

            var userEF = await this._dbContext.Users
                .SingleOrDefaultAsync(x => x.UserName == userName);

            result = userEF == null
                ? new LoggedInUser
                {
                    Sub = "75",
                    Name = userName
                }
                : new LoggedInUser
                {
                    Sub = userEF.UserId,
                    Email = userEF.Email,
                    Name = userEF.UserName,
                    TenantId = userEF.TenantId
                };


            return result;
        }

        public class PolicyRow
        {
            public int UserId { get; set; } = default!;
            public string Permission { get; set; } = default!;
            public string Role { get; set; } = default!;
            public string PolicyName { get; internal set; }
        }
    }
}
