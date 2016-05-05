using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Net;


namespace MyTinyCollege.Controllers
{
    [Authorize(Roles ="student")]
    public class StudentEnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: StudentEnrollment
        public ActionResult Index()
        {

            string userID = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userID))
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
                var currentStudent = manager.FindByEmail(User.Identity.GetUserName());
                Student student = db.Students.Include(i => i.Enrollments).Where(i => i.Email == currentStudent.Email).Single();
                string query = "select CourseID, Title from Course where CourseID not in(select distinct CourseID from Enrollment where StudentID=@p0)";
                IEnumerable<ViewModels.AssignedCourseData> data = db.Database.SqlQuery<ViewModels.AssignedCourseData>(query, student.ID);
                ViewBag.Courses = data.ToList();
                var studentEnrollments = db.Enrollments.Include(i => i.course).Include(i => i.student).Where(i => i.student.Email == currentStudent.Email);
                return View(studentEnrollments.ToList());
            }
            else
                return View();
        }

        public ActionResult Enroll(int? courseID)
        {
            if (courseID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string userID = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userID))
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
                var currentStudent = manager.FindByEmail(User.Identity.GetUserName());
                Student student = db.Students.Include(i => i.Enrollments).Where(i => i.Email == currentStudent.Email).Single();
                ViewBag.StudentID = student.ID;
                var studentEnrollments = new HashSet<int>(db.Enrollments.Include(e=>e.course).Include(e=>e.student).Where(e=>e.student.Email==currentStudent.Email).Select(e=>e.CourseID));
                int currentCourseID;
                if (courseID.HasValue)
                    currentCourseID = (int)courseID;
                else
                    currentCourseID = 0;
                if(studentEnrollments.Contains(currentCourseID))                
                    ModelState.AddModelError("AlreadyEnrolled", "Already enrolled in this course");                                 
            }
            Course course = db.Courses.Find(courseID);
            if (course == null)
                return HttpNotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll([Bind(Include ="CourseID, StudentID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                RedirectToAction("Index");
            }
                
            return View();
        }
    }
}