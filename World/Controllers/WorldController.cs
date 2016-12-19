using System;
using Microsoft.AspNetCore.Mvc;

namespace World
{
    [Route("/")]
    public class WorldController : Controller
    {
        public JsonResult Index()
        {
            var ip = Environment.GetEnvironmentVariable("POD_IP");

            var hostname = Environment.GetEnvironmentVariable("NODE_NAME");

            return Json($"World (pod ip : {ip}) (host ip : {hostname})");
        }
    }
}
