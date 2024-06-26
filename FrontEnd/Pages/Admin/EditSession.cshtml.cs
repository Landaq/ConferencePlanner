using FrontEnd.Services;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Admin
{
    public class EditSessionModel : PageModel
    {
        [BindProperty]
        public Session Session { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        private readonly IApiClient _apiClient;

        public EditSessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync(int id)
        {
            var session = await _apiClient.GetSessionAsync(id);
            Session = new Session
            {
                Id = session.Id,
                TrackId = session.TrackId,
                Title = session.Title,
                Abstract = session.Abstract,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Session updated successfully!";

            await _apiClient.PutSessionAsync(Session);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var session = await _apiClient.GetSessionAsync(id);

            if (session != null)
            {
                await _apiClient.DeleteSessionAsync(id);
            }

            Message = "Session deleted successfully!";

            return RedirectToPage("/Index");
        }
    }
}
