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
	public class SubmissionRepository : ISubmissionRepository
	{
		ShoppingCartDbContext _context;
		public SubmissionRepository(ShoppingCartDbContext context)
		{
			_context = context;
		}
		public Guid AddSubmission(Submission s)
		{
			s.Id = Guid.NewGuid();
			_context.Submissions.Add(s);
			_context.SaveChanges();

			return s.Id;
		}

		public void DeleteSubmission(Guid id)
		{
			Submission s = GetSubmission(id);
			_context.Submissions.Remove(s);
			_context.SaveChanges();
		}

		public Submission GetSubmission(Guid id)
		{
			return _context.Submissions.Include(x => x.Task).SingleOrDefault(x => x.Id == id);
		}

		public Submission GetSubmission(Guid taskId, String email)
		{
			return _context.Submissions.Include(x => x.Task).SingleOrDefault(x => x.TaskId == taskId && x.UploaderEmail == email);
		}

		public IQueryable<Submission> GetSubmissions()
		{
			return _context.Submissions.Include(x => x.Task);
		}
	}
}
