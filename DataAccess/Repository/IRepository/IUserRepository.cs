using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        void Update(ApplicationUser user);
    }
}
