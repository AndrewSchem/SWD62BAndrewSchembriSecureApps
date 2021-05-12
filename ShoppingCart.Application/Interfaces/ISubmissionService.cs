using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
	public interface ISubmissionService
	{
        IQueryable<SubmissionViewModel> GetSubmissions();
        SubmissionViewModel GetSubmission(Guid id);
        SubmissionViewModel GetSubmission(Guid taskId, String email);
        void AddSubmission(SubmissionViewModel model);
        void DeleteSubmission(Guid id);
    }
}
