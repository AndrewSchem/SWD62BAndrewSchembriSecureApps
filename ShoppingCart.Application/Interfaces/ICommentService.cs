using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
	public interface ICommentService
	{
		IQueryable<CommentViewModel> GetComments();
		CommentViewModel GetComment(Guid id);
		IQueryable<CommentViewModel> GetComments(Guid submissionId);
		void AddComment(CommentViewModel model);
		void DeleteComment(Guid id);
	}
}
