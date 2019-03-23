using HRTest_MVC.Models; 
using System.Data.Entity;

namespace HRTest_MVC.Persistence
{
    public class PlutoContext : DbContext
    {
        public PlutoContext()
            : base("name=PlutoContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Timesheet> Timesheets { get; set; }
 
    }
}
