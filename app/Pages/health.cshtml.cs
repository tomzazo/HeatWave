using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace HeatWave.Pages
{
    class Health
    {
        public required string Status { get; set; }
    }

    [AllowAnonymous]
    public class HealthModel : PageModel
    {
        public JsonResult OnGet()
        {
            return new JsonResult(JsonConvert.SerializeObject(new Health() { Status = "ok" }));
        }
    }
}
