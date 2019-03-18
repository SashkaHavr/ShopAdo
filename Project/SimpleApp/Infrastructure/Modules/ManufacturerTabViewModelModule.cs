using DAL.Categories;
using DAL.Goods;
using DAL.Manufacturers;
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
    class ManufacturerTabViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Manufacturer>>().To<ConnectedManufacturerRepositoryAdapter>();
        }
    }
}
