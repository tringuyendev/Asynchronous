using Deadlock.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Deadlock.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var githubTask = await GetGitHubStringAsync();
            var githubString = githubTask.ToString();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static async Task<object> GetGitHubStringAsync()
        {
            using (var client = new HttpClient())
            {
                // Technically does a 403, but that doesn't matter for our case as all we need is a HTTP response
                var githubResult = await client.GetStringAsync(@"https://api.github.com/zen").ConfigureAwait(false);
                return githubResult;
            }
        }
    }
}