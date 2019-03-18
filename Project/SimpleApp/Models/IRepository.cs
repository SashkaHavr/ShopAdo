using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.Models
{
    interface IRepository<T>
    {
        bool ConnectOrLoad(string dataSource);
        void DisconectOrSave();
        IEnumerable<T> GetAll();
        T Get(int id);
        int Add(T obj);
        void Update(T obj);
        void Delete(int id);
    }
}
