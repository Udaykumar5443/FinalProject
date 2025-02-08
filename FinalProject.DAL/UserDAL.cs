using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.DAL
{
    public class UserDAL
    {
        private readonly string _connectionString = "Data Source=.;database=EFDBFirstDatabase;Integrated Security=True";

        public bool RegisterUser(User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "INSERT INTO Users (Username, Password, Email, Role) VALUES (@Username, @Password, @Email, @Role)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Role", "User");  // Default role

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public User ValidateUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    return new User
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Username = rdr["Username"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Role = rdr["Role"].ToString()
                    };
                }
                return null;
            }
        }
    }
}
