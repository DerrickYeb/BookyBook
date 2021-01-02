using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Models.ViewModel;
using Utility;

namespace BookyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _iHost;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment iHost)
        {
            _unitOfWork = unitOfWork;
            _iHost = iHost;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM product = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(product);
            }
            product.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (product.Product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVm)
        {
            if (ModelState.IsValid)
            {
                string _webRootPath = _iHost.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(_webRootPath, @"images\product");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVm.Product.ImageUrl != null)
                    {
                       
                        var imagePath = Path.Combine(_webRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"\images\product\" + filename + extension;
                }
                else
                {
                    //Update when they do not change the image
                    if (productVm.Product.Id == 0)
                    {
                        Product objFrmDb = _unitOfWork.Product.Get(productVm.Product.Id);
                        productVm.Product.ImageUrl = objFrmDb.ImageUrl;
                    }
                }
                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(productVm);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var objData = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = objData });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFrmDb = _unitOfWork.Product.Get(id);
            if (objFrmDb == null)
            {
                return Json(new { success = false, message = "Failed to delete product" });
            }
            string _webRootPath = _iHost.WebRootPath;
            var imagePath = Path.Combine(_webRootPath, objFrmDb.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Product.Remove(id);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });
        }
        #endregion
    }
}
