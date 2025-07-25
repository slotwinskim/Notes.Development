using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync("http://localhost:5176/gyms");

        if (response.IsSuccessStatusCode)
        {
            var gyms = await response.Content.ReadFromJsonAsync<string[]>()
                .ToList();

            return View(gyms);
        }
        
        return Error();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
