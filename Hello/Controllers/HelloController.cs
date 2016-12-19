using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hello.Controllers
{
    public class HelloController : Controller
    {
        string nodeAddress;
        string podAddress;
        public HelloController()
        {
            var nodeName = Environment.GetEnvironmentVariable("NODE_NAME");
            var podAddress = Environment.GetEnvironmentVariable("POD_IP");
            nodeAddress = $"{nodeName}:4140";
        }

        [Route("/")]
        public JsonResult Hello()
        {
            string worldResponse;

            worldResponse = GetWorldResponse().Result;

            return Json($"Hello (podIp : {podAddress}) (hostName : {nodeAddress}) Calling world service on {nodeAddress} {worldResponse}");
        }

        private async Task<string> GetWorldResponse()
        {
            var client = GetClientWithForwardedHeaders();

            client.DefaultRequestHeaders.Add("HOST", "world");

            HttpResponseMessage response;

            string message;

            try
            {
                response = await client.GetAsync($"http://{nodeAddress}").ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                message = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                message = $"{ex.Data.ToString()} {ex.Message} {ex.Source}";
            }
            return message;
        }

        //some examples show that all l5d headers need to be forwarded on
        private HttpClient GetClientWithForwardedHeaders()
        {
            var client = new HttpClient();

            foreach (var header in Request.Headers.Where(h => h.Key.Contains("l5d-")))
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value.ToList());
            }

            return client;
        }
    }
}
