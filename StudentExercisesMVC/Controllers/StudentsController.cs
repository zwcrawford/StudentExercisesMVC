using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;


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

        //  Get all students
        public ActionResult Index(string q, string firstName = "", string lastName = "", string slackHandle = "", string include = "")
        {
            string searchFirst = (firstName == "") ? "%" : firstName;
            string searchLast = (firstName == "") ? "%" : firstName;
            string searchSlack = (firstName == "") ? "%" : firstName;

            if (include != "exercises")
            {
                List<Student> students = new List<Student>();
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                        @"
                            SELECT s.Id
                                s.FirstName,
                                s.LastName,
                                s.SlackHandle,
                                s.CohortId
                            FROM Student s
                        ";

                        if (!string.IsNullOrWhiteSpace(q))
                        {
                            cmd.CommandText += @" AND";
                            cmd.Parameters.Add(new SqlParameter("q", $"%{q}%"));
                        }
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Student student = new Student(reader.GetInt32(reader.GetOrdinal("studentId")),

                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.GetString(reader.GetOrdinal("SlackHandle")),
                                reader.GetInt32(reader.GetOrdinal("CohortId")))
                            {
                                //May need to tweak this to 'Cohort cohort = new...'
                                Cohort = new Cohort(
                                    reader.GetInt32(reader.GetOrdinal("cohortId")),
                                    reader.GetString(reader.GetOrdinal("cohortName")))
                            };
                            students.Add(student);
                        }
                        reader.Close();
                        return View(students);
                    } 
                }
            }
        } 

        // GET: Students
        public ActionResult Index()
        {
            return View();
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Students/Edit/5
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

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Students/Delete/5
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