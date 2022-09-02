using Medex.DATA;
using Medex.IHelper;
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
          private readonly  IAccountService ourAccountService;
        public AccountController(MedexDbContext db, UserManager<Doctor> injectedUserManager, SignInManager<Doctor> injectedSignInManager, 
            IWebHostEnvironment injectedWebHostEnvironment, IAccountService injectedAccountService)
        {
            _db = db;
            ourUserManger = injectedUserManager;
            signInManager = injectedSignInManager;
            _webHostEnvironment = injectedWebHostEnvironment;
            ourAccountService = injectedAccountService;
        }


        //GET || Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.MedexDepartment = ourAccountService.GetAllTheDepartment();
            return View();
        }

        [HttpPost]
        //POST || Register
        public async Task<IActionResult> Register(Doctor doctorDetailsForReg)
        {
            try
            {
                // We are Validating User Details For Register
                ViewBag.MedexDepartment = ourAccountService.GetAllTheDepartment();


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
                //  Validating User Details For Register stops here

                // Query the user details if it exists in thr Db B4 Authentication
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
                // End of Query  4  the user details if it exists in thr Db B4 Authentication
                if (doctorDetailsForReg.ImageUrl == null)
                {
                    doctorDetailsForReg.Message = "Please Upload your picture";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);
                }

                var returndResultFrmRegisterService = await  ourAccountService.RegisterDoctorService(doctorDetailsForReg);
                if (returndResultFrmRegisterService != null)
                {
                    doctorDetailsForReg.Message = "Doctor Created Succesfully.";
                    doctorDetailsForReg.ErrorHappened = false;
                    return RedirectToAction("WorkHourSetUp", "Doctor");
                }
                else
                {
                    doctorDetailsForReg.Message = "Internal Error Occured";
                    doctorDetailsForReg.ErrorHappened = true;
                    return View(doctorDetailsForReg);


                }

            }
            catch (Exception)
            {

                throw;
            }         
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

                // We are Validating User Details For Login
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
                //  Validating User Details For Login stops here

                // Query  4  the user with emaail detail if it exists in thr Db B4 Authentication
                var queryDoctorTableWithEmail = await  ourAccountService.FindWithEmailAsync(doctorDetailsForLogin.Email);
                if (queryDoctorTableWithEmail == null)
                {
                    doctorDetailsForLogin.Message = "Your details was not found in our database, plaese register.";
                    doctorDetailsForLogin.ErrorHappened = true;
                    return View(doctorDetailsForLogin);
                }
                // End of Query  4  the user details if it exists in thr Db B4 Authentication


                // We are using Ef Core IDDB SignIn Method with the params it needed to sign the user In & if it Succeeded it goes to the Dashboard
                var result = await signInManager.PasswordSignInAsync(doctorDetailsForLogin.Email, doctorDetailsForLogin.Password, true, false);
                if (result.Succeeded)
                {

                    return RedirectToAction("ViewDoctor", "Patient");
                }
                // We are using Ef Core IDDB SignIn Method with the params it needed to sign the user In & if it Succeeded it goes to the Dashboard


                doctorDetailsForLogin.Message = "wrong password, log-in failed.";
                doctorDetailsForLogin.ErrorHappened = true;
                return View(doctorDetailsForLogin);


            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //GET || Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Doctors.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST || Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Doctor doctorDetailsForReg)
        {
            if (ModelState.IsValid)
            {
                _db.Doctors.Update(doctorDetailsForReg);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctorDetailsForReg);


        }
        //GET || Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Doctors.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST || Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Doctor doctorDetailsForReg)
        {
            if (doctorDetailsForReg == null)
            {

                return NotFound();

            }
            _db.Doctors.Remove(doctorDetailsForReg);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }





    }
}
