using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.DataAccess.EntityFramework;
using System.IO;
using System.Threading.Tasks;
using FamtasticPublicWebsite.Business;

namespace FamtasticAdminWebsite.Controllers
{
    public class ServicesController : Controller
	{
        private FamtasticPublicEntities db = new FamtasticPublicEntities();
		private ImageHelper imageHelper;

		public ServicesController()
		{
			imageHelper = new ImageHelper();
		}

		public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
				id = db.ContentPublicSites.FirstOrDefault(s => s.NavLink.ToLower().Contains("training")).ID;
			}

            ContentPublicSite contentPublicSite = db.ContentPublicSites.Find(id);

			if (contentPublicSite == null)
            {
                return HttpNotFound();
            }

			if (contentPublicSite.NavLink.ToLower().Contains("training"))
			{
				PopulateCoursesSidebar();
				PopulateTrainersSidebar();
			}

			PopulateServicesSidebar();

			var contactData = db.ContentPublicSites.FirstOrDefault(c => c.NavLink.ToLower().Contains("contact"));
			ViewBag.ContactContent = contactData.PageText;
			ViewBag.ContactTitle = contactData.Title;

			return View(contentPublicSite);
        }

		public void PopulateCoursesSidebar()
		{
			var courseList = db.Courses.Where(c => c.StartDate > DateTime.Now).ToList();
			ViewBag.CourseListForSidebar = courseList;
		}

		public void PopulateTrainersSidebar()
		{
			var trainersList = db.Trainers.ToList();
			ViewBag.TrainersListForSidebar = trainersList;
		}

		public void PopulateServicesSidebar()
		{
			var servicesList = db.ContentPublicSites.Where(c => c.IsUniqueServicePage.Equals(true)).ToList();
			ViewBag.ServicesListForSidebar = servicesList;
		}

		public ActionResult CourseDetails(int? id)
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

			var contactData = db.ContentPublicSites.FirstOrDefault(c => c.NavLink.ToLower().Contains("contact"));
			ViewBag.ContactContent = contactData.PageText;
			ViewBag.ContactTitle = contactData.Title;

			return View(course);
		}

		public ActionResult TrainerDetails(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Trainer trainer = db.Trainers.Find(id);
			if (trainer == null)
			{
				return HttpNotFound();
			}

			var contactData = db.ContentPublicSites.FirstOrDefault(c => c.NavLink.ToLower().Contains("contact"));
			ViewBag.ContactContent = contactData.PageText;
			ViewBag.ContactTitle = contactData.Title;

			return View(trainer);
		}

		public async Task<FileContentResult> RenderImage(int id)
		{
			return await imageHelper.RenderImage(id);
		}

		public async Task<FileContentResult> RenderProfileImage(int id)
		{
			return await imageHelper.RenderProfileImage(id);
		}

		public async Task<FileContentResult> RenderProfileBanner(int id)
		{
			return await imageHelper.RenderProfileBanner(id);
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
