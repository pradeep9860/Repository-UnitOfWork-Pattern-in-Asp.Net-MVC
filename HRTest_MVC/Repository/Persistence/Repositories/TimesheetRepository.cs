using HRTest_MVC.Core.Repositories;
using HRTest_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace HRTest_MVC.Persistence.Repositories
{
    public class TimesheetRepository : Repository<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(PlutoContext context) 
            : base(context)
        {
        }

        public Timesheet SaveTimesheet(Timesheet model)
        {
             
            var timesheet = PlutoContext.Timesheets.Add(model);
            PlutoContext.SaveChanges(); 
            return timesheet;
        }

        public IEnumerable<Timesheet> GetAllTimesheet(Timesheet model)
        { 
            var timesheet = PlutoContext.Timesheets.OrderBy(s => s.CheckInAt); 
            return timesheet;
        }

        public IEnumerable<Timesheet> GetTodayTimesheet()
        {
            var timesheet = PlutoContext.Timesheets.OrderBy(s => s.CheckInAt).Where(s=> s.CheckInAt.Date == DateTime.Now.Date );
            return timesheet;
        }

        public IEnumerable<Timesheet> GetTodayTimesheetByUser(int userId)
        {
            var today = DateTime.Now.Date;
            var timesheet = PlutoContext.Timesheets
                            .Where(x => x.UserId == userId &&
                                         x.CheckInAt.Year == today.Year &&
                                         x.CheckInAt.Month == today.Month &&
                                         x.CheckInAt.Day == today.Day)
                            .ToList(); 
            return timesheet;
        }

        public IEnumerable<ReportViewModel> GetAllReport()
        { 
            var result = PlutoContext.Database
                .SqlQuery<ReportViewModel>("ReportList")
                .ToList();

            return result;
        }

        public IEnumerable<ReportViewModel> GetAllReportForThatDay()
        {
            var DateAt = new SqlParameter("@DateAt", DateTime.Now);
           
            var result = PlutoContext.Database
                .SqlQuery<ReportViewModel>("ReportListForSpecificDate @DateAt", DateAt)
                .ToList();

            return result;
        }

        public PlutoContext PlutoContext
        {
            get { return Context as PlutoContext; }
        }
    }
}