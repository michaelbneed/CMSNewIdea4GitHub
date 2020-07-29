using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.Business;
using FamtasticPublicWebsite.DataAccess.EntityFramework;

namespace FamtasticAdminWebsite.Controllers
{
    public class TrainersController : Controller
    {
        private FamtasticPublicEntities db = new FamtasticPublicEntities();
		private ImageHelper imageHelper;

		public TrainersController()
		{
			imageHelper = new ImageHelper();
		}

		[Authorize]
		public ActionResult Index()
        {
            return View(db.Trainers.ToList());
        }

		[Authorize]
		public ActionResult Details(int? id)
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
            return View(trainer);
        }

		[Authorize]
		public ActionResult Create()
        {
            return View();
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ProfileImage,ProfileBanner")] Trainer trainer, HttpPostedFileBase ProfileImage, HttpPostedFileBase ProfileBanner)
		{
            if (ModelState.IsValid)
            {
				trainer.CreateDate = DateTime.Now;
				trainer.CreatedBy = User.Identity.Name;

				using (var memoryStreamProfile = new MemoryStream())
				{
					ProfileImage.InputStream.CopyTo(memoryStreamProfile);
					trainer.ProfileImage = memoryStreamProfile.ToArray();
					trainer.ProfileImageType = ProfileImage.ContentType;
				}

				using (var memoryStreamBanner = new MemoryStream())
				{
					ProfileBanner.InputStream.CopyTo(memoryStreamBanner);
					trainer.ProfileBanner = memoryStreamBanner.ToArray();
					trainer.ProfileBannerType = ProfileBanner.ContentType;
				}

				db.Trainers.Add(trainer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainer);
        }

		[Authorize]
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);

			ViewBag.CurrentImgProfile = trainer.ProfileImage;
			ViewBag.CurrentImgBanner = trainer.ProfileBanner;

			if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

		[Authorize]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ProfileImage,ProfileBanner")] Trainer trainer, HttpPostedFileBase ProfileImage, HttpPostedFileBase ProfileBanner)
		{
            if (ModelState.IsValid)
            {
				trainer.UpdateDate = DateTime.Now;
				trainer.UpdatedBy = User.Identity.Name;

				if (ProfileImage != null)
				{
					using (var memoryStreamProfile = new MemoryStream())
					{
						ProfileImage.InputStream.CopyTo(memoryStreamProfile);
						trainer.ProfileImage = memoryStreamProfile.ToArray();
						trainer.ProfileImageType = ProfileImage.ContentType;
						db.Entry(trainer).State = EntityState.Modified;
					}
				}
				else
				{
					var file = imageHelper.RenderPersistedProfileImage(trainer.ID);
					trainer.ProfileImage = file.FileContents;
					trainer.ProfileImageType = file.ContentType;
					db.Entry(trainer).State = EntityState.Modified;
				}

				if (ProfileBanner != null)
				{
					using (var memoryStreamBanner = new MemoryStream())
					{
						ProfileBanner.InputStream.CopyTo(memoryStreamBanner);
						trainer.ProfileBanner = memoryStreamBanner.ToArray();
						trainer.ProfileBannerType = ProfileBanner.ContentType;
						db.Entry(trainer).State = EntityState.Modified;
					}
				}
				else
				{
					var file = imageHelper.RenderPersistedProfileBanner(trainer.ID);
					trainer.ProfileBanner = file.FileContents;
					trainer.ProfileBannerType = file.ContentType;
					db.Entry(trainer).State = EntityState.Modified;
				}


				db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainer);
        }

		[Authorize]
		public ActionResult Delete(int? id)
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
            return View(trainer);
        }

		[Authorize]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            db.Trainers.Remove(trainer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		public async Task<FileContentResult> RenderProfileImage(int id)
		{
			return await imageHelper.RenderProfileImage(id);
		}

		public async Task<FileContentResult> RenderProfileBanner(int id)
		{
			return await imageHelper.RenderProfileBanner(id);
		}

		public FileContentResult RenderPersistedProfileBanner(int id)
		{
			return imageHelper.RenderPersistedProfileBanner(id);
		}

		public String GetImage(byte[] img)
		{
			return imageHelper.GetImage(img);
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
