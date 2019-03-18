using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Goods;

namespace DAL.Sales
{
    public class DisconnectedSaleRepository
    {
        int saleId;
        int salePosId;
        bool loaded;
        SqlDataAdapter saleDataAdapter;
        SqlDataAdapter salePosDataAdapter;
        DataTable sales = new DataTable();
        DataTable salePoss = new DataTable();
        List<DataRow> deletedRows = new List<DataRow>();
        ConnectedGoodRepository goodRepository = new ConnectedGoodRepository();
        SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder()
        {
            InitialCatalog = "ShopAdo",
            IntegratedSecurity = true,
            ConnectTimeout = 1

        };

        IList <SalePos> GetSalePositions(IEnumerable<Good> goods)
        {
             return (from row in salePoss.AsEnumerable().Except(deletedRows)
                     select new SalePos()
                     {
                         SalePosId = row.Field<int>("SalePosId"),
                         SaleId = row.Field<int>("SaleId"),
                         GoodId = row.Field<int>("GoodId"),
                         CountGood = row.Field<int>("CountGood"),
                         Good = goods.Where(g => g.GoodId == row.Field<int>("GoodId")).SingleOrDefault()
                     }).ToList();
        }   

        public bool Load(string dataSource)
        {
            try
            {
                loaded = true;
                deletedRows.Clear();
                sales.Clear();
                salePoss.Clear();
                saleId = CurrentIdentity.Get(dataSource, "Sale");
                salePosId = CurrentIdentity.Get(dataSource, "SalePos");
                csBuilder.DataSource = dataSource;
                goodRepository.Connect(dataSource);
                saleDataAdapter = new SqlDataAdapter(@"select * from Sale", csBuilder.ConnectionString);
                salePosDataAdapter = new SqlDataAdapter(@"select * from SalePos", csBuilder.ConnectionString);
                SqlCommandBuilder builder = new SqlCommandBuilder(saleDataAdapter);
                builder = new SqlCommandBuilder(salePosDataAdapter);
                saleDataAdapter.Fill(sales);
                salePosDataAdapter.Fill(salePoss);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Save()
        {
            if (loaded)
            {
                try
                {
                    saleDataAdapter.Update(sales);
                    salePosDataAdapter.Update(salePoss);
                }
                catch
                {
                    salePosDataAdapter.Update(salePoss);
                    saleDataAdapter.Update(sales);
                }
                finally
                {
                    deletedRows.Clear();
                }
            }
        }

        public IEnumerable<Sale> GetAll()
        {
            var salePositions = GetSalePositions(goodRepository.GetAll());
            return (from row in sales.AsEnumerable().Except(deletedRows)
                    select new Sale()
                    {
                        SaleId = row.Field<int>("SaleId"),
                        SaleDate = row.Field<DateTime>("DateSale"),
                        Sum = row.Field<decimal>("Sum"),
                        SalePositions = salePositions.Where(s => s.SaleId == row.Field<int>("SaleId")).ToList()
                    }).ToList();
        }

        public Sale Get(int id)
        {
            var salePositions = GetSalePositions(goodRepository.GetAll());
            return GetAll().Where(s => s.SaleId == id).SingleOrDefault();
        }

        void SetSale(DataRow row, Sale sale)
        {
            row.SetField("SaleId", sale.SaleId);
            row.SetField("DateSale", sale.SaleDate);
            row.SetField("Sum", sale.Sum);
        }

        void SetSalePos(DataRow row, SalePos pos)
        {
            row.SetField("SalePosId", pos.SalePosId);
            row.SetField("SaleId", pos.SaleId);
            row.SetField("CountGood", pos.CountGood);
            row.SetField("GoodId", pos.GoodId);
        }

        public void Add(Sale sale)
        {
            DataRow row = null;
            sale.SaleId = ++saleId;
            foreach (var pos in sale.SalePositions)
            {
                row = salePoss.NewRow();
                pos.SaleId = sale.SaleId;
                pos.SalePosId = ++salePosId;
                SetSalePos(row, pos);
                salePoss.Rows.Add(row);
            }
            row = sales.NewRow();
            SetSale(row, sale);
            sales.Rows.Add(row);
            foreach (var pos in sale.SalePositions)
                pos.SaleId = sale.SaleId;
        }

        List<DataRow> GetSalePosRowsFromSale(int saleId)
        {
            return (from sRow in salePoss.AsEnumerable().Except(deletedRows)
             where sRow.Field<int>("SaleId") == saleId
             select sRow).ToList();
        }

        public SalePos GetPos(DataRow row)
        {
            return new SalePos()
            {
                SalePosId = row.Field<int>("SalePosId"),
                SaleId = row.Field<int>("SaleId"),
                GoodId = row.Field<int>("GoodId"),
                CountGood = row.Field<int>("CountGood"),
                Good = goodRepository.GetAll().Where(g => g.GoodId == row.Field<int>("GoodId")).SingleOrDefault()
            };
        }

        public void Update(Sale sale)
        {
            var rows = GetSalePosRowsFromSale(sale.SaleId);
            if (rows.Count > sale.SalePositions.Count)
            {
                foreach (var row in rows)
                    if (!sale.SalePositions.Select(s => s.SalePosId).Contains(row.Field<int>("SalePosId")))
                    {
                        deletedRows.Add(row);
                        row.Delete();
                    }
            }
            else if(rows.Count < sale.SalePositions.Count)
            {
                foreach (var pos in sale.SalePositions)
                    if (!rows.Select(r => r.Field<int>("SalePosId")).Contains(pos.SalePosId))
                    {
                        var row = salePoss.NewRow();
                        pos.SaleId = sale.SaleId;
                        pos.SalePosId = ++salePosId;
                        SetSalePos(row, pos);
                        salePoss.Rows.Add(row);
                    }
            }
            foreach (var row in rows)
            {
                var pos = sale.SalePositions.Where(s => row.RowState != DataRowState.Deleted && s.SalePosId == row.Field<int>("SalePosId")).SingleOrDefault();
                    if(pos != null)
                        SetSalePos(row, pos);
            }
            SetSale(sales.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted && s.Field<int>("SaleId") == sale.SaleId).SingleOrDefault(), sale);
        }

        public void Delete(int id)
        {
            var sale = Get(id);
            foreach (var row in GetSalePosRowsFromSale(id))
            {
                deletedRows.Add(row);
                row.Delete();
            }
            var srow = sales.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted && s.Field<int>("SaleId") == id).SingleOrDefault();
            deletedRows.Add(srow);
            srow.Delete();
        }
    }
}
