using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    abstract public class ConnectedRepositoryBase<T> where T: new()
    {
        abstract protected SqlCommand SelectAllCommand { get; }
        abstract protected SqlCommand SelectOneCommand { get; }
        abstract protected SqlCommand InsertCommand { get; }
        abstract protected SqlCommand SelectLastId { get; }
        abstract protected SqlCommand UpdateCommand { get; }
        abstract protected SqlCommand DeleteCommand { get; }
        protected SqlConnection connection;
        protected SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder()
        {
            InitialCatalog = "ShopAdo",
            IntegratedSecurity = true,
            ConnectTimeout = 1
        };

        public bool Connect(string dataSource)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                csBuilder.DataSource = dataSource;
                connection = new SqlConnection(csBuilder.ConnectionString);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if(connection != null && connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        protected void ConnectionCheck()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                throw new Exception("Connection is closed");
        }

        Type objectType = typeof(T);

        T GetObject(SqlDataReader reader)
        {
            T obj = new T();
            for (int i = 0; i < reader.FieldCount; i++)
                objectType.GetProperty(reader.GetName(i)).SetValue(obj, reader.GetValue(i));
            return obj;
        }

        public IEnumerable<T> GetAll()
        {
            ConnectionCheck();
            SelectAllCommand.Connection = connection;
            using (SqlDataReader reader = SelectAllCommand.ExecuteReader())
            {
                List<T> objects = new List<T>();
                while (reader.Read())
                    objects.Add(GetObject(reader));
                return objects;
            }
        }

        public T Get(int id)
        {
            ConnectionCheck();
            SelectOneCommand.Connection = connection;
            SelectOneCommand.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = SelectOneCommand.ExecuteReader())
            {
                SelectOneCommand.Parameters.Clear();
                if (reader.HasRows)
                {
                    reader.Read();
                    return GetObject(reader);
                }
                else
                    throw new IndexOutOfRangeException();
            }
        }

        abstract protected void AddParametrs(T obj, SqlCommand sqlCommand);

        void AddUpdateTemplate(T obj, SqlCommand sqlCommand)
        {
            ConnectionCheck();
            SqlTransaction transaction = connection.BeginTransaction();
            sqlCommand.Connection = connection;
            sqlCommand.Transaction = transaction;
            AddParametrs(obj, sqlCommand);
            try
            {
                sqlCommand.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                sqlCommand.Parameters.Clear();
            }
        }

        public int Add(T obj)
        {
            AddUpdateTemplate(obj, InsertCommand);
            SelectLastId.Connection = connection;
            using (SqlDataReader reader = SelectLastId.ExecuteReader())
            {
                SelectLastId.Parameters.Clear();
                reader.Read();
                return (int)reader.GetValue(0);
            }
        }

        public void Update(T obj) => AddUpdateTemplate(obj, UpdateCommand);

        public void Delete(int id)
        {
            ConnectionCheck();
            SqlTransaction transaction = connection.BeginTransaction();
            DeleteCommand.Connection = connection;
            DeleteCommand.Transaction = transaction;
            DeleteCommand.Parameters.AddWithValue("@id", id);
            try
            {
                DeleteCommand.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
            finally
            {
                DeleteCommand.Parameters.Clear();
            }
        }
    }
}
