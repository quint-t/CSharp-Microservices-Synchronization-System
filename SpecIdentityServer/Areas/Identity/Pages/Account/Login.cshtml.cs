using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Duende.IdentityServer.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Grpc.Core;

namespace SpecIdentityServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    var eventType = "LoginUser";
                    string[] events = _configuration.GetSection("Announcer:Events").GetChildren().ToArray().Select(c => c.Value).ToArray();
                    string url = _configuration.GetSection("Announcer:URL").Get<string>();

                    using var channel = GrpcChannel.ForAddress(url);

                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    CancellationToken token = cancelTokenSource.Token;

                    cancelTokenSource.CancelAfter(100);
                    await channel.ConnectAsync(token);
                    if (channel.State == ConnectivityState.Ready)
                    {
                        var client = new EventHandler.EventHandlerClient(channel);

                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var user = await _userManager.FindByNameAsync(Input.Email);
                            if (!url.IsNullOrEmpty() && !events.IsNullOrEmpty() &&
                                events.Contains(eventType))
                            {
                                var jsonData = JsonConvert.SerializeObject(new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email
                                });
                                var reply = await client.NewEventAsync(
                                    new NewEventRequest
                                    {
                                        EventUniqueName = eventType,
                                        EventDate = DateTime.UtcNow.ToTimestamp(),
                                        EventData = jsonData,
                                    }
                                );
                                if (reply.Status != 200)
                                {
                                    await _signInManager.SignOutAsync();
                                    ErrorMessage = "Now there are problems on the server.\n";
                                    ErrorMessage += "Error code: " + reply.Status.ToString() + ".\n";
                                    ErrorMessage += "Error message: " + reply.StatusMessage + ".";
                                    ModelState.AddModelError(string.Empty, ErrorMessage);
                                    return Page();
                                }
                            }
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid password.");
                            return Page();
                        }
                    }
                }
            }
            catch
            {
                ErrorMessage = "Now there are problems on the server.";
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
