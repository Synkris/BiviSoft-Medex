using Medex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.IHelper
{
    public interface IAccountService
    {

        Task<Doctor> RegisterDoctorService(Doctor doctorDetailsForReg);

        string UploadedFile(Doctor filesSender);

        Task<Doctor> FindWithEmailAsync(string Email);

        List<Department> GetAllTheDepartment();

        List<TemporaryModel> DaysOfTheWeek();

        Task<WorkHour> RegisterWorkHourSetUp(WorkHour WorkHourSetUp, string currentUserName);

        public string UpdateEditWorkHourSetUp(WorkHour doctorsNewWorkHour);

    }
}
