using Medex.DATA;
using Medex.IHelper;
using Medex.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly MedexDbContext _db;
		private readonly IAccountService ourAccountService;
        public DepartmentController(MedexDbContext db, IAccountService injectedAccountService)
        {
            _db = db;
            ourAccountService = injectedAccountService;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> ObjList = _db.Departments;
            return View(ObjList);
        }

        // GET || AddDepartment
        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }

       // POST || AddDepartment
       [HttpPost]
        public JsonResult AddNewDepartment(string department)
        {

			if (!string.IsNullOrWhiteSpace(department))
			{
				var deserializeDepartment = JsonConvert.DeserializeObject<Department>(department);
				if (deserializeDepartment != null)
				{
                    var newInstantOfDepartment = new Department();
                    {
                        newInstantOfDepartment.Name = deserializeDepartment.Name;
                        newInstantOfDepartment.Active = deserializeDepartment.Active;
                    }
                    _db.Add(newInstantOfDepartment);
                    var saveChanges = _db.SaveChanges();
                    if(saveChanges != 0)
                    {
                        return Json(new { isError = false, msg = "Department created Successfully " });
                    }
                    else
                    {
                        return Json(new { isError = true, msg = " Department couldn't be created " });
                    }

				}				
				return Json(new { isError = true, msg = "Department is Empty" });
			}
			return Json(new { isError = true, msg = "Failed please try again" });
		}
    }
}
