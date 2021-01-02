﻿using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser user);
    }
}
