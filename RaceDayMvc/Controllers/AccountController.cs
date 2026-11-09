
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDayMvc.Models;

namespace RaceDayMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AccountController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private HttpClient CreateApiClient()
        {
            string baseUrl =
                _configuration["ApiSettings:BaseUrl"]
                ?? "https://localhost:7245/";

            HttpClient client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl);

            string? apiCookie =
                HttpContext.Session.GetString("ApiSessionCookie");

            if (!string.IsNullOrWhiteSpace(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cookie", apiCookie);
            }

            return client;
        }


        private void SaveApiCookie(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues(
                "Set-Cookie", out var cookies))
            {
                foreach (string cookie in cookies)
                {
                    string cookiePair = cookie.Split(';')[0];

                    if (cookiePair.StartsWith(
                        ".AspNetCore.Session="))
                    {
                        HttpContext.Session.SetString(
                            "ApiSessionCookie", cookiePair);
                    }
                }
            }

            // Diagnostic information only; do not display cookie values.
            HttpContext.Session.SetString(
                "ApiCookieReceived",
                response.Headers.Contains("Set-Cookie")
                    ? "Yes"
                    : "No");
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                using HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.PostAsJsonAsync(
                        "api/Auth/login", request);

                SaveApiCookie(response);

                if (!response.IsSuccessStatusCode)
                {
                    string error =
                        await response.Content.ReadAsStringAsync();

                    ModelState.AddModelError(
                        string.Empty,
                        string.IsNullOrWhiteSpace(error)
                            ? "Login failed. Check your details."
                            : error.Trim('"'));

                    return View(request);
                }

                HttpContext.Session.SetString(
                    "Role", request.Role.ToLower());

                HttpContext.Session.SetString(
                    "Email", request.Email);

                string? apiCookie =
                    HttpContext.Session.GetString("ApiSessionCookie");

                if (string.IsNullOrWhiteSpace(apiCookie))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Login succeeded, but the API session cookie was not received. Check the API session configuration.");

                    return View(request);
                }

                if (request.Role.Equals(
                    "organiser",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(
                        "Dashboard", "Organiser");
                }

                return RedirectToAction("Index", "Events");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API. Make sure RaceDayTestAPI is running.");

                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                ModelState.AddModelError(
                    nameof(request.Phone),
                    "Phone number is required.");

                return View(request);
            }

            try
            {
                using HttpClient client = CreateApiClient();

                HttpResponseMessage response =
                    await client.PostAsJsonAsync(
                        "api/Auth/register", request);

                if (!response.IsSuccessStatusCode)
                {
                    string error =
                        await response.Content.ReadAsStringAsync();

                    ModelState.AddModelError(
                        string.Empty,
                        string.IsNullOrWhiteSpace(error)
                            ? "Registration failed."
                            : error.Trim('"'));

                    return View(request);
                }

                TempData["SuccessMessage"] =
                    "Registration successful. You can now log in.";

                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not connect to the API. Make sure RaceDayTestAPI is running.");

                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                using HttpClient client = CreateApiClient();

                await client.PostAsync("api/Auth/logout", null);
            }
            catch (HttpRequestException)
            {
                // Clear the local session even if the API is unavailable.
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}

