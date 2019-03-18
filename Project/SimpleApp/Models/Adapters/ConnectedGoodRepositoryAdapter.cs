using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Goods;

namespace SimpleApp.Models.Adapters
{
    class ConnectedGoodRepositoryAdapter : ConnectedGoodRepository, IRepository<Good>
    {
        public bool ConnectOrLoad(string dataSource) => Connect(dataSource);

        public void DisconectOrSave() => Disconnect();
    }
}
