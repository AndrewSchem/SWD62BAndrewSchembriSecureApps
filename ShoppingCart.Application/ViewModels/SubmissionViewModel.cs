using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
	public class SubmissionViewModel
	{
		public Guid Id { get; set; }

		public DateTime DateUploaded { get; set; }

		[Required(ErrorMessage = "Please Upload File")]
		public string FilePath { get; set; }

		public Guid TaskId { get; set; }

		public virtual TaskViewModel Task { get; set; }

		public string UploaderEmail { get; set; }

		public virtual IList<CommentViewModel> Comments {get; set;}

		public string Signature { get; set; }

		public string Hash { get; set; }
	}
}
