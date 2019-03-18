using DAL.Goods;
using DAL.Sales;
using Ninject.Modules;
using SimpleApp.Models;
using SimpleApp.Models.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.Infrastructure.Modules
{
    class SaleViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Sale>>().To<DisconnectedSaleRepositoryAdapter>();
        }
    }
}
