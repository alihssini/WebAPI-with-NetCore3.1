using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Data.Repository
{
    public class ApplicantRepository : IApplicantRepository
    {
        #region Vars
        private readonly AppDbContext _context;
        #endregion
        #region Ctor
        public ApplicantRepository(AppDbContext context) => _context = context;
        #endregion
        #region Impl
        public async Task<bool> DeleteAsync(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null) return false;
            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Applicant> GetAsync(int key)
        {
            return await _context.Applicants.FindAsync(key);
        }

        public async Task<Applicant> InsertAsync(Applicant entity)
        {
            _context.Applicants.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Applicant> UpdateAsync(Applicant entity)
        {
            Applicant existing = await _context.Applicants.FindAsync(entity.ID);
            if (existing != null)
            {
                try
                {
                    _context.Entry(existing).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GetAll().Any(e => e.ID == entity.ID))
                    {
                        return null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return existing;
        }
        public IQueryable<Applicant> GetAll()
        {
            return _context.Set<Applicant>();
        }
        #endregion
    }
}
