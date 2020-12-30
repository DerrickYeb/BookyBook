using BookyBook.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public CoverTypeRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(CoverType coverType)
        {
            var objFrmDb = _context.CoverTypes.FirstOrDefault(s => s.Id == coverType.Id);
            if (objFrmDb != null)
            {
                objFrmDb.Name = coverType.Name;
                _context.SaveChanges();
            }
        }
    }
}
