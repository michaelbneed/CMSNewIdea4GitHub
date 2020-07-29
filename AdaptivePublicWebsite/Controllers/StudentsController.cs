using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.DataAccess.EntityFramework;
using FamtasticPublicWebsite.Business;
using System.Diagnostics;

namespace FamtasticAdminWebsite.Controllers
{
    public class StudentsController : Controller
    {
        private FamtasticPublicEntities db = new FamtasticPublicEntities();

		public void PopulateCourses()
		{
			var courseList = db.Courses.Where(c => c.StartDate > DateTime.Now);
			ViewBag.CourseID = new SelectList(db.Courses, "ID", "CourseName");
		}

		[Route("PopulateCoursesList")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult PopulateCoursesList()
		{
			SelectList courseDropdown = null;
			var courseList = db.Courses.Where(c => c.StartDate > DateTime.Now).ToList();
			courseDropdown = new SelectList((IEnumerable)courseList, "ID", "CourseName".Trim());

			ViewData["CourseId"] = courseDropdown;
			return Json(courseDropdown);
		}

		[Route("Create")]
		[AcceptVerbs(HttpVerbs.Post)]
		[HttpPost, ValidateInput(false)]
		public JsonResult Create([Bind(Exclude = "CreateDate")] Student student)
        {
            if (ModelState.IsValid)
            {
				student.CreateDate = DateTime.Now;
                db.Students.Add(student);
                db.SaveChanges();

				//Send an email on training form submit
				try
				{
					MailAddress mailAddress = new MailAddress(student.Email);
					MailMessage message = new MailMessage();
					message.IsBodyHtml = true;
					message.Subject = "Training Request for Course ID: " + student.CourseID;
					message.Body = student.Comments;
					EmailHelper.SendUserMessage(mailAddress, message, "sales");
				}
				catch (Exception ex)
				{
					using (EventLog eventLog = new EventLog("Application"))
					{
						eventLog.Source = "Public Website";
						eventLog.WriteEntry("Error sending email in Students " + ex.InnerException + " Stack Trace: " + ex.StackTrace, EventLogEntryType.Error);
					}
				}

				var jsonResult = Json(student);
				return jsonResult;
			}
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "CourseName", student.CourseID);
			return Json(null);
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
