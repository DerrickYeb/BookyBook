using BookyBook.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class UserRepository:Repository<ApplicationUser>,IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(ApplicationUser user)
        {
            _context.Update(user);
        }
    }
}
