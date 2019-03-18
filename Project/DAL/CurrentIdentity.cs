using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    static class CurrentIdentity
    {
        static SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder() { InitialCatalog = "ShopAdo", IntegratedSecurity = true };
        static SqlCommand idCur = new SqlCommand("select IDENT_CURRENT(@table)");
        public static int Get(string dataSource, string tableName)
        {
            csBuilder.DataSource = dataSource;
            using (SqlConnection sqlConnection = new SqlConnection(csBuilder.ConnectionString))
            {
                idCur.Parameters.Add(new SqlParameter("@table", tableName));
                idCur.Connection = sqlConnection;
                sqlConnection.Open();
                int res = (int)(decimal)idCur.ExecuteScalar();
                idCur.Parameters.Clear();
                return res;
            }
        }
    }
}
