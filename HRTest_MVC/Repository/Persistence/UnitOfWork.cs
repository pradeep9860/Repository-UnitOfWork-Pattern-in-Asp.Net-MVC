using HRTest_MVC.Core;
using HRTest_MVC.Core.Repositories;
using HRTest_MVC.Persistence;
using HRTest_MVC.Persistence.Repositories; 

namespace HRTest_MVC.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlutoContext _context;

        public UnitOfWork(PlutoContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Roles = new RolesRepository(_context);
            Employees = new EmployeeRepository(_context);
            Timesheets = new TimesheetRepository(_context);
        }

        public IUserRepository Users { get; set; }
        public IRolesRepository Roles { get; set; }
        public IEmployeeRepository Employees { get; set; }
        public ITimesheetRepository Timesheets { get; set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}