using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
	public interface ISubmissionRepository
	{
		Submission GetSubmission(Guid id);
		IQueryable<Submission> GetSubmissions();
		Submission GetSubmission(Guid taskId, String email);
		Guid AddSubmission(Submission s);

		void DeleteSubmission(Guid id);
	}
}
