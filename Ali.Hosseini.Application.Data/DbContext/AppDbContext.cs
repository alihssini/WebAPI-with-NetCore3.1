using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Microsoft.EntityFrameworkCore;

namespace Ali.Hosseini.Application.Data.DBContext
{
    public class AppDbContext : DbContext
    {
        #region Ctor
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        #endregion
        #region Props
        public DbSet<Applicant> Applicants { get; set; }
        #endregion
 
    }
}
