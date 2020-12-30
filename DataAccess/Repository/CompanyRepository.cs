using BookyBook.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class CompanyRepository:Repository<Company>,ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Company company)
        {

            _context.Update(company);
        }
    }
}
