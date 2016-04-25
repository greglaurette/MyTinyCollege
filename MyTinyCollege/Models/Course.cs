using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTinyCollege.Models
{
    public class Course
    {
        /* By default the ID property wll become the Primary Key of the database table
         * that corresponds t this class.  By default the EF(Entity Framwork)
         * interprets a property that's name ID or ClassNameID as the PK.
         * Also, this PK will have IDENTITY property, you can override it by using the 
         * DatabaseGeneratedOption enum:
         * -Computed:   Databased generates a value when row is inserted or updated
         * -Identity:   Database generates a value when row is inserted
         * -None:       Database does not generate values        
         */
         [DatabaseGenerated(DatabaseGeneratedOption.None)]
         [Display(Name = "Number")]
        public int CourseID { get; set; } //PK - Note: with no Identity Property

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0,5)]// (x, y) x = min y = max
        public int Credits { get; set; }
        public int DepartmentID { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual Department Department { get; set; }
        public string CourseIDTitle
        {
            get
            {
                return CourseID + ": " + Title;
            }
        }

    }
}