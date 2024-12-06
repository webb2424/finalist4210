using IST4210_hw5.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IST4210_hw5
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Student GetStudent(string email)
        {
            Student student = null;

            const string query = "SELECT * FROM DBO.STUDENT WHERE Email = @Email";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            StudentId = reader.GetInt32(0),
                            FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Gender = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Department = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Height = reader.IsDBNull(5) ? 0 : reader.GetInt32(5), // Changed to int
                            Major = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            Email = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Password = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                        };
                    }
                }
            }

            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            const string query = "SELECT * FROM DBO.STUDENT";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new Student
                        {
                            StudentId = reader.GetInt32(0),
                            FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Gender = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Department = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Height = reader.IsDBNull(5) ? 0 : reader.GetInt32(5), // Changed to int
                            Major = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            Email = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            Password = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                        };
                    }
                }
            }
        }

        public void InsertStudent(string firstName, string lastName, string email, string password, string gender, int height, string department, string major)
        {
            const string query = @"
                INSERT INTO DBO.STUDENT
                ([FirstName], [LastName], [Gender], [Department], [Height], [Major], [Email], [Password])
                VALUES (@FirstName, @LastName, @Gender, @Department, @Height, @Major, @Email, @Password)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;
                command.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = gender;
                command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = department;
                command.Parameters.Add("@Height", SqlDbType.Int).Value = height; // Changed to Int
                command.Parameters.Add("@Major", SqlDbType.NVarChar).Value = major;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertEnrollment(int studentId, string term, string year)
        {
            const string query = @"
                INSERT INTO DBO.Enrollment ([StudentId], [Term], [Year])
                VALUES (@StudentId, @Term, @Year)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
                command.Parameters.Add("@Term", SqlDbType.NVarChar).Value = term;
                command.Parameters.Add("@Year", SqlDbType.NVarChar).Value = year;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool CheckEnrollment(int studentId)
        {
            const string query = "SELECT 1 FROM DBO.Enrollment WHERE StudentId = @StudentId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }
    }
}
