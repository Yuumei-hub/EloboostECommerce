using Microsoft.AspNetCore.Mvc;

namespace EloboostCommerce.Controllers
{
    public class AdminController : Controller
    {

        private const string APIKey = "c4d702674af4f5f2706ed116b9687a46";
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.jotform.com/form/240317972313958/submissions")
            };
        }

        public async Task<ActionResult> GetFormSubmissions(string formId)
        {
            var requestUri = $"form/{formId}/submissions?apiKey={APIKey}";

            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            else
            {
                return Content("Error fetching form submissions", "text/plain");
            }
        }

    }
}
