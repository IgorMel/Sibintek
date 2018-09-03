using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ForSibintek.Models
{

    public class AdoRepository : IRepository
    {
        string connectionString;
        SqlConnection connection;
        public AdoRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["DbEntityConnection"]
                .ConnectionString;
        }

        public IRepository Context()
        {
            connection = new SqlConnection(connectionString);
            return this;
        }

        public void Create(File item)
        {
            string sqlExpression;
            SqlCommand command;
            connection.Open();
            if (item.FileError == null)
            {
                sqlExpression = "INSERT INTO Files(Path,CreateTime,Hashe)" +
                    "VALUES(@path,@createTime,@hashe)";
                command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@path", item.Path));
                command.Parameters.Add(new SqlParameter("@createTime", item.CreateTime));
                command.Parameters.Add(new SqlParameter("@hashe", item.Hashe));
                command.ExecuteNonQuery();
            }
            else
            {
                sqlExpression = "SELECT Id FROM FileErrors WHERE ErrorMessage=@message";
                command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@message", item.FileError.ErrorMessage));
                var reader = command.ExecuteReader();
                int id=0;
                if (reader.HasRows)
                {
                    reader.Read();
                    id = reader.GetInt32(0);
                }
                else
                {
                    reader.Close();
                    sqlExpression = "INSERT INTO FileErrors(ErrorMessage) VALUES(@message)";
                    command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@message", item.FileError.ErrorMessage));
                    command.ExecuteNonQuery();
                    sqlExpression = "SELECT Id FROM FileErrors WHERE ErrorMessage=@message";
                    command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add(new SqlParameter("@message", item.FileError.ErrorMessage));
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        id = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                reader?.Close();
                sqlExpression = "INSERT INTO Files(Path,CreateTime,FileErrorId)" +
                    "VALUES(@path,@createTime,@errorId)";
                command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@path", item.Path));
                command.Parameters.Add(new SqlParameter("@createTime", item.CreateTime));
                command.Parameters.Add(new SqlParameter("@errorId", id));
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        
    }
}
