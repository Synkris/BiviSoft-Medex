using Medex.DATA;
using Medex.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class DoctorController : Controller
    {
        private readonly MedexDbContext _db;
        private readonly UserManager<Doctor> ourUserManger;
        public DoctorController(MedexDbContext db, UserManager<Doctor> injectedUserManager)
        {
            _db = db;
            ourUserManger = injectedUserManager;
        }

        //GET || WorkHourSetUp
        [HttpGet]
        public IActionResult WorkHourSetUp()
        {
            var logedInUser = User.Identity.Name;
            if(logedInUser == null)
            {
                return RedirectToAction("LogIn","Account" );
            }
            ViewBag.DaysOfTheWeek = DaysOfTheWeek();
            return View();
        }

        //POST || WorkHourSetUp
        [HttpPost]
        public IActionResult WorkHourSetUp(WorkHour WorkHourSetUp)
        {
            try
            {
                ViewBag.DaysOfTheWeek = DaysOfTheWeek();

                if (((int)WorkHourSetUp.WeekDays) == 11)
                {
                    WorkHourSetUp.Message = "please put the day of the week";
                    WorkHourSetUp.ErrorHappened = true;
                    return View(WorkHourSetUp);

                }
                if (WorkHourSetUp.StartTime == TimeSpan.MinValue)
                {
                    WorkHourSetUp.Message = "please input your start time.";
                    WorkHourSetUp.ErrorHappened = true;
                    return View(WorkHourSetUp);
                }


                if (WorkHourSetUp.EndTime == TimeSpan.MinValue)
                {
                    WorkHourSetUp.Message = "please input your end time.";
                    WorkHourSetUp.ErrorHappened = true;
                    return View(WorkHourSetUp);
                }

                if (WorkHourSetUp.ActiveHours != 0)
                {
                    WorkHourSetUp.Message = "please input your active hour.";
                    WorkHourSetUp.ErrorHappened = true;
                    return View(WorkHourSetUp);
                }

                var currentUserName = User.Identity.Name;
                var fullLogedInUser = ourUserManger.Users.Where(s => s.UserName == currentUserName).FirstOrDefault();
                if(fullLogedInUser != null)
                {
                    var workHour = WorkHourSetUp.EndTime.Hours - WorkHourSetUp.StartTime.Hours;
                    var totalWorkHours = workHour;
                    var newInstanceOfWorkHourSetUpAboutToBCreated = new WorkHour();
                    {
                        newInstanceOfWorkHourSetUpAboutToBCreated.WeekDays = WorkHourSetUp.WeekDays;
                        newInstanceOfWorkHourSetUpAboutToBCreated.StartTime = WorkHourSetUp.StartTime;
                        newInstanceOfWorkHourSetUpAboutToBCreated.EndTime = WorkHourSetUp.EndTime;
                        newInstanceOfWorkHourSetUpAboutToBCreated.ActiveHours = totalWorkHours;
                        newInstanceOfWorkHourSetUpAboutToBCreated.DoctorId = fullLogedInUser.Id;
                    };
                    _db.Add(newInstanceOfWorkHourSetUpAboutToBCreated);
                    var justSaveChanges = _db.SaveChanges();

                    if (justSaveChanges != 0)
                    {
                        WorkHourSetUp.Message = "Doctor Work Hour Created Succesfully.";
                        WorkHourSetUp.ErrorHappened = false;
                        return View("DashBoard");
                    }
                }

               
                WorkHourSetUp.Message = "Internal Error Occured";
                WorkHourSetUp.ErrorHappened = true;
                return View(WorkHourSetUp);

            }
            catch (Exception)
            {

                throw;
            }

        }
        //GET || DashBoard
        [HttpGet]
        public IActionResult DashBoard()
        {
            var logggedInDoctorUserName = User.Identity.Name;
            var LoggedInDoctorFullDetails = ourUserManger.Users.Where(s => s.UserName == logggedInDoctorUserName).Include(d => d.Department).FirstOrDefault();
            var queriedWorkHourRecordsWitCurrentDoctorId = _db.WorkHours.Where(wkH => wkH.DoctorId == LoggedInDoctorFullDetails.Id).Include(d => d.Doctor).ToList();

            if(queriedWorkHourRecordsWitCurrentDoctorId != null && queriedWorkHourRecordsWitCurrentDoctorId.Count() > 0)
            {
                return View(queriedWorkHourRecordsWitCurrentDoctorId);
            }
            return View();
        }


        public List<TemporaryModel> DaysOfTheWeek()
        { 
            var defualtTemporaryModel = new TemporaryModel()
                {
                    Id = 11,
                    Name = "---- Select Day of the week ----"
                };
    
            var getAllTheDaysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
             .Select(tempoModel => new TemporaryModel()
             { Id = (int)tempoModel, Name = tempoModel.ToString() })
             .OrderBy(x => x.Id).ToList();

            getAllTheDaysOfWeek.Insert(0, defualtTemporaryModel);
            return getAllTheDaysOfWeek;

        }

        public class TemporaryModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }


}
