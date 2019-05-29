using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data.Sql;
using TylerCai.Models;
using TylerCai.ViewModels;

namespace TylerCai.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            Connect();
            sqlCommand.CommandText = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";

            sqlCommand.Parameters.Add(GetEmail(user));
            sqlCommand.Parameters.Add(GetPassword(user));

            return View("Login");
        }

        [HttpPost]
        public IActionResult Verify(User user)
        {
            Connect();

            sqlCommand.CommandText = "select * from Users where Email=@Email and Password=@Password";

            sqlCommand.Parameters.Add(GetEmail(user));
            sqlCommand.Parameters.Add(GetPassword(user));

            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                Console.WriteLine(dataReader["Email"]);
                Close();
                return View("Contact", new UserViewModel { Email = user.Email});
            }
            else
            {
                Close();
                return View("Error");
            }
        }

        void Connect()
        {
            string connectString = "Server = tcp:tylercai.database.windows.net,1433; Initial Catalog = TylerCaiDB; Persist Security Info = False; User ID = { your_username }; Password ={ your_password}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";

            sqlConnection = new SqlConnection(connectString);
            sqlConnection.Open();
            sqlCommand = new SqlCommand
            {
                Connection = sqlConnection
            };
        }
        void Close()
        {
            sqlConnection.Close();
        }

        SqlParameter GetEmail(User user)
        {
            SqlParameter email = new SqlParameter();
            email.ParameterName = "@Email";
            email.Value = user.Email;
            return email;
        }

        SqlParameter GetPassword(User user)
        {
            SqlParameter password = new SqlParameter();
            password.ParameterName = "@Password";
            password.Value = user.Password;
            return password;
        }
    }
}
