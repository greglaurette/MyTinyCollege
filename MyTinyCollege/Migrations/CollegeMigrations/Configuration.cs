namespace MyTinyCollege.Migrations.CollegeMigrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\CollegeMigrations";
        }

        protected override void Seed(MyTinyCollege.DAL.SchoolContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //1. add students
            var students = new List<Student>
            {
                new Student {FirstName="Carson", LastName="Alexander", EnrollmentDate=DateTime.Parse("2015-02-01"), Email="calexander@tinycollege.com" },
                new Student {FirstName="Alonso", LastName="Arturo", EnrollmentDate=DateTime.Parse("2015-02-01"), Email="aarturo@tinycollege.com" },
                new Student {FirstName="John", LastName="Smith", EnrollmentDate=DateTime.Parse("2015-02-01"), Email="jsmith@tinycollege.com" },
                new Student {FirstName="Frank", LastName="Bekkering", EnrollmentDate=DateTime.Parse("2015-02-01"), Email="fbekkering@tinycollege.com" },
                new Student {FirstName="Laura", LastName="Norman", EnrollmentDate=DateTime.Parse("2015-02-01"), Email="lnorman@tinycollege.com" }

            };

            students.ForEach(s => context.Students.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //2. add instructors

            var instructors = new List<Instructor>
            {
                new Instructor {FirstName="Marc", LastName="Williams", HireDate=DateTime.Parse("2013-07-12"), Email="mwilliams@facultiytinycollege.com" },
                new Instructor {FirstName="Terrence", LastName="Clark", HireDate=DateTime.Parse("2013-07-12"), Email="tclark@facultiytinycollege.com" }

            };

            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //add departments
            var departments = new List<Department>
            {
                new Department {Name="Engineering", Budget=350000, StartDate=DateTime.Parse("2010-05-19"), InstructorID=1},
                new Department {Name="English", Budget=15000, StartDate=DateTime.Parse("2010-05-19"), InstructorID=2}
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            //3. add courses

            var courses = new List<Course>
            {
                new Course {CourseID=1045, Title="Chemistry",Credits=3, DepartmentID=1},
                new Course {CourseID=4022, Title="Physics",Credits=3, DepartmentID=1},
                new Course {CourseID=3141, Title="Calculus",Credits=3, DepartmentID=1},
                new Course {CourseID=2021, Title="Literature",Credits=3, DepartmentID=2}
            };

            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseID, s));
            context.SaveChanges();

            //4. add enrollments
            var enrollments = new List<Enrollment>
            {
                new Enrollment {StudentID=1, CourseID=1045, Grade=Grade.A },
                new Enrollment {StudentID=1, CourseID=4022, Grade=Grade.B },
                new Enrollment {StudentID=2, CourseID=3141, Grade=Grade.C },
                new Enrollment {StudentID=2, CourseID=1045, Grade=Grade.B },
                new Enrollment {StudentID=3, CourseID=2021, Grade=Grade.B },
                new Enrollment {StudentID=3, CourseID=3141, Grade=Grade.C }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDatabase = context.Enrollments.Where(s => s.StudentID == e.StudentID && s.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDatabase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();
        }
    }
}
