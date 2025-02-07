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
        private readonly string _connectionString = "Data Source=.;database=EFDBFirstDatabase;Integrated Security=True";

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

        //public List<DynamicTemplate> GetTemplates()
        //{
        //    string query = "SELECT * FROM DynamicTemplate WHERE StatusId = 1";
        //    List<DynamicTemplate> templates = new List<DynamicTemplate>();

        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            templates.Add(new DynamicTemplate
        //            {
        //                Id = Convert.ToInt32(reader["Id"]),
        //                FileTemplateName = reader["FileTemplateName"].ToString(),
        //                Domain = reader["Domain"].ToString(),
        //                Category = reader["Category"].ToString(),
        //                SchoolYear = Convert.ToInt32(reader["SchoolYear"]),
        //                Roles = reader["Roles"].ToString(),
        //                StatusId = Convert.ToInt32(reader["StatusId"])
        //            });
        //        }
        //    }
        //    return templates;
        //}

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


        // Add new template
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

        public void SaveFileData(List<DynamicTemplate> templates)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                foreach (var template in templates)
                {
                    string query = "INSERT INTO DynamicTemplate (FileTemplateName, Domain, Category, SchoolYear, Roles, StatusId) " +
                                   "VALUES (@FileTemplateName, @Domain, @Category, @SchoolYear, @Roles, @StatusId)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FileTemplateName", template.FileTemplateName);
                        cmd.Parameters.AddWithValue("@Domain", template.Domain);
                        cmd.Parameters.AddWithValue("@Category", template.Category);
                        cmd.Parameters.AddWithValue("@SchoolYear", template.SchoolYear);
                        cmd.Parameters.AddWithValue("@Roles", template.Roles);
                        cmd.Parameters.AddWithValue("@StatusId", template.StatusId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<DynamicTemplate> GetFileData(int fileId)
        {
            List<DynamicTemplate> fileData = new List<DynamicTemplate>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM DynamicTemplate WHERE Id = @FileId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FileId", fileId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fileData.Add(new DynamicTemplate
                            {
                                Id = reader.GetInt32(0),
                                FileTemplateName = reader.GetString(1),
                                Domain = reader.GetString(2),
                                Category = reader.GetString(3),
                                SchoolYear = Convert.ToInt32(reader["SchoolYear"]),
                                Roles = reader.GetString(5),
                                StatusId = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return fileData;
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

    }
}
