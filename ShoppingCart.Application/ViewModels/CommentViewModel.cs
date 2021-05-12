using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
	public class CommentViewModel
	{
		public Guid id { get; set; }

		[Required(ErrorMessage = "Please Type a Comment")]
		public string Message { get; set; }
		public Guid SubmissionId { get; set; }
		public virtual SubmissionViewModel Submission { get; set; }

		public string Email { get; set; }
	}
}
