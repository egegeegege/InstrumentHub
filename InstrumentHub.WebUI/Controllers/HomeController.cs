using System.Diagnostics;
using Instrument.Business.Abstract;
using Instrument.WebUI.Models;
using InstrumentHub.Entites;
using Microsoft.AspNetCore.Mvc;

namespace InstrumentHub.WebUI.Controllers
{
	public class HomeController : Controller
	{
		private IEProductServices _productServices;

		public HomeController(IEProductServices productServices)
		{
			_productServices = productServices;
		}

		public IActionResult Index()
		{
			var products = _productServices.GetAll();


			if (products == null || !products.Any())
			{
				products = new List<EProduct>();
			}

			return View(new EProductListModel()
			{
				EProducts = products,
			});
		}
		public IActionResult FilterByPrice(decimal minPrice, decimal maxPrice)
		{
			if (minPrice > maxPrice)
			{
				TempData["Error"] = "Minimum fiyat, maksimum fiyattan b�y�k olamaz.";
				return RedirectToAction("Index");
			}

			var productsInRange = _productServices.GetProductsByPriceRange(minPrice, maxPrice);

			if (productsInRange == null || productsInRange.Count == 0)
			{
				TempData["Error"] = "Bu fiyat aral���nda �r�n bulunamad�.";
			}

			var model = new EProductListModel
			{
				EProducts = productsInRange
			};

			return View("Index", model);  // **Home/Index'e y�nlendiriyoruz**
		}
	}
}
