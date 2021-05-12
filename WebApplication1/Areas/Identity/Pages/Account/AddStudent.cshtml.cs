using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Teacher")]
    public class AddStudent : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public AddStudent(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public async System.Threading.Tasks.Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                RandomPasswordGenerator pg = new RandomPasswordGenerator();
                string password = pg.GeneratePassword(true, true, true, true, 16);

                AsymmetricKeys keys = Encryption.GenerateAsymmetricKeys();
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, Address= "", PrivateKey = keys.PrivateKey.ToString(), PublicKey = keys.PublicKey.ToString()};
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, "STUDENT");

                        //noreplysecureapps1@gmail.com //Testing_123
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("noreplysecureapps1@gmail.com", "Testing_123");

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("noreplysecureapps1@gmail.com");
                        msg.To.Add(Input.Email);
                        msg.Subject = "New Account";
                        msg.Body = "New Account with Password: " + password;
                        //msg.Body = "New Account with Password: []!";

                        client.Send(msg);

                        TempData["message"] = "Student Created Successfully";
                        return Page();
                    }

                    //await _emailSender.SendEmailAsync(Input.Email, "New Account", $"Account Created. Passowrd: ["+password+"]");

                    _logger.LogInformation("User created a new account with password.");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
