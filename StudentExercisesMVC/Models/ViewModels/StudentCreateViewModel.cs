﻿/*
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Models.ViewModels
{
	public class StudentCreateViewModel
	{
		public StudentCreateViewModel()	
		{
			Cohorts = new List<Cohort>();
		}

		public StudentCreateViewModel(string connectionString)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = conn.CreateCommand())
				{
					cmd.CommandText = "SELECT id, name FROM Cohort";
					SqlDataReader reader = cmd.ExecuteReader();

					Cohorts = new List<Cohort>();

					while (reader.Read())
					{
						Cohorts.Add(new Cohort
						{
							Id = reader.GetInt32(reader.GetOrdinal("id")),
							Name = reader.GetString(reader.GetOrdinal("name"))
						});
					}
					reader.Close();
				}
			}
		}
		public Student Student { get; set; }
		public List<Cohort> { get; set; }
		public string SlackHandle { get; set; }

		public List<SelectedItemList>  CohortOptions
		{
		get
		{
			return Cohorts.Select(c => new SelectItemList
			{
				ValueTask = c.Id.ToString(),
				Text = c.Name
			}).ToList();
		}
	}
}
  */