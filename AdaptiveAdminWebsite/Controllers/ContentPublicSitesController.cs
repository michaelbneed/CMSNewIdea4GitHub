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
    public class ContentPublicSitesController : Controller
    {
        private FamtasticPublicEntities db = new FamtasticPublicEntities();
		private ImageHelper imageHelper;

		public ContentPublicSitesController()
		{
			imageHelper = new ImageHelper();
		}

		[Authorize]
        public ActionResult Index()
        {
            return View(db.ContentPublicSites.ToList());
        }

		[Authorize]
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPublicSite contentPublicSite = db.ContentPublicSites.Find(id);
            if (contentPublicSite == null)
            {
                return HttpNotFound();
            }
            return View(contentPublicSite);
        }

		[Authorize]
		public ActionResult Create()
        {
            return View();
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Exclude = "PageImage")] ContentPublicSite contentPublicSite, HttpPostedFileBase PageImage)
        {
            if (ModelState.IsValid)
            {
				contentPublicSite.CreateDate = DateTime.Now;
				contentPublicSite.CreatedBy = User.Identity.Name;

				using (var memoryStream = new MemoryStream())
				{
					PageImage.InputStream.CopyTo(memoryStream);
					contentPublicSite.PageImage = memoryStream.ToArray();
					contentPublicSite.PageImageContentType = PageImage.ContentType;
				}

				db.ContentPublicSites.Add(contentPublicSite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contentPublicSite);
        }

		[Authorize]
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPublicSite contentPublicSite = db.ContentPublicSites.Find(id);
			
			ViewBag.CurrentImg = contentPublicSite.PageImage;

			if (contentPublicSite == null)
            {
                return HttpNotFound();
            }
            return View(contentPublicSite);
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Exclude = "PageImage")] ContentPublicSite contentPublicSite, HttpPostedFileBase PageImage)
		{ 
			if (ModelState.IsValid)
            {
				contentPublicSite.UpdateDate = DateTime.Now;
				contentPublicSite.UpdatedBy = User.Identity.Name;

				if (PageImage != null)
				{
					using (var memoryStream = new MemoryStream())
					{
						PageImage.InputStream.CopyTo(memoryStream);
						contentPublicSite.PageImage = memoryStream.ToArray();
						contentPublicSite.PageImageContentType = PageImage.ContentType;
						db.Entry(contentPublicSite).State = EntityState.Modified;
					}
				}
				else
				{
					var file = imageHelper.RenderPersistedImage(contentPublicSite.ID);
					contentPublicSite.PageImage = file.FileContents;
					contentPublicSite.PageImageContentType = file.ContentType;
					db.Entry(contentPublicSite).State = EntityState.Modified;
				}
				
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contentPublicSite);
        }

		[Authorize]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ContentPublicSite contentPublicSite = db.ContentPublicSites.Find(id);
			if (contentPublicSite == null)
			{
				return HttpNotFound();
			}
			return View(contentPublicSite);
		}

		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			ContentPublicSite contentPublicSite = db.ContentPublicSites.Find(id);
			db.ContentPublicSites.Remove(contentPublicSite);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<FileContentResult> RenderImage(int id)
		{
			return await imageHelper.RenderImage(id);
		}

		public FileContentResult RenderPersistedImage(int id)
		{
			return imageHelper.RenderPersistedImage(id);
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
