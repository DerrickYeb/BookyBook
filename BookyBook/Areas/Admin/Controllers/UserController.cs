using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DataAccess.Repository;
using Models;
using Microsoft.AspNetCore.Mvc;
using BookyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Utility;

namespace BookyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        #region Get API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.ApplicationUsers.Include(m => m.Company).ToList();
            var userRole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(m => m.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new Company
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFrmdb = _context.ApplicationUsers.FirstOrDefault(m => m.Id == id);
            if (objFrmdb == null)
            {
                return Json(new { success = false, message = "Error while Locking or Unlock" });
            }
            if (objFrmdb.LockoutEnd !=null && objFrmdb.LockoutEnd > DateTime.Now)
            {
                objFrmdb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFrmdb.LockoutEnd = DateTime.Now.AddYears(10);
            }
            _context.SaveChanges();
            return Json(new { success = true, message = "Operation successful" });
        }
       
        #endregion
    }
}
