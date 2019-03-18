using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manufacturers
{
    public class Manufacturer : NotifiedModelBase
    {
        int manufacturerId;
        string manufacturerName = string.Empty;

        public int ManufacturerId { get => manufacturerId; set { manufacturerId = value; Notify(); } }
        public string ManufacturerName { get => manufacturerName; set { manufacturerName = value; Notify(); } }
    }
}
