using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamtasticPublicWebsite.DataAccess.EntityFramework;
using System.Web;
using System.Web.Mvc;

namespace FamtasticPublicWebsite.Business
{
    public class ImageHelper : Controller
	{
		private FamtasticPublicEntities db = new FamtasticPublicEntities();

		public async Task<FileContentResult> RenderImage(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = await db.ContentPublicSites.FindAsync(id);
			imageByteArray = image.PageImage;
			contentType = image.PageImageContentType;

			return File(imageByteArray, contentType);
		}

		public FileContentResult RenderPersistedImage(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = db.ContentPublicSites.FindAsync(id);
			imageByteArray = image.Result.PageImage;
			contentType = image.Result.PageImageContentType;

			return File(imageByteArray, contentType);
		}

		public async Task<FileContentResult> RenderProfileImage(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = await db.Trainers.FindAsync(id);
			imageByteArray = image.ProfileImage;
			contentType = image.ProfileImageType;

			return File(imageByteArray, contentType);
		}

		public FileContentResult RenderPersistedProfileImage(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = db.Trainers.FindAsync(id);
			imageByteArray = image.Result.ProfileImage;
			contentType = image.Result.ProfileImageType;

			return File(imageByteArray, contentType);
		}

		public async Task<FileContentResult> RenderProfileBanner(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = await db.Trainers.FindAsync(id);
			imageByteArray = image.ProfileBanner;
			contentType = image.ProfileBannerType;

			return File(imageByteArray, contentType);
		}

		public FileContentResult RenderPersistedProfileBanner(int id)
		{
			byte[] imageByteArray = null;
			string contentType = string.Empty;

			var image = db.Trainers.FindAsync(id);
			imageByteArray = image.Result.ProfileBanner;
			contentType = image.Result.ProfileBannerType;

			return File(imageByteArray, contentType);
		}

		public String GetImage(byte[] img)
		{
			string imageBase64Data = Convert.ToBase64String(img);
			string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
			return imageDataURL;
		}
	}
}
