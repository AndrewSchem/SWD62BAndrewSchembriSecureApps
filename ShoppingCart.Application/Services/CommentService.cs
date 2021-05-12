using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
	public class CommentService : ICommentService
	{
		private ICommentRepository _commentsRepo;
		private IMapper _autoMapper;
		public CommentService(ICommentRepository commentsRepo, IMapper autoMapper)
		{
			_commentsRepo = commentsRepo;
			_autoMapper = autoMapper;
		}

		public void AddComment(CommentViewModel model)
		{
			_commentsRepo.AddComment(_autoMapper.Map<Comment>(model));
		}

		public void DeleteComment(Guid id)
		{
			_commentsRepo.DeleteComment(id);
		}

		public CommentViewModel GetComment(Guid id)
		{
			var c = _commentsRepo.GetComment(id);
			if (c == null) return null;
			else
			{
				var result = _autoMapper.Map<CommentViewModel>(c);
				return result;
			}
		}

		public IQueryable<CommentViewModel> GetComments()
		{
			return _commentsRepo.GetComments().ProjectTo<CommentViewModel>(_autoMapper.ConfigurationProvider);
		}

		public IQueryable<CommentViewModel> GetComments(Guid submissionId)
		{
			return _commentsRepo.GetComments().Where(x => x.SubmissionId == submissionId).ProjectTo<CommentViewModel>(_autoMapper.ConfigurationProvider);
		}
	}
}
