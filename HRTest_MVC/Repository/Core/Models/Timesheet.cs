using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRTest_MVC.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Check in Date time is required")]
        public DateTime CheckInAt { get; set; }
 
        [Required(ErrorMessage = "Check out Date time is required")] 
        public DateTime CheckOutAt { get; set; }
 
        public long TimeDuration { get; set; }

        public int UserId { get; set; }  

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }

    public class ReportViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime FirstCheckInAt { get; set; }
        public DateTime LastCheckoutAt { get; set; }
        public long TotalDuration { get; set; }
    }
}