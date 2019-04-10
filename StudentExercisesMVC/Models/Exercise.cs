using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
	public class Exercise
	{
		public int Id { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 3)]
		public string Language { get; set; }

		public List<Student> Students { get; set; } = new List<Student>();
	}
}
