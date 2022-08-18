using Medex.DATA;
using Medex.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class AccountController : Controller
    {
          private readonly MedexDbContext _db;
          private readonly UserManager<Doctor> ourUserManger;
          private readonly SignInManager<Doctor> signInManager;
          private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(MedexDbContext db, UserManager<Doctor> injectedUserManager, SignInManager<Doctor> injectedSignInManager, IWebHostEnvironment injectedWebHostEnvironment)
          {
              _db = db;
              ourUserManger = injectedUserManager;
             signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
          }

        
        //GET || Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.MedexDepartment = GetAllTheDepartment();
            return View();
        }

        [HttpPost]
        //POST || Register
        public async Task<IActionResult> Register(Doctor doctorDetailsForReg)
        {
            try
            {
                ViewBag.MedexDepartment = GetAllTheDepartment();
                string docProfilePictureFilePath = string.Empty;

                if (doctorDetailsForReg.Email == null)
                {
                    doctorDetailsForReg.Message = "Your mail was empty,please put mail.";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                if (doctorDetailsForReg.FirstName == null || doctorDetailsForReg.LastName == null)
                {
                    doctorDetailsForReg.Message = " Please put in your first and last name.";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                if (doctorDetailsForReg.PhoneNumber == null)
                {
                    doctorDetailsForReg.Message = "Please put in your phone number.";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                if (doctorDetailsForReg.Password == null)
                {
                    doctorDetailsForReg.Message = "Please enter your password.";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                if(doctorDetailsForReg.DepartmentId == 0)
                {
                    doctorDetailsForReg.Message = "Please your department is needed.";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

               

                var queryDoctorTableWithEmail = await ourUserManger.Users.Where(s => s.Email == doctorDetailsForReg.Email)?.FirstOrDefaultAsync();
                if (queryDoctorTableWithEmail != null)
                {
                    doctorDetailsForReg.Message = "Your detail aready exist, please log in...";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                var queryDoctorDetailWithPhoneNumber = await ourUserManger.Users.Where(s => s.PhoneNumber == doctorDetailsForReg.PhoneNumber)?.FirstOrDefaultAsync();
                if (queryDoctorDetailWithPhoneNumber != null)
                {
                    doctorDetailsForReg.Message = "Your detail aready exist, please log in...";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }


                if(doctorDetailsForReg.ImageUrl == null)
                {
                    doctorDetailsForReg.Message = "Please Upload your picture";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }
                else
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
                var cretedDoctor = await ourUserManger.CreateAsync(newInstanceOfDoctorModelAboutToBCreated, doctorDetailsForReg.Password);
                if (cretedDoctor.Succeeded)
                {
                    doctorDetailsForReg.Message = "Doctor Created Succesfully.";
                    doctorDetailsForReg.ErrorHappened = false;
                    return View(doctorDetailsForReg);
                }
                doctorDetailsForReg.Message = "Internal Error Occured";
                doctorDetailsForReg.ErrorHappened = true;
                return View(doctorDetailsForReg);

            }
            catch (Exception)
            {

                throw;
            }         
        }

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
                    filesSender.ImageUrl.CopyTo(fileStream);
                }
            }
            var generatedPictureFilePath = "/doctorUploads/" + uniqueFileName;
            return generatedPictureFilePath;
        }

        //GET || Log-in
        [HttpGet]
        public IActionResult LogIn(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //POST || Log-in
        [HttpPost]
        public async Task<IActionResult> LogIn(Doctor doctorDetailsForLogin)
        {
            try
            {
                if (doctorDetailsForLogin.Email == null)
                {
                    doctorDetailsForLogin.Message = "Your mail was empty,please put mail.";
                    doctorDetailsForLogin.ErrorHappened = true;
                    return View(doctorDetailsForLogin);

                }
                if (doctorDetailsForLogin.Password == null)
                {
                    doctorDetailsForLogin.Message = "Your password was empty,please put in your password before logging in.";
                    doctorDetailsForLogin.ErrorHappened = true;
                    return View(doctorDetailsForLogin);
                }

                 var queryDoctorTableWithEmail = await ourUserManger.Users.Where(s => s.Email == doctorDetailsForLogin.Email)?.FirstOrDefaultAsync();
                if(queryDoctorTableWithEmail == null)
                {
                    doctorDetailsForLogin.Message = "Your details was not found in our database, plaese register.";
                    doctorDetailsForLogin.ErrorHappened = true;
                    return View(doctorDetailsForLogin);
                }

                var result = await signInManager.PasswordSignInAsync(queryDoctorTableWithEmail.UserName, doctorDetailsForLogin.Password, true, false);
                if (result.Succeeded)
                {

                    return RedirectToAction("ViewDoctor", "Patient");
                }
                doctorDetailsForLogin.Message = "wrong password, log-in failed.";
                doctorDetailsForLogin.ErrorHappened = true;
                return View(doctorDetailsForLogin);


            }
            catch (Exception)
            {

                throw;
            }
           
        }

            public List<Department> GetAllTheDepartment()
            {
                var newDepartment = new Department()
                {
                    Id = 0,
                    Name = "---Select Department---"
                };
                var getAllTheDepartment =  _db.Departments.Where(x => x.Active == true).ToList();
                getAllTheDepartment.Insert(0, newDepartment);

                return getAllTheDepartment;
            }


    }
}
