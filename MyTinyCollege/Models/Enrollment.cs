using System.ComponentModel.DataAnnotations;

namespace MyTinyCollege.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        public virtual Student student { get; set; }
        public virtual Course course { get; set; }
        
        [DisplayFormat(NullDisplayText = "No Grade")]
        public Grade? Grade { get; set; }
    }
}