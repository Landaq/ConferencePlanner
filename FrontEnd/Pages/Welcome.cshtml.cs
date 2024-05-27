using BackEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FrontEnd.Pages
{
    [SkipWelcome]
    public class WelcomeModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public WelcomeModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }
        public IActionResult OnGet()
        {
            var isAttendee = User.IsAttendee();

            if (isAttendee || !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _apiClient.AddAttendeeAsync(Attendee);
            if(!success)
            {
                ModelState.AddModelError("", "There was an issue creating the attendee for this user.");
                return Page();
            }

            // Re-issue the auth cookie with the new IsAttendee claim
            User.MakeAttendee();
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, User);

            return RedirectToPage("/Index");
        }
    }
}
