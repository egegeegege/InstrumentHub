﻿using System.Diagnostics;
using Instrument.Business.Abstract;
using Instrument.WebUI.Models;
using InstrumentHub.Entites;
using Microsoft.AspNetCore.Mvc;

namespace InstrumentHub.WebUI.Controllers
{
	public class SalesController : Controller
	{
		IEProductServices _eproductServices;
		ICommentServices _commentServices;

		public SalesController(IEProductServices eProductServices, ICommentServices commentServices)
		{
			_eproductServices = eProductServices;
			_commentServices = commentServices;
		}

		[Route("eproducts/{division?}")]
		public IActionResult Liste(string division, int page = 1)
		{
			if (string.IsNullOrEmpty(division))
			{
				return NotFound(); // division bilgisi boş ise 404 döndür
			}

			const int pageSize = 15; // sayfa açıldığında göreceğimiz ürün sayısı bukadardır

			var eproducts = new EProductListModel()
			{
				PageInformation = new PageInformation()
				{
					ToatalItems = _eproductServices.GetCountByDivision(division), 
					ItemsPerPage = pageSize,
					CurrentPage = page,
					CurrentDivision = division
				},
				EProducts = _eproductServices.GetEProductByDivision(division, page, pageSize)
			};

			return View(eproducts);
		}



		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			EProduct eproduct = _eproductServices.GetEProductDetail(id.Value);
			if (eproduct == null)
			{
				return NotFound();//id yoksa 404 döndürmek için
			}

			double avarageRaiting = _commentServices.GetAverageRating(id.Value);
			ViewBag.AvarageRaiting = avarageRaiting;

			// Aynı division'a ait diğer ürünleri getirirmesi için bir where sorgusu ekledim
			var relatedProducts = _eproductServices.GetEProductByDivision(eproduct.ProductDivisions.FirstOrDefault()?.Division.CategoryName, page: 1, pageSize: 4)
								.Where(p => p.Id != id.Value)
								.ToList();

			return View(new EProductDetailModel()
			{
				EProduct = eproduct,
				Divisions = eproduct.ProductDivisions.Select(i => i.Division).ToList(),
				Comments = eproduct.Comments,
				RelatedProducts = relatedProducts 
			});
		}

	}
}
