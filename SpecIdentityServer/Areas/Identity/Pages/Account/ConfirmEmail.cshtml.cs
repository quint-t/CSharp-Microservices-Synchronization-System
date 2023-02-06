using System.Text;
using Duende.IdentityServer.Extensions;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Google.Protobuf.WellKnownTypes;

namespace SpecIdentityServer.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ConfirmEmailModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;

		public ConfirmEmailModel(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return RedirectToPage("/Index");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{userId}'.");
			}

			code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
			var result = await _userManager.ConfirmEmailAsync(user, code);

			if (!result.Succeeded)
			{
				StatusMessage = "Error confirming your email.";
				return Page();
			}

			try
			{
				var eventType = "NewUserRegistration";
				var appSettings = System.Configuration.ConfigurationManager.AppSettings;
				string url = appSettings.Get("Announcer:URL");
				string[] events = appSettings.GetValues("Announcer:Events");
				if (!url.IsNullOrEmpty() && !events.IsNullOrEmpty() &&
					events.Contains(eventType))
				{
					using var channel = GrpcChannel.ForAddress(url);
					var client = new EventHandler.EventHandlerClient(channel);
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
					if (reply.Status == 200)
					{
						StatusMessage = "Thank you for confirming your email.\n";
						StatusMessage += "Now you have access to all services.";
					}
					else
					{
						StatusMessage = "Thank you for confirming your email.\n";
						StatusMessage += "Now there are problems on the server.\n";
						StatusMessage += "Error code:" + reply.Status.ToString() + ".\n";
						StatusMessage += "Error message:" + reply.StatusMessage + ".";
					}
				}
			}
			catch
			{
				StatusMessage = "Thank you for confirming your email.\n";
				StatusMessage += "Now there are problems on the server.";
			}
			return Page();
		}
	}
}
