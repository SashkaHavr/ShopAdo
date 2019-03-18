using DAL.Goods;
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
    class GoodTabViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Good>>().To<ConnectedGoodRepositoryAdapter>();

        }
    }
}
