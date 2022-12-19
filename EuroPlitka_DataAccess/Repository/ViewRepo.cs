﻿using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Repository
{
    public class ViewRepo : Repository<Europlitkaview>, IViewRepo
    {
        public ViewRepo(EuroPlitkaDbContext db) : base(db)
        {
        }
    }
}
