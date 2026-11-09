
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;

namespace RaceDayMvc.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _api;

        public OrganiserController(ApiService api)
        {
            _api = api;
        }

        private bool IsOrganiser()
        {
            return string.Equals(
                HttpContext.Session.GetString("Role"),
                "organiser",
                StringComparison.OrdinalIgnoreCase);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            return View(new CreateEventViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(
            CreateEventViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventData = new
            {
                eventName = model.EventName,
                description = model.Description,
                eventDate = model.EventDate.ToString("yyyy-MM-dd"),
                location = model.Location,
                distanceKm = model.Distance,
                eventType = model.EventType
            };

            try
            {
                var response = await _api.PostAsync(
                    "api/Events",
                    eventData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Event created successfully!";

                    return RedirectToAction("Dashboard");
                }

                var error =
                    await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Could not create event. API response: " + error);

                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API. Make sure it is running.");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory(int eventId)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var categories = await _api.GetAsync<Category>(
                    "api/Events/" + eventId + "/categories");

                ViewBag.Categories = categories;
            }
            catch (HttpRequestException)
            {
                ViewBag.Categories = new List<Category>();
                ModelState.AddModelError(
                    string.Empty,
                    "Could not load categories. Make sure the API is running.");
            }

            return View(new CategoryViewModel
            {
                EventID = eventId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(
            CategoryViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new List<Category>();
                return View(model);
            }

            var categoryData = new
            {
                categoryName = model.CategoryName,
                categoryType = model.CategoryType,
                minAge = model.MinAge,
                maxAge = model.MaxAge,
                distanceKm = model.DistanceKm,
                description = model.Description
            };

            try
            {
                var response = await _api.PostAsync(
                    "api/Events/" + model.EventID + "/categories",
                    categoryData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Category created successfully!";

                    return RedirectToAction(
                        "CreateCategory",
                        new { eventId = model.EventID });
                }

                var error =
                    await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Could not create category. API response: " + error);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API. Make sure it is running.");
            }

            ViewBag.Categories = new List<Category>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EventEnrolments(int eventId)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var enrolments = await _api.GetAsync<OrganiserEnrolmentViewModel>(
                    "api/events/" + eventId + "/enrolments");

                ViewBag.EventID = eventId;

                return View(enrolments);
            }
            catch (HttpRequestException)
            {
                TempData["ErrorMessage"] =
                    "Could not load event enrolments. Please check that the API is running.";

                return View(new List<OrganiserEnrolmentViewModel>());
            }
        }
        [HttpGet]
        public IActionResult RecordResult(int enrolmentId)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new CreateResultViewModel
            {
                EnrolmentID = enrolmentId,
                ResultStatus = "Completed"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordResult(
            CreateResultViewModel model)
        {
            if (!IsOrganiser())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultData = new
            {
                finishTime = model.FinishTime,
                position = model.Position,
                resultStatus = model.ResultStatus
            };

            try
            {
                var response = await _api.PostAsync(
                    "api/enrolments/" + model.EnrolmentID + "/result",
                    resultData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Race result recorded successfully!";

                    return RedirectToAction("Dashboard");
                }

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Could not record result: " + error);

                return View(model);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API. Make sure it is running.");

                return View(model);
            }
        }
    }
}


