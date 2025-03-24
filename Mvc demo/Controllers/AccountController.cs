using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using Mvc_demo.SqlDbTasks;
using Microsoft.Owin.Security;
using System.Web;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        DbTasks dbTasks = new DbTasks();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MvcdemoDbContext"].ConnectionString;

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (dbTasks.ValidateUserCredentials(email, password))
            {
              
                Session["UserEmail"] = email;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Login");
            }
        }


        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(string name, string email, string password)
        {
            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (FullName, Email, PasswordHash) VALUES (@Name, @Email, @Password)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                try
                {
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "Registration successful! You can now login.";
                    return RedirectToAction("Login");
                }
                catch (SqlException)
                {
                    ViewBag.Error = "Email already exists.";
                    return View();
                }
            }
        }

        // Password Hashing Method
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    
        public ActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", "Account", null, Request.Url.Scheme)
            };

            AuthenticationManager.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        // Google Authentication Callback
        public ActionResult GoogleLoginCallback()
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                TempData["ErrorMessage"] = "Google authentication failed.";
                return RedirectToAction("Login");
            }

            // Save user details in session or database
            Session["UserEmail"] = loginInfo.Email;
            Session["UserName"] = loginInfo.DefaultUserName;

            return RedirectToAction("Index", "Home");
        }

    }
}
