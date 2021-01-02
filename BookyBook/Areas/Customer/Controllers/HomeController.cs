using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.ViewModel;
using Utility;

namespace BookyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claims.Value)
                    .ToList()
                    .Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }


            return View(productList);
        }
        public IActionResult Details(int id)
        {
            var objfrmDb = _unitOfWork.Product.GetFirstOrDefault(m => m.Id == id, includeProperties: "CoverType,Category");
            ShoppingCart cartObj = new ShoppingCart
            {
                Product = objfrmDb,
                ProductId = objfrmDb.Id
            };
            return View(cartObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claims.Value;

                ShoppingCart cartFrmDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    m => m.ApplicationUserId == shoppingCart.ApplicationUserId && m.ProductId == shoppingCart.ProductId,
                    includeProperties: "Product");

                if (cartFrmDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    cartFrmDb.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(cartFrmDb);
                }
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == shoppingCart.ApplicationUserId)
                    .ToList()
                    .Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var objfrmDb = _unitOfWork.Product.GetFirstOrDefault(m => m.Id == shoppingCart.ProductId, includeProperties: "CoverType,Category");
                ShoppingCart cartObj = new ShoppingCart
                {
                    Product = objfrmDb,
                    Id = objfrmDb.Id
                };
                return View(cartObj);
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
