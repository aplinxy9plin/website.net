﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActiveCitizenWeb.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is Active Citizen";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Test3";

            return View();
        }
    }
}