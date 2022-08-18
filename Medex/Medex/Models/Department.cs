using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medex.Models
{
    public class Department : DefualtBaseModel
    {
        public bool Active { get; set; }

        [NotMapped]
        public string Message { get; set; }

        [NotMapped]
        public bool ErrorHappened { get; set; }


    }
}
