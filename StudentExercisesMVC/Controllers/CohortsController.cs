﻿using System;
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

		// GET: Cohorts/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
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