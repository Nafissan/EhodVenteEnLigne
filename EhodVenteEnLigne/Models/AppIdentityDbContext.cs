using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EhodBoutiqueEnLigne.Models
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        private IDbConnection DbConnection { get; }

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options, IConfiguration config)
        : base(options)
        {
            DbConnection = new SqlConnection(config.GetConnectionString("EhodVLUtilisateurs"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection.ConnectionString, providerOptions => providerOptions.EnableRetryOnFailure());
            }
        }
    }
}