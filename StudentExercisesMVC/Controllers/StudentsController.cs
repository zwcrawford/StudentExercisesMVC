using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        //  GET STUDENTS
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                    @"
                        SELECT s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId,
							   c.Name AS CohortName
                        FROM Student s
								LEFT JOIN Cohort c ON s.CohortId = c.Id";
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
							Cohort = new Cohort
							{
								Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
								Name = reader.GetString(reader.GetOrdinal("CohortName"))
							}
						}; 
                        students.Add(student);
                    }
                    reader.Close();
                    return View(students);
                } 
            }   
        }
		
        // GET STUDENT/DETAILS
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
					@"
                        SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId,
                               c.[Name] AS CohortName, e.Id AS ExerciseId, e.[Name] AS ExerciseName,
                               e.[Language]
                          FROM Student s
                                 LEFT JOIN Cohort c ON s.CohortId = c.Id
                                 LEFT JOIN StudentExercise se ON s.Id = se.StudentId
								 LEFT JOIN Exercise e on se.exerciseId = se.Id
                         WHERE s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    while (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        }; 
                    }
                    reader.Close();
                    return View(student);
                }
            }
        }
		
	   // GET: STUDENT/CREATE
	   public ActionResult Create()
	   {
		   {
			   StudentCreateViewModel viewModel =
				   new StudentCreateViewModel(_config.GetConnectionString("DefaultConnection"));
			   return View(viewModel);
		   } 
	   }

	   // POST: Students/Create
	   [HttpPost]
	   [ValidateAntiForgeryToken]
	   public ActionResult Create(StudentCreateViewModel viewModel)
	   {
		   try
		   {
			   using (SqlConnection conn = Connection)
			   {
				   conn.Open();
				   using (SqlCommand cmd = conn.CreateCommand())
				   {
					   cmd.CommandText = @"INSERT INTO student (firstname, lastname, slackhandle, cohortid)
										   VALUES (@firstname, @lastname, @slackhandle, @cohortid)";
					   cmd.Parameters.Add(new SqlParameter("@firstname", viewModel.Student.FirstName));
					   cmd.Parameters.Add(new SqlParameter("@lastname", viewModel.Student.LastName));
					   cmd.Parameters.Add(new SqlParameter("@slackhandle", viewModel.Student.SlackHandle));
					   cmd.Parameters.Add(new SqlParameter("@cohortid", viewModel.Student.CohortId));

					   cmd.ExecuteNonQuery();

					   return RedirectToAction(nameof(Index));
				   }
			   }
		   }
		   catch
		   {
				viewModel.Cohorts = GetAllCohorts();
			    return View(viewModel);
		   };
	   }
	   private List<Cohort>	GetAllCohorts()
	   {
			using (SqlConnection conn = Connection)
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"SELECT id, name from Cohort;";
					SqlDataReader reader = cmd.ExecuteReader();

					List<Cohort> cohorts = new List<Cohort>();

					while (reader.Read())
					{
						cohorts.Add(new Cohort
						{
							Id = reader.GetInt32(reader.GetOrdinal("Id")),
							Name = reader.GetString(reader.GetOrdinal("Name"))
						});
					}
					reader.Close();

					return cohorts;
				}
			}
		}
		// We need to GET the student first and then do a POST to submit the changes.
		// GET: STUDENT/EDIT/{id}
		public ActionResult Edit(int id)
		{
			Student student = GetStudentById(id);
			if (student == null)
			{
				return NotFound();
			}

			StudentEditViewModel viewModel = new StudentEditViewModel
			{
				Cohorts = GetAllCohorts(),
				Student = student
			};

			return View(viewModel);
		}

		// POST: STUDENT/EDIT/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, StudentEditViewModel viewModel)
		{
			try
			{
				using (SqlConnection conn = Connection)
				{
					conn.Open();
					using (SqlCommand cmd = conn.CreateCommand())
					{
						cmd.CommandText = @"UPDATE Student
											   SET firstname = @firstname,lastname = @lastname,
												   slackhandle = @slackhandle, cohortid = @cohortid
											 WHERE id = @id";
						cmd.Parameters.Add(new SqlParameter("@firstname", viewModel.Student.FirstName));
						cmd.Parameters.Add(new SqlParameter("@lastname", viewModel.Student.LastName));
						cmd.Parameters.Add(new SqlParameter("@slackhandle", viewModel.Student.SlackHandle));
						cmd.Parameters.Add(new SqlParameter("@cohortid", viewModel.Student.CohortId));
						cmd.Parameters.Add(new SqlParameter("@id", id));

						cmd.ExecuteNonQuery();

						return RedirectToAction(nameof(Index));
					}
				}
			}
			catch
			{
				viewModel.Cohorts = GetAllCohorts();
				return View(viewModel);
			}
		}

		// GET: Students/Delete/5
		public ActionResult Delete(int id)
		{
			Student student = GetStudentById(id);
			if (student == null)
			{
				return NotFound();
			}
			else
			{
				return View(student);
			}
		}

		// POST: Students/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Student student)
		{
			try
			{
				using (SqlConnection conn = Connection)
				{
					conn.Open();
					using (SqlCommand cmd = conn.CreateCommand())
					{
						cmd.CommandText = @"DELETE FROM StudentExercise WHERE StudentId = @id;
										  DELETE FROM Student WHERE Id = @id;";
						cmd.Parameters.Add(new SqlParameter("@id", id));

						cmd.ExecuteNonQuery();

						return RedirectToAction(nameof(Index));
					}
				}
			}
			catch
			{
				return View(student);
			}
		}

		// GetStudentById Method:
		private Student GetStudentById(int id)
		{
			using (SqlConnection conn = Connection)
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"SELECT s.Id AS StudentId, s.FirstName, s.LastName, 
											   s.SlackHandle, s.CohortId, c.Name AS CohortName
										  FROM Student s 
												 LEFT JOIN Cohort c on s.cohortid = c.id
										 WHERE s.Id = @id";
					cmd.Parameters.Add(new SqlParameter("@id", id));
					SqlDataReader reader = cmd.ExecuteReader();

					Student student = null;

					if (reader.Read())
					{
						student = new Student
						{
							Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
							FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
							LastName = reader.GetString(reader.GetOrdinal("LastName")),
							SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
							CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
							Cohort = new Cohort
							{
								Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
								Name = reader.GetString(reader.GetOrdinal("CohortName"))
							}
						};
					}
					reader.Close();
					return student;
				}
			}
		}
	}
}