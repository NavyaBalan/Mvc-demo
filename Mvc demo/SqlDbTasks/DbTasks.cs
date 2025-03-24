using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace Mvc_demo.SqlDbTasks
{
    public class DbTasks
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MvcdemoDbContext"].ConnectionString;
        public bool ValidateUserCredentials(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND PasswordHash = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                int count = (int)cmd.ExecuteScalar();
                if( count > 0)
                    return true;
                else
                    return false;
            }
            
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}