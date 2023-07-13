using Kube.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Kube.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<WeatherForecast> data = new List<WeatherForecast>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["KubeApiurl"]);
                var response = await client.GetAsync("/WeatherForecast");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
            }
            return View(data);
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
    }
}