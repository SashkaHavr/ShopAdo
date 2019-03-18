using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DAL.Goods;
using SimpleApp.Infrastructure;
using SimpleApp.Models;
using SimpleApp.Views;

namespace SimpleApp.ViewModels
{
    class GoodTabViewModel : TabViewModelBase<Good>
    {
        public GoodTabViewModel(IRepository<Good> repository) : base(repository) { }

        protected override void SetId(Good obj, int id) => obj.GoodId = id;

        protected override void ReUpdateAllProperties(Good obj) => obj.UpdateProperties(repository.Get(obj.GoodId));

        protected override int GetId(Good obj) => obj.GoodId;

        protected override bool SearchPredicate(Good obj) => obj.GoodName.ToLower().Contains(SearchString.ToLower());
    }
}
