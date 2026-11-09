
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;

namespace RaceDayMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _api;

        public EventsController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var eventsList = await _api.GetAsync<Event>("api/Events");

            var categoriesByEvent = new Dictionary<int, List<Category>>();

            foreach (var item in eventsList)
            {
                var categories = await _api.GetAsync<Category>(
                    "api/Events/" + item.EventID + "/categories");

                categoriesByEvent[item.EventID] = categories;
            }

            ViewBag.CategoriesByEvent = categoriesByEvent;

            return View(eventsList);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            var response = await _api.PostAsync(
                "api/Events/" + eventId + "/enrolments",
                new { categoryID = categoryId });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] =
                    "You have successfully enrolled in the event.";
            }
            else
            {
                var errorMessage =
                    await response.Content.ReadAsStringAsync();

                TempData["ErrorMessage"] =
                    "Enrolment failed. API response: " + errorMessage;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var eventItem = await _api.GetSingleAsync<Event>(
                "api/Events/" + id);

            if (eventItem == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("Index");
            }

            var model = new UpdateEventViewModel
            {
                EventID = eventItem.EventID,
                EventName = eventItem.EventName,
                Description = eventItem.Description,
                EventDate = eventItem.EventDate,
                Location = eventItem.Location,
                DistanceKm = Convert.ToDecimal(eventItem.DistanceKm),
                EventType = eventItem.EventType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEventViewModel model)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateData = new
            {
                eventName = model.EventName,
                description = model.Description,
                eventDate = model.EventDate,
                location = model.Location,
                distanceKm = model.DistanceKm,
                eventType = model.EventType
            };

            try
            {
                var response = await _api.PutAsync(
                    "api/Events/" + model.EventID,
                    updateData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Event updated successfully!";
                    return RedirectToAction("Index");
                }

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Could not update event: " + error);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _api.DeleteAsync(
                    "api/Events/" + id);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Event deleted successfully!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    TempData["ErrorMessage"] =
                        "Could not delete event: " + error;
                }
            }
            catch (HttpRequestException)
            {
                TempData["ErrorMessage"] =
                    "Could not connect to the API.";
            }

            return RedirectToAction("Index");
        }

    }
}
