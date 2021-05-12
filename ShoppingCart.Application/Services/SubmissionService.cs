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
	public class SubmissionService : ISubmissionService
	{
		private ISubmissionRepository _submissionRepo;
		private IMapper _autoMapper;
		public SubmissionService(ISubmissionRepository submissionRepo, IMapper autoMapper)
		{
			_submissionRepo = submissionRepo;
			_autoMapper = autoMapper;
		}

		public void AddSubmission(SubmissionViewModel model)
		{
			_submissionRepo.AddSubmission(_autoMapper.Map<Submission>(model));
		}

		public void DeleteSubmission(Guid id)
		{
			_submissionRepo.DeleteSubmission(id);
		}

		public SubmissionViewModel GetSubmission(Guid id)
		{
			var s = _submissionRepo.GetSubmission(id);
			if (s == null) return null;
			else
			{
				var result = _autoMapper.Map<SubmissionViewModel>(s);
				return result;
			}
		}

		public SubmissionViewModel GetSubmission(Guid taskId, String email)
		{
			var s = _submissionRepo.GetSubmission(taskId, email);
			if (s == null) return null;
			else
			{
				var result = _autoMapper.Map<SubmissionViewModel>(s);
				return result;
			}
		}

		public IQueryable<SubmissionViewModel> GetSubmissions()
		{
			return _submissionRepo.GetSubmissions().ProjectTo<SubmissionViewModel>(_autoMapper.ConfigurationProvider);
		}
	}
}
