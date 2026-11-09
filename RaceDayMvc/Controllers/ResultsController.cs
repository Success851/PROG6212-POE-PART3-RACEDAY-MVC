
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;

namespace RaceDayMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApiService _api;

        public ResultsController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "participant")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var results = await _api.GetAsync<ParticipantResult>(
                    "api/Results/my-results");

                return View(results);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode ==
                    System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                ViewBag.ErrorMessage =
                    "Unable to load your race results.";

                return View(new List<ParticipantResult>());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage =
                    "An error occurred while loading your results.";

                return View(new List<ParticipantResult>());
            }
        }
    }
}
