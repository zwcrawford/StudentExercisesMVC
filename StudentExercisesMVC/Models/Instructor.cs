using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
	public class Instructor
	{
		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(40, MinimumLength = 2)]
		public string LastName { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 2)]
		public string SlackHandle { get; set; }

		[Required]
		public int CohortId { get; set; }

		public Cohort cohort { get; set; }

	}
}
