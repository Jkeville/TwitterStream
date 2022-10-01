﻿using Microsoft.AspNetCore.Mvc;

namespace Ingestion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
