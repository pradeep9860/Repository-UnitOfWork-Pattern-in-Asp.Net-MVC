using HRTest_MVC.Models;
using HRTest_MVC.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace HRTest_MVC.Controllers
{
    //[Authorize]
    public class TimesheetController : BaseController
    {

        public TimesheetController()
        {

        }

        [HttpPost]
        public JsonResult CheckIn(Timesheet model)
        {
            using (var unitOfWork = new UnitOfWork(new PlutoContext()))
            {
                try
                {
                    model.UserId = GetCurrentUser();
                    var data = unitOfWork.Timesheets.SaveTimesheet(model);
                    return Json(new { Code = 200, Message = "success", Data = data }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Code = 500, Message = "Exception", Data = "" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult CheckOut(Timesheet model)
        {
            try
            {
                using (var unitOfWorks = new UnitOfWork(new PlutoContext()))
                {
                    model.UserId = GetCurrentUser();
                    var data = unitOfWorks.Timesheets.SaveTimesheet(model);
                    return Json(new { Code = 200, Message = "success", Data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Message = "Exception", Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTimesheetByUser()
        {
            try
            {
                using (var unitOfWorks = new UnitOfWork(new PlutoContext()))
                {
                    var userId = GetCurrentUser();
                    var data = unitOfWorks.Timesheets.GetTodayTimesheetByUser(userId);
                    return Json(new { Code = 200, Message = "success", Data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Message = "Exception", Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }


    
    }
}