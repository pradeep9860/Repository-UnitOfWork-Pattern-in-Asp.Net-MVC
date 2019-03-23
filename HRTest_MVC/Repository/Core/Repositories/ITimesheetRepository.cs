using HRTest_MVC.Models;
using HRTest_MVC.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace HRTest_MVC.Core.Repositories
{
    public interface ITimesheetRepository : IRepository<Timesheet>
    {
        Timesheet SaveTimesheet(Timesheet model);
        IEnumerable<Timesheet> GetAllTimesheet(Timesheet model);
        IEnumerable<Timesheet> GetTodayTimesheet();
        IEnumerable<Timesheet> GetTodayTimesheetByUser(int userId);

        IEnumerable<ReportViewModel> GetAllReport();
        IEnumerable<ReportViewModel> GetAllReportForThatDay();
    }
}