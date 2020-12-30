using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
            {
                return View(company);
            }
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
        #region Get API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var objall = _unitOfWork.Company.GetAll();
            return Json(new { data = objall });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var objFrmDb = _unitOfWork.Company.Get(Id);
            if (objFrmDb == null)
            {
                return Json(new { success = false, message = "Error while trying to delete" });
            }
            _unitOfWork.Company.Remove(objFrmDb);
            _unitOfWork.Save();
            return Json(new { succes = true, message = "Deleted successfully" });
        }
        #endregion
    }
}
