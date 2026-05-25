using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamsManagement
{
    internal class Examination
    {
        private string connectionString= "Server=localhost;Database=grp4_exam_db;Uid=root;Pwd=password;";

        public List<Mark> GetAllMarks()
        {
            List<Mark> marks = new List<Mark>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM marks";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mark mark = new Mark
                            {
                                Id = reader.GetInt32("Id"),
                                StudentNumber = reader.GetString("StudentNumber"),
                                MarkValue = reader.GetInt32("Mark"),
                                Grade = reader.GetString("Grade")
                            };
                            marks.Add(mark);
                        }
                    }
                }
            }
            return marks;
        }

        public void AddMark(Mark mark)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var query = "INSERT INTO marks (StudentNumber, Mark, Grade) VALUES (@studentNumber, @mark, @grade)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentNumber", mark.StudentNumber);
                cmd.Parameters.AddWithValue("@mark", mark.MarkValue);
                cmd.Parameters.AddWithValue("@grade", mark.Grade);
                cmd.ExecuteNonQuery();// Execute the query to insert the mark into the database
            }
        }
        public void UpdateMark(Mark mark)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var query = "UPDATE marks SET StudentNumber = @studentNumber,Mark = @mark, Grade = @grade WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", mark.Id);
                cmd.Parameters.AddWithValue("@studentNumber", mark.StudentNumber);
                cmd.Parameters.AddWithValue("@mark", mark.MarkValue);
                cmd.Parameters.AddWithValue("@grade", mark.Grade);
                cmd.ExecuteNonQuery();// Execute the query to update the mark in the database
            }
        }

        public void DeleteMark(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var query = "DELETE FROM marks WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();// delete the mark from the database
            }
        }
    }
}
