using Medex.DATA;
using Medex.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class PatientController : Controller
    {
        private readonly MedexDbContext _db;
        private readonly UserManager<Doctor> ourUserManger;
        private readonly SignInManager<Doctor> signInManager;

        public PatientController(MedexDbContext db, UserManager<Doctor> injectedUserManager, SignInManager<Doctor> injectedSignInManager)
        {
            _db = db;
            ourUserManger = injectedUserManager;
            signInManager = injectedSignInManager;
        }



        //GET || Search
        [HttpGet]
        public IActionResult ViewDoctor()
        {
            var allMedexDoctors = ourUserManger.Users.Where(s => s.Id != null && s.ProfilePicture != null).Include(d => d.Department).ToList();
            if(allMedexDoctors != null && allMedexDoctors.Count() > 0 )
            {
                return View(allMedexDoctors);
            }
           
            return View();  
        }
    }
}
