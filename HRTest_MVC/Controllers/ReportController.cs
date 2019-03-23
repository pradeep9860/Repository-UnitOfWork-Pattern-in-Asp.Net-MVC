using HRTest_MVC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTest_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : BaseController
    {
        public ActionResult Index()
        {
            using (var unitOfWorks = new UnitOfWork(new PlutoContext()))
            {
                var data = unitOfWorks.Timesheets.GetAllReport();
                return View(data);
            }
        }
         
        public ActionResult Detail(int id)
        {
            using (var unitOfWorks = new UnitOfWork(new PlutoContext()))
            { 
                var data = unitOfWorks.Timesheets.GetTodayTimesheetByUser(id);
                return View(data);

            }
        }
    }
}