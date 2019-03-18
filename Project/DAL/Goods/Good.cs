using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Goods
{
    public class Good : NotifiedModelBase
    {
        int goodId;
        string goodName = string.Empty;
        int manufacturerId = 1;
        int categoryId = 1;
        decimal price;
        decimal goodCount;
        string manufacturerName;
        string categoryName;
        Type goodType = typeof(Good);

        public int GoodId { get => goodId; set { goodId = value; Notify(); } }
        public string GoodName { get => goodName; set { goodName = value; Notify(); } }
        public int ManufacturerId { get => manufacturerId; set { manufacturerId = value; Notify(); } }
        public int CategoryId { get => categoryId; set { categoryId = value; Notify(); } }
        public decimal Price { get => price; set { price = value; Notify(); } }
        public decimal GoodCount { get => goodCount; set { goodCount = value; Notify(); } }
        public string ManufacturerName { get => manufacturerName; set { manufacturerName = value; Notify(); } }
        public string CategoryName { get => categoryName; set { categoryName = value; Notify(); } }

        public void UpdateProperties(Good good)
        {
            foreach (var prop in goodType.GetProperties())
                prop.SetValue(this, prop.GetValue(good));
        }
    }
}
