using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
	public class Student
	{
		public int Id { get; set; }

		[StringLength(20, MinimumLength = 2)]
		public string FirstName { get; set; }

		[StringLength(40, MinimumLength = 2)]
		public string LastName { get; set; }

		[StringLength(15, MinimumLength = 2)]
		public string SlackHandle { get; set; }

		public int CohortId { get; set; }

		public Cohort cohort { get; set; }

		public List<Exercise> Exercises { get; set } = new List<Exercise>();
	}
}
