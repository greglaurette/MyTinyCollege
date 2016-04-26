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

namespace MyTinyCollege.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Student

        //adding sorting functionality
        public ActionResult Index(string sortOrder)
        {
            //prep sort
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FNameSortParm = string.IsNullOrEmpty(sortOrder)?"fname_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder)?"lname_desc" : "";
            //ViewBag.DateSortParm = sortOrder;

            // get data
            var students = from s in db.Students select s;
            // apply sort order
            switch(sortOrder)
            {
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            return View(students.ToList());

            //return View(db.Students.ToList());
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
