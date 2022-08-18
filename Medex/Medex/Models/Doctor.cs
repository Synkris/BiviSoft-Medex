using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Models
{
    public class Doctor : IdentityUser
    { 
   
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public int DepartmentId { get; set; }
        [Display(Name = "Department Name")]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        

        [DisplayName("Profile Picture")]
        public string ProfilePicture { get; set; }

        [NotMapped]
        public IFormFile ImageUrl { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool ErrorHappened { get; set; }



    }
}
