using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleApp.Models;
using DAL.Manufacturers;

namespace SimpleApp.ViewModels
{
    class ManufacturerTabViewModel : TabViewModelBase<Manufacturer>
    {
        public ManufacturerTabViewModel(IRepository<Manufacturer> repository) : base(repository) { }

        protected override int GetId(Manufacturer obj) => obj.ManufacturerId;

        protected override void ReUpdateAllProperties(Manufacturer obj) { }

        protected override bool SearchPredicate(Manufacturer obj) => obj.ManufacturerName.ToLower().Contains(SearchString.ToLower());

        protected override void SetId(Manufacturer obj, int id) => obj.ManufacturerId = id;
    }
}
