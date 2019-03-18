using DAL.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Sales
{
    public class SalePos : NotifiedModelBase
    {
        int salePosId;
        int saleId;
        int goodId;
        int countGood;

        public int SalePosId { get => salePosId; set { salePosId = value; Notify(); } }
        public int SaleId { get => saleId; set { saleId = value; Notify(); } }
        public int GoodId { get => goodId; set { goodId = value; Notify(); } }
        public Good Good { get; set; }
        public int CountGood { get => countGood; set { countGood = value; Notify(); } }
    }
}
