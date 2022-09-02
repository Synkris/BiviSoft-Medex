using Medex.DATA;
using Medex.IHelper;
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
        private readonly IAccountService ourAccountService;
        public DoctorController(MedexDbContext db, UserManager<Doctor> injectedUserManager, IAccountService injectedAccountService)
        {
            _db = db;
            ourUserManger = injectedUserManager;
            ourAccountService = injectedAccountService;
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
            ViewBag.DaysOfTheWeek = ourAccountService.DaysOfTheWeek();
            return View();
        }

        //POST || WorkHourSetUp
        [HttpPost]
        public async Task<IActionResult> WorkHourSetUpAsync(WorkHour WorkHourSetUp)
        {
            try
            {
                // We are Validating User Details For WorkHourSetUp
                ViewBag.DaysOfTheWeek = ourAccountService.DaysOfTheWeek();

                if (((int)WorkHourSetUp.WeekDays) == 0)
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



                // Query the user details with UserName, if it exists in thr Db B4 Authentication
                var currentUserName = User.Identity.Name;
                var returnedResultFrmRegWorkHour = await ourAccountService.RegisterWorkHourSetUp(WorkHourSetUp, currentUserName);
                if (returnedResultFrmRegWorkHour != null)
                {
                   WorkHourSetUp.Message = " Work Hour Created Succesfully.";
                   WorkHourSetUp.ErrorHappened = false;
                   return RedirectToAction("DashBoard","Doctor");
                }
                else
                {
                   WorkHourSetUp.Message = "Internal Error Occured";
                   WorkHourSetUp.ErrorHappened = true;
                   return View(WorkHourSetUp);

                }

            

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
            if (logggedInDoctorUserName != null)
            {
                var LoggedInDoctorFullDetails = ourUserManger.Users.Where(s => s.UserName == logggedInDoctorUserName).Include(d => d.Department).FirstOrDefault();

                var queriedWorkHourRecordsWitCurrentDoctorId = _db.WorkHours.Where(wkH => wkH.DoctorId == LoggedInDoctorFullDetails.Id /* && wkH.Deactivated != true*/).Include(d => d.Doctor).ToList();

                if (queriedWorkHourRecordsWitCurrentDoctorId != null && queriedWorkHourRecordsWitCurrentDoctorId.Count() > 0 )
                {
                    return View(queriedWorkHourRecordsWitCurrentDoctorId);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }
        // GET|| Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.DaysOfTheWeek = ourAccountService.DaysOfTheWeek();
            if (id == null)
            {
                return NotFound();
            }
            var obj = _db.WorkHours.Find(id);
            if (obj == null)
            {
                return View();
            }

            return View(obj);
        }
        //POST || Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WorkHour doctorsNewWorkHour)
        {
            ViewBag.DaysOfTheWeek = ourAccountService.DaysOfTheWeek(); 

            if (doctorsNewWorkHour != null)
            {
                var theCurruentDoctorsDetails = ourAccountService.UpdateEditWorkHourSetUp(doctorsNewWorkHour);
                if (theCurruentDoctorsDetails.Contains("Successfully"))
                {
                    doctorsNewWorkHour.Message = "Update Successfully";
                    return RedirectToAction("DashBoard", "Doctor");
                }
            }
                doctorsNewWorkHour.ErrorHappened = false;
                return View();
        }


        // GET || Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var WorkHourSetUp = _db.WorkHours.Find(id);
            if (WorkHourSetUp == null)
            {
                return NotFound();
            }

            return View(WorkHourSetUp);
        }

       // POST || Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(WorkHour WorkHourSetUp)
        {
           // var getTheUserIdToDelete = _db.WorkHours.Where(s => s.Work)
            if (WorkHourSetUp == null)
            {

                return NotFound();

            }
           // WorkHourSetUp.Deactivated = true;
            _db.WorkHours.Remove(WorkHourSetUp);
            _db.SaveChanges();
            return RedirectToAction("WorkHourSetUp");

        }
    }




    }

