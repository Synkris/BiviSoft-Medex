using Medex.DATA;
using Medex.Models;
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
        public WorkHourController(MedexDbContext db)
        {
            _db = db;
        }
        //public IActionResult Index()
        //{
        //   if()
        //    return View(ObjList);
        //}

    }
}
