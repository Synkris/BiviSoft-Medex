using Medex.DATA;
using Medex.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class WorkHourController : Controller
    {
        private readonly MedexDbContext _db;
        private readonly UserManager<Doctor> _ourUserManger;
        public WorkHourController(MedexDbContext db, UserManager<Doctor> ourUserManger)
        {
            _db = db;
            _ourUserManger = ourUserManger;
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(string userId)
        //{
        //    var userIdForDeactivation =_ourUserManger.FindByIdAsync(userId).Result;
        //    if (userIdForDeactivation != null)
        //    {
        //        userIdForDeactivation.Deactivated = true;
        //        _db.Update(userIdForDeactivation);
        //        _db.SaveChanges();

        //        return RedirectToAction("Dashboard");

        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

    }
}
