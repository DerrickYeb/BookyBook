using BookyBook.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class CategoryRepository: Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var objfromDb = _context.Categories.FirstOrDefault(s => s.Id == category.Id);
            if (objfromDb != null)
            {
                objfromDb.Name = category.Name;
                _context.SaveChanges();
            }
        }
    }
}
