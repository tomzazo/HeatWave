using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeatWave.Pages
{
    [Authorize]
    public class HistoryModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
