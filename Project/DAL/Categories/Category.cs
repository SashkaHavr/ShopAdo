using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Categories
{
    public class Category : NotifiedModelBase
    {
        int categoryId;
        string categoryName = string.Empty;

        public int CategoryId { get => categoryId; set { categoryId = value; Notify(); } }
        public string CategoryName { get => categoryName; set { categoryName = value; Notify(); } }
    }
}
