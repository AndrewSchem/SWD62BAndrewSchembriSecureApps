using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
	public interface ITaskService
	{
        IQueryable<TaskViewModel> GetTasks();
        TaskViewModel GetTask(Guid id);
        void AddTask(TaskViewModel model);
        void DeleteTask(Guid id);
    }
}
