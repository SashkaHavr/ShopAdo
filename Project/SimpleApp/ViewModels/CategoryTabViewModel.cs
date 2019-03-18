using DAL.Categories;
using SimpleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleApp.ViewModels
{
    class CategoryTabViewModel : TabViewModelBase<Category>
    {
        public CategoryTabViewModel(IRepository<Category> repository) : base(repository) { }

        protected override int GetId(Category obj) => obj.CategoryId;

        protected override void ReUpdateAllProperties(Category obj) { }

        protected override bool SearchPredicate(Category obj) => obj.CategoryName.ToLower().Contains(SearchString);

        protected override void SetId(Category obj, int id) => obj.CategoryId = id;
    }
}
