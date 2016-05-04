using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using PagedList;
using MyTinyCollege.ViewModels;

namespace MyTinyCollege.Controllers
{
    [Authorize(Roles = "admin")]
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Student

        //adding sorting functionality,  filtering, paging
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //prep sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FNameSortParm = string.IsNullOrEmpty(sortOrder)?"fname_desc" : "fname";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder)?"lname_desc" : "";
            ViewBag.Email = string.IsNullOrEmpty(sortOrder) ? "email_desc" : "email";
            ViewBag.EnrollmentDate = string.IsNullOrEmpty(sortOrder) ? "enroll_desc" : "enroll";


            //ViewBag.DateSortParm = sortOrder;
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            // get data
            var students = from s in db.Students select s;

            // check for filter
            if (!string.IsNullOrEmpty(searchString))
            {
                //Apply filter on first and last name
                students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }

            // apply sort order
            switch(sortOrder)
            {
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "fname":
                    students = students.OrderBy(s => s.FirstName);
                    break;
                case "email_desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                case "email":
                    students = students.OrderBy(s => s.Email);
                    break;
                case "enroll_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "enroll":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            //return View(students.ToList());

            //return View(db.Students.ToList());
            //setup pager
            int pageSize = 3; //3 records per page
            int pageNumber = (page ?? 1);

            /* The two question marks represent the null- coalescing operator.
             * the null coalescing operator defines a default value for a nullable type;
             * the expression (page ?? 1) means return the value of page if it has a value,
             * or return 1 if page is null
             */
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        //end of add

        //public ActionResult Index()
        //{
        //    return View(db.Students.ToList());
        //}
        

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,Email,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.People.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Unable to save. Try again");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var studentToUpdate = db.Students.Find(id);
            if(TryUpdateModel(studentToUpdate,"", new string[] {"LastName", "FirstName", "EnrollmentDate", "Email"}))
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes");
                }
            
            return View(studentToUpdate);
        }

        //public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,Email,EnrollmentDate")] Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(student).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(student);
        //}

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? savedChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (savedChangesError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Delete failed. Please try again";

            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            { 
            Student student = db.Students.Find(id);
            db.People.Remove(student);
            db.SaveChanges();            
            }
            catch(Exception)
            {
                return RedirectToAction("Delete", new { id = id, savedChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Stats()
        {
            IQueryable<ViewModels.EnrollmentDateGroup> data = from student in db.Students
                                                              group student by student.EnrollmentDate into dateGroup
                                                              select new ViewModels.EnrollmentDateGroup()
                                                              {
                                                                  EnrollmentDate = dateGroup.Key,
                                                                  StudentCount = dateGroup.Count()
                                                              };

            //The LINQ statement above groups the student entities by enrollment date, 
            //calculating the number of enities in each group, and storing the results
            //in a colleection of EnrollmentDateGroup view model objects

            return View(data.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
