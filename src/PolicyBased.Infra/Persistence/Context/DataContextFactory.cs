using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PolicyBased.Infra.Persistence.Context
{
    public class DataContextFactory : IDataContextFactory, IDesignTimeDbContextFactory<PolicyTestDBContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string dbCon = string.Empty;

        public DataContextFactory() { }

        public DataContextFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            var dataPath = _configuration.GetConnectionString("DefaultConnection");
            var dirPath = Directory.GetParent(Directory.GetCurrentDirectory()) + $@"\AppData\demoDb.db";
            dbCon = dataPath;
        }

        public PolicyTestDBContext CreateContext()
        {
            var signedInUser = _httpContextAccessor.HttpContext.User ?? null;
            var options = new DbContextOptionsBuilder<PolicyTestDBContext>()
                .UseSqlServer(dbCon)
                .Options;
            //, signedInUser?.Identity?.Name, signedInUser?.IsInRole("admin") ?? false
            return new PolicyTestDBContext(options);
        }

        public PolicyTestDBContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<PolicyTestDBContext>()
                .UseSqlite(dbCon)
                .Options;

            return new PolicyTestDBContext(options);
        }
    }
}
