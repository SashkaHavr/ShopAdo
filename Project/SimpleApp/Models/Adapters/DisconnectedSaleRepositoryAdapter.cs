using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Sales;

namespace SimpleApp.Models.Adapters
{
    class DisconnectedSaleRepositoryAdapter : DisconnectedSaleRepository, IRepository<Sale>
    {
        public bool ConnectOrLoad(string dataSource) => Load(dataSource);

        public void DisconectOrSave() => Save();

        int IRepository<Sale>.Add(Sale obj)
        {
            Add(obj);
            return 0;
        }
    }
}
