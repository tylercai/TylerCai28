using System;
using System.Data.SqlClient;
using TylerCai.ViewModels;

namespace TylerCai.DatabaseConnections
{
    public class DBContext
    {
        private const string CHECK_USER_EXISTS = "SELECT * FROM Users WHERE Email=@Email";
        SqlConnectionStringBuilder cb;
        SqlConnection connection;
        SqlCommand sqlCommand;
        public DBContext()
        {
            try
            {
                cb = new SqlConnectionStringBuilder();
                cb.DataSource = "tylercai.database.windows.net";
                cb.UserID = "tylercai";
                cb.Password = "Chicken1";
                cb.InitialCatalog = "TylerCaiDB";
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Connect()
        {
            connection = new SqlConnection(cb.ConnectionString);
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public bool CreateUser(UserViewModel user)
        {
            if (connection != null)
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = CHECK_USER_EXISTS;

                sqlCommand.Parameters.Add(GetEmail(user));
                SqlDataReader dr = sqlCommand.ExecuteReader();
                if (dr.HasRows)
                {
                    return false;
                }

                sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";

                sqlCommand.Parameters.Add(GetEmail(user));
                sqlCommand.Parameters.Add(GetPassword(user));
                dr.Close();
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            return false;
        }

        public bool VerifyUser(UserViewModel user)
        {
            if (connection != null)
            {
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;

                sqlCommand.CommandText = "SELECT * from Users WHERE Email=@Email AND Password=@Password";
                sqlCommand.Parameters.Add(GetEmail(user));
                sqlCommand.Parameters.Add(GetPassword(user));

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine(reader["Email"]);
                    reader.Close();
                    return true;
                }
                reader.Close();
            }
            return false;
        }

        SqlParameter GetEmail(UserViewModel user)
        {
            SqlParameter email = new SqlParameter();
            email.ParameterName = "@Email";
            email.Value = user.Email;
            return email;
        }

        SqlParameter GetPassword(UserViewModel user)
        {
            SqlParameter password = new SqlParameter();
            password.ParameterName = "@Password";
            password.Value = user.Password;
            return password;
        }
    }
}
