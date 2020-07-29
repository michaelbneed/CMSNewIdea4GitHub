using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.DataAccess.EntityFramework;

namespace FamtasticAdminWebsite.Controllers
{
    public class CoursesController : Controller
    {
        private FamtasticPublicEntities db = new FamtasticPublicEntities();

		[Authorize]
		public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

		[Authorize]
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

		[Authorize]
		public ActionResult Create()
        {
            return View();
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CourseName,CourseDescription,Certification,Instructor,TrainerId,StartDate,EndDate,CreateDate,UpdateDate")] Course course)
        {
            if (ModelState.IsValid)
            {
				course.CreateDate = DateTime.Now;
				
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

		[Authorize]
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
			ViewBag.TrainerId = new SelectList(db.Trainers, "ID", "CourseName", course.TrainerID);
			return View(course);
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CourseName,CourseDescription,Certification,Instructor,TrainerId,StartDate,EndDate,CreateDate,UpdateDate")] Course course)
        {
            if (ModelState.IsValid)
            {
				course.UpdateDate = DateTime.Now;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

		[Authorize]
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

		[Authorize]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
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
