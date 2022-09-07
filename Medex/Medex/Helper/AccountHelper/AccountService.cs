using Medex.DATA;
using Medex.IHelper;
using Medex.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Helper.AccountHelper
{
    public class AccountService : IAccountService
    {
        private readonly MedexDbContext _db;
        private readonly UserManager<Doctor> ourUserManger;
        private readonly SignInManager<Doctor> signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public AccountService(MedexDbContext db, UserManager<Doctor> injectedUserManager, SignInManager<Doctor> injectedSignInManager, IWebHostEnvironment injectedWebHostEnvironment)
        {
            _db = db;
            ourUserManger = injectedUserManager;
            signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
        }



        public async Task< Doctor> RegisterDoctorService(Doctor doctorDetailsForReg)
        {
            string docProfilePictureFilePath = string.Empty;

            if (doctorDetailsForReg.ImageUrl != null)
            {
                docProfilePictureFilePath = UploadedFile(doctorDetailsForReg);
            }

            var newInstanceOfDoctorModelAboutToBCreated = new Doctor();
            {
                newInstanceOfDoctorModelAboutToBCreated.FirstName = doctorDetailsForReg.FirstName;
                newInstanceOfDoctorModelAboutToBCreated.LastName = doctorDetailsForReg.LastName;
                newInstanceOfDoctorModelAboutToBCreated.Email = doctorDetailsForReg.Email;
                newInstanceOfDoctorModelAboutToBCreated.PhoneNumber = doctorDetailsForReg.PhoneNumber;
                newInstanceOfDoctorModelAboutToBCreated.DepartmentId = doctorDetailsForReg.DepartmentId;
                newInstanceOfDoctorModelAboutToBCreated.UserName = doctorDetailsForReg.Email;
                newInstanceOfDoctorModelAboutToBCreated.ProfilePicture = docProfilePictureFilePath;
            };


            var cretedDoctor =  await ourUserManger.CreateAsync(newInstanceOfDoctorModelAboutToBCreated, doctorDetailsForReg.Password);
            if (cretedDoctor.Succeeded)
            {
                return newInstanceOfDoctorModelAboutToBCreated;
            }
            else
            {
                return null;
            }
        }

       



        //We collected user's profile pics and create a method which collects it, and store them in one directory 
        public string UploadedFile(Doctor filesSender)
        {
            string uniqueFileName = string.Empty;

            if (filesSender.ImageUrl != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "doctorUploads");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doctorUploads");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + filesSender.ImageUrl.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                   // filesSender.ImageUrl.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/doctorUploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }

        public async Task<Doctor> FindWithEmailAsync(string Email)
        {
            return await ourUserManger.Users.Where(s => s.Email == Email)?.FirstOrDefaultAsync();
        }

        //public async Task<Doctor> FindWithEmailAsync(string Email)
        //{
        //    return await ourUserManger.Users.Where(s => s.Email == Email)?.FirstOrDefaultAsync();
        //}

        //this is a method that lists out the  Department as a droopdown in Doctor registration
        public List<Department> GetAllTheDepartment()
        {
            var newDepartment = new Department()
            {
                Id = 0,
                Name = "---Select Department---"
            };
            var getAllTheDepartment = _db.Departments.Where(x => x.Active == true).ToList();
            getAllTheDepartment.Insert(0, newDepartment);

            return getAllTheDepartment;
        }

        //this is a method that lists out the days of the week as a droopdown in WorkHourSetUp
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

        public async Task<WorkHour> RegisterWorkHourSetUp(WorkHour WorkHourSetUp, string currentUserName )
        {

            var fullLogedInUser = await ourUserManger.Users.Where(s => s.UserName == currentUserName).FirstOrDefaultAsync();
            if (fullLogedInUser != null)
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
                    return newInstanceOfWorkHourSetUpAboutToBCreated;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public string UpdateEditWorkHourSetUp(WorkHour doctorsNewWorkHour)
        {
            var doctorsOldDetails = _db.WorkHours.Find(doctorsNewWorkHour.Id);
            if (doctorsOldDetails != null)
            {
                var workHour = doctorsOldDetails.EndTime.Hours - doctorsOldDetails.StartTime.Hours;
                var totalWorkHours = workHour;
                {
                    doctorsOldDetails.WeekDays = doctorsNewWorkHour.WeekDays;
                    doctorsOldDetails.StartTime = doctorsNewWorkHour.StartTime;
                    doctorsOldDetails.EndTime = doctorsNewWorkHour.EndTime;
                    doctorsOldDetails.ActiveHours = totalWorkHours;

                    _db.Update(doctorsOldDetails);
                    _db.SaveChanges();

                    return "Work Hour Updated Successfully";
                }
               
            }
            return "Work Hour Updated Failed";
        }
    }
}
