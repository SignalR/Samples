using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class FeatureController : Controller
    {
        public ActionResult PersistentConnection()
        {
            return View();
        }

        public ActionResult Hub()
        {
            return View();
        }

        public ActionResult ConnectionManager()
        {
            return View();
        }

        public ActionResult HubT()
        {
            return View();
        }

        public ActionResult Progress()
        {
            return View();
        }

        [Authorize]
        public ActionResult SendToUser()
        {
            return View();
        }
    }
}