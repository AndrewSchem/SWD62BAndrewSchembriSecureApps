using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
	[Authorize]
	public class SubmissionsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ISubmissionService _submissionService;
		private readonly IWebHostEnvironment _host;
		private readonly ILogger<SubmissionsController> _logger;
		public SubmissionsController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager, IWebHostEnvironment host, ILogger<SubmissionsController> logger)
		{
			_logger = logger;
			_host = host;
			_submissionService = submissionService;
			_userManager = userManager;
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Index()
		{
			var list = _submissionService.GetSubmissions();
			return View(list);
		}

		[HttpGet]
		public IActionResult Details(string taskId, string email)
		{

			if (taskId.IsNullOrEmpty() == false)
			{
				try
				{
					var submission = _submissionService.GetSubmission(Guid.Parse(Encryption.SymmetricDecrypt(taskId)), Encryption.SymmetricDecrypt(email));
					ViewBag.taskId = taskId;
					ViewBag.Copied = true;
					if (submission != null) {
						IEnumerable<SubmissionViewModel> copies = _submissionService.GetSubmissions().Where(x => x.Hash == submission.Hash);
						if (copies.Count() > 1)
						{
							ViewBag.Copied = false;
						} 
					}
					//submission.TaskId = Guid.Parse(Encryption.SymmetricDecrypt(taskId));
					return View(submission);
				}
				catch (Exception e) {
					return Redirect("/Home/Index");
				}
			}
			else
			{
				return Redirect("/Home/Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> DetailsAsync(IFormFile file, String taskId,SubmissionViewModel data)
		{
			try
			{
				data.TaskId = Guid.Parse(Encryption.SymmetricDecrypt(taskId));
				data.UploaderEmail = User.Identity.Name;
				data.DateUploaded = DateTime.Now;
				if (file != null)
				{
					if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
					{
						//FF D8 >>>>> 255 216
						string uniqueFilename;
						//byte[] whitelist = new byte[] { 255, 216 };
						byte[] whitelist = new byte[] { 37, 80 };

						if (file != null)
						{
							MemoryStream userFile = new MemoryStream();

							using (var f = file.OpenReadStream())
							{
								byte[] buffer = new byte[2];
								f.Read(buffer, 0, 2);
								for (int i = 0; i < whitelist.Length; i++)
								{
									if (whitelist[i] == buffer[i])
									{ }
									else
									{
										TempData["Error"] = "Task Not Submitted";
										ModelState.AddModelError("file", "File Not Valid");
										return View();
									}
								}

								f.CopyTo(userFile);

								string hash = Encryption.Hash(userFile.ToString());

								f.Position = 0;

								uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
								data.FilePath = uniqueFilename;
								string absolutePath = _host.WebRootPath + @"\submissions\" + uniqueFilename;
								try
								{
									using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
									{
										
										f.CopyTo(fsOut);
									}
									f.Close();

									

									file.CopyTo(userFile);
									data.Hash = hash;

									ApplicationUser user = await _userManager.FindByEmailAsync(User.Identity.Name);
									data.Signature = Encryption.SignData(userFile, user.PrivateKey);

									TempData["message"] = "Task Submitted Successfully";
									_submissionService.AddSubmission(data);
								}
								catch (Exception ex)
								{
									TempData["error"] = "Error Uploading File";
									_logger.LogError(ex, "Error While Saving File");
									return View("Error", new ErrorViewModel() { Message = "Error While Saving File. Try Again Later" + ex });
								}
							}

						}
					}
				}

				return Redirect("/Tasks/Index");
			}
			catch (Exception e)
			{
				TempData["error"] = "Task Not Submitted";
				return Redirect("/Tasks/Index");
			}
		}

		public IActionResult Download(string submissionId) {
			try
			{
				string sub = "";
				if (submissionId.IsNullOrEmpty() == false)
				{
					sub = Encryption.SymmetricDecrypt(submissionId);
				}
				var submission = _submissionService.GetSubmission(Guid.Parse(sub));

				return File(System.IO.File.OpenRead(_host.WebRootPath + @"\submissions\" + submission.FilePath), "application/pdf");
			}
			catch (Exception e) {
				TempData["error"] = "Error Downloading File";
				_logger.LogError("ERROR Downloading File: "+ e);
				return Redirect("/Tasks/Index");
			}
		}
	}
}
