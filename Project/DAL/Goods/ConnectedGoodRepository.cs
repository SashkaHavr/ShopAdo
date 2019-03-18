using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL.Goods
{
    public class ConnectedGoodRepository : ConnectedRepositoryBase<Good>
    {
        const string joinedSelect = @"select GoodId, GoodName, Good.ManufacturerId, ManufacturerName, Good.CategoryId, CategoryName, Price, GoodCount from Good
			                                      join Manufacturer on Good.ManufacturerId = Manufacturer.ManufacturerId
			                                      join Category on Good.CategoryId = Category.CategoryId";
        SqlCommand selectAllCommand = new SqlCommand(joinedSelect);
        SqlCommand selectOneCommand = new SqlCommand(string.Concat(joinedSelect, " where GoodId = @id"));
        SqlCommand insertCommand = new SqlCommand(@"insert into Good(GoodName, ManufacturerId, CategoryId, Price, GoodCount)
                                             values (@goodName, @manufacturerId, @categoryId, @price, @goodCount);");
        SqlCommand updateCommand = new SqlCommand(@"update Good
                                            set GoodName = @goodName, ManufacturerId = @manufacturerId, CategoryId = @categoryId,
                                            Price = @price, GoodCount = @GoodCount
                                            where GoodId = @id");
        SqlCommand deleteCommand = new SqlCommand(@"delete from Good
                                                    where GoodId = @id");
        SqlCommand selectLastId = new SqlCommand(@"select top 1 GoodId from Good
                                                order by GoodId desc;");

        protected override SqlCommand SelectAllCommand => selectAllCommand;

        protected override SqlCommand SelectOneCommand => selectOneCommand;

        protected override SqlCommand InsertCommand => insertCommand;

        protected override SqlCommand SelectLastId => selectLastId;

        protected override SqlCommand UpdateCommand => updateCommand;

        protected override SqlCommand DeleteCommand => deleteCommand;

        protected override void AddParametrs(Good obj, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("@id", obj.GoodId);
            sqlCommand.Parameters.AddWithValue("@goodName", obj.GoodName);
            sqlCommand.Parameters.AddWithValue("@manufacturerId", obj.ManufacturerId);
            sqlCommand.Parameters.AddWithValue("@categoryId", obj.CategoryId);
            sqlCommand.Parameters.AddWithValue("@price", obj.Price);
            sqlCommand.Parameters.AddWithValue("@goodCount", obj.GoodCount);
        }
    }
}
