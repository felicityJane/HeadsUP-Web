using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcApplication.Model;

namespace MvcApplication.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorizeRedirect(Roles = "admin")]
        public async Task<ActionResult> UserData()
        {
            List<User> user = null;

            using (var client = new HttpClient())
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
                string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                //string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

                client.BaseAddress = new Uri("http://localhost:54111/user/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                HttpResponseMessage response = await client.GetAsync("getuser");
                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadAsAsync<List<User>>();
                }
                else
                {
                    return View("Error");
                }
            }

            return View(user);
        }

        [CustomAuthorizeRedirect(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
        {
            using (var client = new HttpClient())
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
                string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                // string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

                client.BaseAddress = new Uri("http://localhost:54111/user/deleteuser/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                HttpResponseMessage response = await client.DeleteAsync(id);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { status = 201, responseMessage = "User " + id + " removed from user table" });
                }
                return Json(new { status = 500, responseMessage = "An error occured" });
            }
        }

    }
}