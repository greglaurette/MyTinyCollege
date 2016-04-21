using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTinyCollege.Models
{
    public class Instructor:Person
    {
        public DateTime HireDate { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual OfficeAssinment OfficeAssignment { get; set; }
    }
}