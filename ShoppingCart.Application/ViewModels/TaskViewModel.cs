using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
	public class TaskViewModel
	{
		public Guid Id { get; set; }
		[Required(ErrorMessage = "Please input task name")]
		public string TaskName { get; set; }

		[Required(ErrorMessage = "Please set a deadline")]
		public DateTime Deadline { get; set; }
		public string TeacherEmail { get; set; }
	}
}
