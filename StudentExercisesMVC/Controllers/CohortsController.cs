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
    public class CohortsController : Controller
    {
		private readonly IConfiguration _config;

		public CohortsController(IConfiguration config)
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
		// GET COHORTS

		// When adding the View for an Index, do not make a folder first. Right click the method.
		// View Name will be Index, GET will use a List template, and then pick the class from dropdown.
		public ActionResult Index()
		{
			using (SqlConnection conn = Connection)
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"SELECT c.Id AS CohortId, c.Name
										  FROM Cohort c;";

					SqlDataReader reader = cmd.ExecuteReader();
					List<Cohort> cohorts = new List<Cohort>();

					while (reader.Read())
					{
						Cohort cohort = new Cohort
						{
							Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
							Name = reader.GetString(reader.GetOrdinal("Name")), 
						};
						cohorts.Add(cohort);
					}
					reader.Close();
					return View(cohorts);
				}
			}
		}

		// GET: COHORTS/DETAILS/5
		public ActionResult Details(int id)
		{
			using (SqlConnection conn = Connection)
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText =
					@"
                        SELECT c.Id AS CohortId, c.Name AS CohortName, 
									s.id AS StudentId, s.FirstName AS StudentFirstName,
									s.LastName AS StudentLastName, s.SlackHandle AS StudentSlackHandle,
										i.Id AS InstructorId, i.FirstName AS InstructorFirstName,
										i.LastName AS InstructorLastName, i.SlackHandle AS InstructorSlackHandle
                          FROM Cohort c
                                 LEFT JOIN Student s ON c.Id = s.CohortId
                                 LEFT JOIN Instructor i ON c.Id = i.CohortId
                         WHERE c.Id = @id";
					cmd.Parameters.Add(new SqlParameter("@id", id));

					SqlDataReader reader = cmd.ExecuteReader();

					Cohort cohort = null;

					while (reader.Read())
					{
						if (cohort == null)
						{
							cohort = new Cohort
							{
								Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
								Name = reader.GetString(reader.GetOrdinal("CohortName")),
							};
						}
						if (!reader.IsDBNull(reader.GetOrdinal("CohortId")))
						{
							if (!reader.IsDBNull(reader.GetOrdinal("StudentId")))
							{
								if (!cohort.Students.Exists(stu => stu.Id == reader.GetInt32(reader.GetOrdinal("StudentId"))))
								{
									cohort.Students.Add(new Student
									{
										Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
										FirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
										LastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
									});
								}
							}
							if (!reader.IsDBNull(reader.GetOrdinal("InstructorId")))
							{
								if (!cohort.Instructors.Exists(ins => ins.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
								{
									cohort.Instructors.Add(new Instructor
									{
										Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
										FirstName = reader.GetString(reader.GetOrdinal("InstructorFirstName")),
										LastName = reader.GetString(reader.GetOrdinal("InstructorLastName")),
									});
								}
							}
						}
					};
					reader.Close();
					return View(cohort);
				}
			}
		}
		
		// GET: Cohorts/Create
		public ActionResult Create()
        {
			return View();
		}

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(Cohort Cohort)
		{
			try
			{
				using (SqlConnection conn = Connection)
				{
					conn.Open();
					using (SqlCommand cmd = conn.CreateCommand())
					{
						cmd.CommandText = @"INSERT INTO cohort (name)
										   VALUES (@name)";
						
						cmd.Parameters.Add(new SqlParameter("@name", Cohort.Name)); 

						cmd.ExecuteNonQuery();

						return RedirectToAction(nameof(Index));
					}
				}
			}
			catch
			{
				return View(Cohort);
			};
		}
		private List<Cohort> GetAllCohorts()
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

		// GET: Cohorts/Edit/5
		public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}