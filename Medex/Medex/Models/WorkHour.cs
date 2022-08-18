using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Models
{
    public class WorkHour : DefualtBaseModel
    {
        public DayOfWeek WeekDays{ get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int ActiveHours { get; set; }

        public string DoctorId { get; set; }
        [Display(Name = "DoctorId")]
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool ErrorHappened { get; set; }




    }
}
