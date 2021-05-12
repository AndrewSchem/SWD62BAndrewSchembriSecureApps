using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
	public class Submission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[Required]
		public DateTime DateUploaded { get; set; }

		[Required]
		public string FilePath { get; set; }

		[Required]
		public virtual Task Task { get; set; }

		[ForeignKey("Task")]
		public Guid TaskId { get; set; }

		[Required]
		public string UploaderEmail { get; set; }

		public virtual IList<Comment> Comments { get; set; }

		public string Signature { get; set; }

		public string Hash { get; set; }
	}
}
