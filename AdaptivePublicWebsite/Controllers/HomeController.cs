using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FamtasticPublicWebsite.DataAccess.EntityFramework;
using FamtasticPublicWebsite.Business;
using System.Threading.Tasks;

namespace FamtasticPublicWebsite.Controllers
{
	public class HomeController : Controller
	{
		private FamtasticPublicEntities db = new FamtasticPublicEntities();
		public List<ContentPublicSite> PageSections;

		private ImageHelper imageHelper;

		public HomeController()
		{
			imageHelper = new ImageHelper();
		}

		public ActionResult Index()
		{
			PageSections = db.ContentPublicSites.ToList();

			foreach (var item in PageSections)
			{
				if (item.NavLink != null && item.NavLink == "Section1")
				{
					if (item.PageImage != null)
					{
						ViewBag.Section1Pic = GetImage(item.PageImage);
					}
					ViewBag.Section1Content = item.PageText;
					ViewBag.Section1Title = item.Title;
				}

				if (item.NavLink != null && item.PageText != null && item.NavLink == "Section2")
				{
					if (item.PageImage != null)
					{
						ViewBag.Section2Pic = GetImage(item.PageImage);
					}
					ViewBag.Section2Content = item.PageText;
					ViewBag.Section2Title = item.Title;
				}


				if (item.NavLink != null && item.PageText != null && item.NavLink == "Section3")
				{
					if (item.PageImage != null)
					{
						ViewBag.Section3Pic = GetImage(item.PageImage);
					}
					ViewBag.Section3Content = item.PageText;
					ViewBag.Section3Title = item.Title;
				}

				//if (item.NavLink != null && item.PageText != null && item.NavLink == "Section4")
				//{
				//	if (item.PageImage != null)
				//	{
				//		ViewBag.Section4Pic = GetImage(item.PageImage);
				//	}
				//	ViewBag.Section4Content = item.PageText;
				//	ViewBag.Section4Title = item.Title;
				//}
			}

			return View(PageSections);
		}

		public async Task<FileContentResult> RenderImage(int id)
		{
			return await imageHelper.RenderImage(id);
		}

		public String GetImage(byte[] img)
		{
			string imageBase64Data = Convert.ToBase64String(img);
			string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
			return imageDataURL;
		}
	}
}