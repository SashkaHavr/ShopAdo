using DAL.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.Models.Adapters
{
    class ConnectedManufacturerRepositoryAdapter : ConnectedManufacturerRepository, IRepository<Manufacturer>
    {
        public bool ConnectOrLoad(string dataSource) => Connect(dataSource);

        public void DisconectOrSave() => Disconnect();
    }
}
