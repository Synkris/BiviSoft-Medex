using Medex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Medex.DATA
{
    public class MedexDbContext : IdentityDbContext
    {
        public MedexDbContext(DbContextOptions<MedexDbContext> Options) : base(Options)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<WorkHour> WorkHours { get; set; }
       
    }   
}
