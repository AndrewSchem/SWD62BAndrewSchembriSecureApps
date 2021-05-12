using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingCart.Domain.Models;
using AutoMapper.QueryableExtensions;

namespace ShoppingCart.Application.Services
{
	public class TaskService : ITaskService
	{
		private ITasksRepository _tasksRepo;
		private IMapper _autoMapper;
		public TaskService(ITasksRepository tasksRepo, IMapper autoMapper)
		{
			_tasksRepo = tasksRepo;
			_autoMapper = autoMapper;
		}

		public void AddTask(TaskViewModel model)
		{
			_tasksRepo.AddTask(_autoMapper.Map<Task>(model));
		}

		public void DeleteTask(Guid id)
		{
			_tasksRepo.DeleteTask(id);
		}

		public TaskViewModel GetTask(Guid id)
		{
			var t = _tasksRepo.GetTask(id);
			if (t == null) return null;
			else
			{
				var result = _autoMapper.Map<TaskViewModel>(t);
				return result;
			}
		}

		public IQueryable<TaskViewModel> GetTasks()
		{
			return _tasksRepo.GetTasks().ProjectTo<TaskViewModel>(_autoMapper.ConfigurationProvider);
		}
	}
}
