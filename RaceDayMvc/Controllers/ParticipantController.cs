
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;
using System.Net;

namespace RaceDayMvc.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsParticipant()
        {
            string? role = HttpContext.Session.GetString("Role");

            return string.Equals(
                role,
                "participant",
                StringComparison.OrdinalIgnoreCase);
        }

        // GET: /Participant/Profile
        public async Task<IActionResult> Profile()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var profile =
                    await _apiService.GetSingleAsync<ParticipantProfileViewModel>(
                        "api/Participants/profile");

                if (profile == null)
                {
                    TempData["Error"] = "Participant profile could not be found.";
                    return RedirectToAction("Index", "Events");
                }

                return View(profile);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                TempData["Error"] = "Unable to load your profile.";
                return RedirectToAction("Index", "Events");
            }
        }

        // GET: /Participant/EditProfile
        public async Task<IActionResult> EditProfile()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var profile =
                    await _apiService.GetSingleAsync<ParticipantProfileViewModel>(
                        "api/Participants/profile");

                if (profile == null)
                {
                    return RedirectToAction("Profile");
                }

                var model = new UpdateParticipantProfileViewModel
                {
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Email = profile.Email,
                    Phone = profile.Phone
                };

                return View(model);
            }
            catch
            {
                TempData["Error"] = "Unable to load your profile for editing.";
                return RedirectToAction("Profile");
            }
        }

        // POST: /Participant/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(
            UpdateParticipantProfileViewModel model)
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _apiService.PutAsync(
                    "api/Participants/profile",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Your profile was updated successfully.";
                    return RedirectToAction("Profile");
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                var error = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    "",
                    string.IsNullOrWhiteSpace(error)
                        ? "Unable to update your profile."
                        : error);

                return View(model);
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "A connection error occurred while updating your profile.");

                return View(model);
            }
        }

        // GET: /Participant/Enrolments
        public async Task<IActionResult> Enrolments()
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var enrolments =
                    await _apiService.GetAsync<ParticipantEnrolmentViewModel>(
                        "api/Participants/enrolments");

                return View(enrolments);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                TempData["Error"] = "Unable to load your enrolments.";
                return View(new List<ParticipantEnrolmentViewModel>());
            }
        }

        // POST: /Participant/CancelEnrolment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrolment(int enrolmentId)
        {
            if (!IsParticipant())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiService.DeleteAsync(
                    "api/Enrolments/" + enrolmentId);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Your enrolment was cancelled successfully.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    TempData["Error"] =
                        "Unable to cancel enrolment: " + error;
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "Could not connect to the API.";
            }

            return RedirectToAction("Enrolments");
        }
    }
}
