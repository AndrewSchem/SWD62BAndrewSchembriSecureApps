using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
	public class Task
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		[Required]
		public string TaskName { get; set; }
		[Required]
		public DateTime Deadline { get; set; }
		[Required]
		public string TeacherEmail { get; set; }
	}
}
