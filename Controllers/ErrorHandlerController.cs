using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERA_BMS.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErrorHandler
        public ActionResult Index(int id)
        {
            Response.StatusCode = id; 
            
            return View();
        }

        public ActionResult DeleteError(int id)
        {
            ViewBag.ErrorCode = id;

            return View();
        }

        public ActionResult DuplicateError(string id)
        {
            ViewBag.ErrorCode = id;

            return View();
        }
    }
}