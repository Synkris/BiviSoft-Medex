using Medex.DATA;
using Medex.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly MedexDbContext _db;
        public DepartmentController(MedexDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> ObjList = _db.Departments;
            return View(ObjList);
        }
    }
}
