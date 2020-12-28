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
            var objData = _context.Companies.FirstOrDefault(o => o.Id == company.Id);
            if (objData != null)
            {
                objData.Name = company.Name;
                objData.StreetAddress = company.StreetAddress;
                objData.City = company.City;
                objData.State = company.State;
                objData.PostalCode = company.PostalCode;
                objData.PhoneNumber = company.PhoneNumber;
                objData.IsAuthorizedCompany = company.IsAuthorizedCompany;
            }
        }
    }
}
