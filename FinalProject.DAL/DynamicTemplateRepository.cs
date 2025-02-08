using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.DAL
{
    public class DynamicTemplateRepository
    {
        private readonly string _connectionString = "Data Source=LAPTOP-IJUT692C\\SQLEXPRESS;database=practice;Integrated Security=True";

        // Get all templates
        public List<DynamicTemplate> GetAllTemplates()
        {
            List<DynamicTemplate> templates = new List<DynamicTemplate>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DynamicTemplate", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    templates.Add(new DynamicTemplate
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FileTemplateName = reader["FileTemplateName"].ToString(),
                        Domain = reader["Domain"].ToString(),
                        Category = reader["Category"].ToString(),
                        SchoolYear = Convert.ToInt32(reader["SchoolYear"]),
                        Roles = reader["Roles"].ToString(),
                        StatusId = Convert.ToInt32(reader["StatusId"])
                    });
                }
            }
            return templates;
        }

        public List<DynamicTemplate> GetTemplates()
        {
            List<DynamicTemplate> templates = new List<DynamicTemplate>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DynamicTemplate WHERE StatusId = 1", con); // Fetch only active records
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    templates.Add(new DynamicTemplate
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FileTemplateName = reader["FileTemplateName"].ToString(),
                        Domain = reader["Domain"].ToString(),
                        Category = reader["Category"].ToString(),
                        SchoolYear = Convert.ToInt32(reader["SchoolYear"]),
                        Roles = reader["Roles"].ToString(),
                        StatusId = Convert.ToInt32(reader["StatusId"])
                    });
                }
            }
            return templates;
        }


  
        public void AddTemplate(DynamicTemplate template)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO DynamicTemplate (FileTemplateName, Domain, Category, SchoolYear, Roles, StatusId) " +
                               "VALUES (@FileTemplateName, @Domain, @Category, @SchoolYear, @Roles, @StatusId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FileTemplateName", template.FileTemplateName);
                cmd.Parameters.AddWithValue("@Domain", template.Domain);
                cmd.Parameters.AddWithValue("@Category", template.Category);
                cmd.Parameters.AddWithValue("@SchoolYear", template.SchoolYear);
                cmd.Parameters.AddWithValue("@Roles", template.Roles);
                cmd.Parameters.AddWithValue("@StatusId", template.StatusId);

                cmd.ExecuteNonQuery();


            }
        }

      
        public void UpdateTemplate(DynamicTemplate template)
        {
            string query = "UPDATE DynamicTemplate SET FileTemplateName = @FileTemplateName, Domain = @Domain, " +
                           "Category = @Category, SchoolYear = @SchoolYear, Roles = @Roles WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FileTemplateName", template.FileTemplateName);
                cmd.Parameters.AddWithValue("@Domain", template.Domain);
                cmd.Parameters.AddWithValue("@Category", template.Category);
                cmd.Parameters.AddWithValue("@SchoolYear", template.SchoolYear);
                cmd.Parameters.AddWithValue("@Roles", template.Roles);
                cmd.Parameters.AddWithValue("@Id", template.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SoftDeleteTemplate(int id)
        {
            string query = "UPDATE DynamicTemplate SET StatusId = 0 WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DynamicTemplate GetTemplateById(int id)
        {
            string query = "SELECT * FROM DynamicTemplate WHERE Id = @Id";
            DynamicTemplate template = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    template = new DynamicTemplate
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FileTemplateName = reader["FileTemplateName"].ToString(),
                        Domain = reader["Domain"].ToString(),
                        Category = reader["Category"].ToString(),
                        SchoolYear = Convert.ToInt32(reader["SchoolYear"]),
                        Roles = reader["Roles"].ToString(),
                        StatusId = Convert.ToInt32(reader["StatusId"])
                    };
                }
            }
            return template;
        }

        public void CreateTable(string templateName, string[] headers)
        {
            StringBuilder createTableQuery = new StringBuilder($"CREATE TABLE [{templateName}] (Id INT IDENTITY(1,1) PRIMARY KEY, ");
            foreach (var header in headers)
            {
                createTableQuery.Append($"[{header}] NVARCHAR(MAX), ");
            }
            createTableQuery.Length -= 2; // Remove last comma
            createTableQuery.Append(");");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(createTableQuery.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
    }
}
