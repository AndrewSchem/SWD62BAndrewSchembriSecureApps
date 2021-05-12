using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class TasksController : Controller
	{
        private readonly ITaskService _taskService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<TasksController> _logger;
        public TasksController(ITaskService taskService, IWebHostEnvironment host, ILogger<TasksController> logger)
        {
            _logger = logger;
            _host = host;
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            var list = _taskService.GetTasks();
            return View(list);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(TaskViewModel data)
        {
            data.TaskName = HtmlEncoder.Default.Encode(data.TaskName);
            data.Deadline = data.Deadline; 
            data.TeacherEmail = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (data.Deadline > DateTime.Now)
                {
                    _taskService.AddTask(data);

                    TempData["message"] = "Task Created Successfully";
                    return View();
                }
                else {
                    TempData["error"] = "Deadline cant be in the past";
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Check your input. Operation failed");
                return View(data);
            }
        }

        public IActionResult Task()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Task(string taskId, string userName)
        {
            String taskIdentity = Encryption.SymmetricDecrypt(taskId);
            String userEmail = Encryption.SymmetricDecrypt(userName);

            return View();
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult SubmittedTask()
        {
            return View();
        }

        public IActionResult SubmittedTasks() {
            var list = _taskService.GetTasks();
            return View(list);
        }
    }
}
