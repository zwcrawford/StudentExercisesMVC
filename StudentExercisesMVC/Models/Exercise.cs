using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
	public class Exercise
	{
		public int Id { get; set; }

		[StringLength(10, MinimumLength = 3)]
		public string Name { get; set; }

		public List<Student> Students { get; set } = new List<Student>();

		public List<Instructor> Instructors { get; set } = new List<Instructor>();
	}
}
