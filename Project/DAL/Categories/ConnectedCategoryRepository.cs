using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Categories
{
    public class ConnectedCategoryRepository : ConnectedRepositoryBase<Category>
    {
        SqlCommand selectAllCommand = new SqlCommand("select * from Category");
        SqlCommand selectOneCommand = new SqlCommand(@"select * fron Category
                                                    where CategoryId = @id");
        SqlCommand insertCommand = new SqlCommand(@"insert into Category(CategoryName)
                                                    values(@categoryName)");
        SqlCommand selectLastId = new SqlCommand(@"select top 1 CategoryId from Category
                                                   order by CategoryId desc");
        SqlCommand updateCommand = new SqlCommand(@"update Category 
                                                    set CategoryName = @categoryName
                                                    where CategoryId = @id");
        SqlCommand deleteCommand = new SqlCommand(@"delete from Category
                                                    where CategoryId = @id");

        protected override SqlCommand SelectAllCommand => selectAllCommand;

        protected override SqlCommand SelectOneCommand => selectOneCommand;

        protected override SqlCommand InsertCommand => insertCommand;

        protected override SqlCommand SelectLastId => selectLastId;

        protected override SqlCommand UpdateCommand => updateCommand;

        protected override SqlCommand DeleteCommand => deleteCommand;

        protected override void AddParametrs(Category obj, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.AddWithValue("@id", obj.CategoryId);
            sqlCommand.Parameters.AddWithValue("@categoryName", obj.CategoryName);
        }
    }
}
