using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Manufacturers
{
    public class ConnectedManufacturerRepository : ConnectedRepositoryBase<Manufacturer>
    {
        SqlCommand selectAllCommand = new SqlCommand("select * from Manufacturer");
        SqlCommand selectOneCommand = new SqlCommand(@"select * from Manufacturer
                                                        where ManufacturerId = @id");
        SqlCommand insertCommand = new SqlCommand(@"insert into Manufacturer(ManufacturerName)
                                                   values(@manufacturerName)");
        SqlCommand updateCommand = new SqlCommand(@"update Manufacturer
                                                    set ManufacturerName = @manufacturerName
                                                    where ManufacturerId = @id");
        SqlCommand deleteCommand = new SqlCommand(@"delete from Manufacturer
                                                  where ManufacturerId = @id");
        SqlCommand selectLastId = new SqlCommand(@"select top 1 ManufacturerId from Manufacturer
                                                   order by ManufacturerId desc");

        protected override SqlCommand SelectAllCommand => selectAllCommand;

        protected override SqlCommand SelectOneCommand => selectOneCommand;

        protected override SqlCommand InsertCommand => insertCommand;

        protected override SqlCommand SelectLastId => selectLastId;

        protected override SqlCommand UpdateCommand => updateCommand;

        protected override SqlCommand DeleteCommand => deleteCommand;

        protected override void AddParametrs(Manufacturer obj, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("@id", obj.ManufacturerId);
            sqlCommand.Parameters.AddWithValue("@manufacturerName", obj.ManufacturerName);
        }
    }
}
