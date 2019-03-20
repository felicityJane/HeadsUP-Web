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
    public class DataController : Controller
    {
        [CustomAuthorizeRedirect(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            List<SensorData> data = null;

            using (var client = new HttpClient())
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
                string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                // string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

                client.BaseAddress = new Uri("http://headsupapi.azurewebsites.net/sensor/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                HttpResponseMessage response = await client.GetAsync("getsensordata");
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsAsync<List<SensorData>>();
                }
                else
                {
                    return View("Error");
                }
            }

            return View(data);
        }

        [CustomAuthorizeRedirect(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
                string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                // string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

                client.BaseAddress = new Uri("http://headsupapi.azurewebsites.net/sensor/deletesensordata/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string userId = id.ToString();

                HttpResponseMessage response = await client.DeleteAsync(userId);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { status = 201, responseMessage = "Entry " + id + " removed from sensor data table" });
                }
                return Json(new { status = 500, responseMessage = "An error occured" });
            }      
        }

        [CustomAuthorizeRedirect(Roles = "admin")]
        public async Task<ActionResult> DeleteAll(string userId)
        {
            using (var client = new HttpClient())
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                // Extract tokens
                string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
                // string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

                client.BaseAddress = new Uri("http://headsupapi.azurewebsites.net/deletesensordataUser/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                HttpResponseMessage response = await client.DeleteAsync(userId);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { status = 201, responseMessage = "All entries removed for " + userId + " from sensor data table" });
                }
                return Json(new { status = 500, responseMessage = "An error occured" });
            }
        }
    }
}