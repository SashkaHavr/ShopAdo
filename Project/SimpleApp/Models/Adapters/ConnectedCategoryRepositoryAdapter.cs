using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Categories;

namespace SimpleApp.Models.Adapters
{
    class ConnectedCategoryRepositoryAdapter : ConnectedCategoryRepository, IRepository<Category>
    {
        public bool ConnectOrLoad(string dataSource) => Connect(dataSource);

        public void DisconectOrSave() => Disconnect();
    }
}
