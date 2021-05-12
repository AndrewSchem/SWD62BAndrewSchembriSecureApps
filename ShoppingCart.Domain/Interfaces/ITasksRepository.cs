using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Interfaces
{
	public interface ITasksRepository
	{
        Task GetTask(Guid id);
        IQueryable<Task> GetTasks();
        Guid AddTask(Task p);
        void DeleteTask(Guid id);
    }
}
