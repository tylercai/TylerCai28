using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data.Sql;
using TylerCai.ViewModels;

namespace TylerCai.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;

        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        public IActionResult Register()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult CreateUser(UserViewModel user)
        {
            Connect();
            sqlCommand.CommandText = "SELECT * FROM Users WHERE Email=@Email";
            sqlCommand.Parameters.Add(GetEmail(user));
            if (sqlCommand.ExecuteReader().HasRows)
            {
                ModelState.AddModelError("User", "User already exists");
                return View("Register", user);
            }

            sqlCommand.CommandText = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";

            sqlCommand.Parameters.Add(GetEmail(user));
            sqlCommand.Parameters.Add(GetPassword(user));
            sqlCommand.ExecuteNonQuery();
            Close();
            return View("Login");
        }

        [HttpPost]
        public IActionResult Verify(UserViewModel user)
        {
            Connect();

            sqlCommand.CommandText = "SELECT * from Users WHERE Email=@Email AND Password=@Password";

            sqlCommand.Parameters.Add(GetEmail(user));
            sqlCommand.Parameters.Add(GetPassword(user));

            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.HasRows)
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
