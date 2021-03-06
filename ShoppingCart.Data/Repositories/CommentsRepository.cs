using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
	public class CommentsRepository : ICommentRepository
	{
		ShoppingCartDbContext _context;
		public CommentsRepository(ShoppingCartDbContext context)
		{
			_context = context;
		}

		public Guid AddComment(Comment c)
		{
			c.Id = Guid.NewGuid();
			_context.Comments.Add(c);
			_context.SaveChanges();

			return c.Id;
		}

		public void DeleteComment(Guid id)
		{
			Comment c = GetComment(id);
			_context.Comments.Remove(c);
			_context.SaveChanges();
		}

		public Comment GetComment(Guid id)
		{
			return _context.Comments.SingleOrDefault(x => x.Id == id);
		}

		public IQueryable<Comment> GetComments()
		{
			return _context.Comments.Include(x => x.Submission);
		}

		public IQueryable<Comment> GetComments(Guid submissionId)
		{
			return _context.Comments.Include(x => x.Submission).Where(x => x.SubmissionId == submissionId);
		}
	}
}
