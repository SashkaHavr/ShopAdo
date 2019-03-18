using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Sales
{
    public class Sale : NotifiedModelBase
    {
        int saleId;
        DateTime saleDate;
        decimal sum;

        public int SaleId { get => saleId; set { saleId = value; Notify(); } }
        public DateTime SaleDate { get => saleDate; set { saleDate = value; Notify(); } }
        public decimal Sum { get => sum; set { sum = value; Notify(); } }
        public List<SalePos> SalePositions { get; set; } = new List<SalePos>();
    }
}
