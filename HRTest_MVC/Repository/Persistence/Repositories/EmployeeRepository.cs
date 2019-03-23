using HRTest_MVC.Core.Repositories;
using HRTest_MVC.Models; 
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HRTest_MVC.Persistence.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PlutoContext context) 
            : base(context)
        {
        }
 
 
        public PlutoContext PlutoContext
        {
            get { return Context as PlutoContext; }
        }
    }
}