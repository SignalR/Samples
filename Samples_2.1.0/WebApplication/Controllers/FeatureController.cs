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

        public ActionResult EnableDetailedErrors()
        {
            return View();
        }

        public ActionResult Authorization()
        {
            return View();
        }

        public ActionResult AuthorizationFailedPersistentConnection()
        {
            return View("AuthorizationPersistentConnection");
        }

        public ActionResult AuthorizationFailedHub()
        {
            return View("AuthorizationHub");
        }

        [Authorize]
        public ActionResult AuthorizationPersistentConnection()
        {
            return View();
        }

        [Authorize]
        public ActionResult AuthorizationHub()
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

        public ActionResult HubException()
        {
            return View();
        }

        [Authorize]
        public ActionResult SendToUser()
        {
            return View();
        }

        [Authorize]
        public ActionResult SendToUsers()
        {
            return View();
        }
    }
}