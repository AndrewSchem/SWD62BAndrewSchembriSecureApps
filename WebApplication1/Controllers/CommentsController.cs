using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
	[Authorize]
	public class CommentsController : Controller
	{
		private readonly ICommentService _commentService;
		private readonly IWebHostEnvironment _host;
		private readonly ILogger<CommentsController> _logger;
		public CommentsController(ICommentService commentService, IWebHostEnvironment host, ILogger<CommentsController> logger)
		{
			_logger = logger;
			_host = host;
			_commentService = commentService;
		}

		public IActionResult Create(string submissionId)
		{
			ViewBag.submissionId = submissionId;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult Create(string submissionId, CommentViewModel data)
		{
			if (submissionId.IsNullOrEmpty() == false && data != null)
			{
				var submission = Encryption.SymmetricDecrypt(submissionId);
				data.SubmissionId = Guid.Parse(submission);
				data.Message = HtmlEncoder.Default.Encode(data.Message);
				data.Email = User.Identity.Name;
				_commentService.AddComment(data);
			}

			TempData["message"] = "Comment Added Successfully";
			return Redirect("/Tasks/Index");
		}
	}
}
