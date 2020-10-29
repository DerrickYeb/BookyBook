using BookyBook.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var dataObj = _context.Products.FirstOrDefault(s => s.Id == product.Id);
            if (dataObj != null)
            {
                if (product.ImageUrl != null)
                {
                    dataObj.ImageUrl = product.ImageUrl;
                }
                dataObj.Title = product.Title;
                dataObj.Author = product.Author;
                dataObj.ISBN = product.ISBN;
                dataObj.Price = product.Price;
                dataObj.Description = product.Description;
                dataObj.ListPrice = product.ListPrice;
                dataObj.Price100 = product.Price100;
                dataObj.Price50 = product.Price50;
                dataObj.CoverTypeId = product.CoverTypeId;
                dataObj.CategoryId = product.CategoryId;
            }
        }
    }
}
