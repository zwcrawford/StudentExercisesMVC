﻿
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace StudentExercisesMVC.Models.ViewModels
{
	public class StudentEditViewModel
	{
		public Student Student { get; set; }
		public List<Cohort> Cohorts { get; set; }
		public string SlackHandle { get; set; }
		public List<SelectListItem> CohortOptions
		{
			get
			{
				if (Cohorts == null)
				{
					return null;
				}
				return Cohorts.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name
				}).ToList();
			}
		}
	}
}		