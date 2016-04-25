using MyTinyCollege.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MyTinyCollege.DAL
{
    public class SchoolContext:DbContext
    {
        public SchoolContext():base("DefaultConnection")
        {            
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }        
        public DbSet<Course> Courses { get; set; }
        public DbSet<OfficeAssinment> OfficeAssignments { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /* Using Fluent API
             *  Take care of many to many relationships between instructor and course entities
             *  EF Code Firt can configure this for us, but if we do not override the names
             *  we will get mappings such as InstructorInstructorID for the InstructorID column
             */
            modelBuilder.Entity<Course>().HasMany(c => c.Instructors).WithMany(i => i.Courses).Map(t=>t.MapLeftKey("CourseID").MapRightKey("InstructorID").ToTable("CourseInstructor"));
            //Above will create a junction (bridging) table called
            //CourseInstructor
            //with 2FK columns CourseID and InstructorID
            //and a composite PK on CourseID + InstructorID
        }

    }
}