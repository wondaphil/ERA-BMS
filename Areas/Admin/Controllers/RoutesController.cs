using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoutesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Routes
        public ActionResult Index()
        {
            return View();
        }

        ////////////////////////////////////
        // "Main Route" start here ...
        ////////////////////////////////////
        public ActionResult MainRoutes(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<MainRoute> mainRoutesList = db.MainRoutes.ToList();

            return PartialView(mainRoutesList);
        }

        public string LastMainRouteId()
        {
            //// MainRouteId is a GUID value
            //// When a new main route is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string
            ///
            string assignedMainRouteId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"MainRouteId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"MainRouteId\": " + "\"" + assignedMainRouteId + "\"}]";
        }

        // GET: Admin/Routes/NewMainRoute
        public ActionResult NewMainRoute()
        {
            ViewBag.RoadClassId = new SelectList(db.RoadClasses, "RoadClassId", "RoadClassName");

            return View();
        }

        // POST: Admin/Routes/NewMainRoute
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMainRoute([Bind(Include = "MainRouteId,MainRouteNo,MainRouteName,Length,RoadClassId,Remark")] MainRoute mainRoute)
        {
            if (ModelState.IsValid)
            {
                db.MainRoutes.Add(mainRoute);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx 
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 4 }); // id = 4 for duplicate main route error
                }
                return RedirectToAction("MainRouteDetails", new { id = mainRoute.MainRouteId });
            }

            ViewBag.RoadClassId = new SelectList(db.RoadClasses, "RoadClassId", "RoadClassName", mainRoute.RoadClassId);

            return View(mainRoute);
        }

        // GET: Admin/Routes/DeleteMainRoute/A1
        public ActionResult DeleteMainRoute(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainRoute mainRoute = db.MainRoutes.Find(id);
            if (mainRoute == null)
            {
                return HttpNotFound();
            }
            return View(mainRoute);
        }

        // POST: Admin/Routes/DeleteMainRoute/A1
        [HttpPost, ActionName("DeleteMainRoute")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedMainRoute(string id)
        {
            MainRoute mainRoute = db.MainRoutes.Find(id);
            db.MainRoutes.Remove(mainRoute);
            try
            {
                db.SaveChanges();
            }
            catch (Exception) // catches all exceptions
            {
                return RedirectToAction("DeleteError", "ErrorHandler", new { id = 4 }); // id = 4 for main route delete error
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateMainRouteNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid main route no
            if (value != "")
            {
                MainRoute mainroute = db.MainRoutes.Find(id);

                mainroute.MainRouteNo = value;
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateMainRouteName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid main route name
            if (value != "")
            {
                MainRoute mainroute = db.MainRoutes.Find(id);

                mainroute.MainRouteName = value;
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateMainRouteLength(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid length
            if (value.All(Char.IsDigit))
            {
                MainRoute mainroute = db.MainRoutes.Find(id);

                mainroute.Length = Convert.ToInt32(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateMainRouteRemark(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            MainRoute mainroute = db.MainRoutes.Find(id);

            mainroute.Remark = value;
            try
            {
                db.SaveChanges();

                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateMainRouteRoadClass(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            MainRoute mainRoute = (from c in db.MainRoutes
                                   where c.MainRouteId == id
                                   select c).FirstOrDefault();

            RoadClass roadClass = (from c in db.RoadClasses
                                   where c.RoadClassName == value
                                   select c).FirstOrDefault();

            mainRoute.RoadClassId = roadClass.RoadClassId;

            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectRoadClass()
        {
            List<RoadClass> roadClass = db.RoadClasses.ToList();
            var list = roadClass.Select(d => new[] { d.RoadClassName, d.RoadClassName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        ////////////////////////////////////
        // "Sub Route" start here ...
        ////////////////////////////////////
        // GET: Admin/Routes/SubRouteDetails/A1
        public ActionResult SubRoutes()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList(); // For a dropdownlist that has a list of all main routes

            return PartialView();
        }

        public ActionResult _GetSubRouteList(string mainrouteid, StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<SubRoute> subRouteList = (from s in db.SubRoutes
                                           where s.MainRouteId == mainrouteid
                                           select s).ToList();
            ViewBag.MainRouteName = db.MainRoutes.Find(mainrouteid).MainRouteName.ToString();

            return PartialView(subRouteList);
        }

        public string LastSubRouteId()
        {
            //// SubRouteId is a GUID value
            //// When a new sub route is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string
            ///
            string assignedSubRouteId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"SubRouteId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"SubRouteId\": " + "\"" + assignedSubRouteId + "\"}]";
        }

        // GET: Admin/Routes/NewSubRoute
        public ActionResult NewSubRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList(); // For a dropdownlist that has a list of all main routes

            return View();
        }

        // POST: Admin/Routes/NewSubRoute
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewSubRoute([Bind(Include = "SubRouteId,SubRouteNo,SubRouteName,MainRouteId,FromKm,ToKm,Length,ADT,Remark")] SubRoute subRoute)
        {
            if (ModelState.IsValid)
            {
                db.SubRoutes.Add(subRoute);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 5 }); // id = 5 for duplicate sub route error
                }
                return RedirectToAction("SubRouteDetails", new { id = subRoute.SubRouteId });
            }
            ViewBag.MainRouteId = new SelectList(db.MainRoutes, "MainRouteId", "MainRouteName", subRoute.MainRouteId);

            return View(subRoute);
        }

        // GET: Admin/Routes/DeleteSubRoute/A1
        public ActionResult DeleteSubRoute(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubRoute subRoute = db.SubRoutes.Find(id);
            if (subRoute == null)
            {
                return HttpNotFound();
            }
            return View(subRoute);
        }

        // POST: Admin/Routes/DeleteSubRoute/A1
        [HttpPost, ActionName("DeleteSubRoute")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedSubRoute(string id)
        {
            SubRoute subRoute = db.SubRoutes.Find(id);
            db.SubRoutes.Remove(subRoute);
            try
            {
                db.SaveChanges();
            }
            catch (Exception) // catches all exceptions
            {
                return RedirectToAction("DeleteError", "ErrorHandler", new { id = 5 }); // id = 5 for sub route delete error
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateSubRouteNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid main route no
            if (value != "")
            {
                SubRoute subroute = db.SubRoutes.Find(id);

                subroute.SubRouteNo = value;
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateSubRouteName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid main route name
            if (value != "")
            {
                SubRoute subroute = db.SubRoutes.Find(id);

                subroute.SubRouteName = value;
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateSubRouteFromKm(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid km measurement
            if (value.All(Char.IsDigit))
            {
                SubRoute subroute = db.SubRoutes.Find(id);

                subroute.FromKm = Convert.ToInt32(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateSubRouteToKm(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid km measurement
            if (value.All(Char.IsDigit))
            {
                SubRoute subroute = db.SubRoutes.Find(id);

                subroute.ToKm = Convert.ToInt32(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateSubRouteLength(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid length
            if (value.All(Char.IsDigit))
            {
                SubRoute subroute = db.SubRoutes.Find(id);

                subroute.Length = Convert.ToInt32(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateSubRouteADT(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            SubRoute subroute = db.SubRoutes.Find(id);

            // Check if value is a count
            if (value.All(Char.IsDigit))
            {
                subroute.AverageDailyTraffic = Convert.ToInt32(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSubRouteRemark(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            SubRoute subroute = db.SubRoutes.Find(id);

            subroute.Remark = value;
            try
            {
                db.SaveChanges();

                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateSubRouteMainRouteId(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            SubRoute subRoute = db.SubRoutes.Find(id);

            MainRoute mainRoute = (from c in db.MainRoutes
                                   where c.MainRouteNo + " - " + c.MainRouteName == value
                                   select c).FirstOrDefault();

            subRoute.MainRouteId = mainRoute.MainRouteId;

            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectMainRoute()
        {
            List<MainRoute> mainRoute = db.MainRoutes.ToList();
            var list = mainRoute.Select(d => new[] { d.MainRouteNo + " - " + d.MainRouteName, d.MainRouteNo + " - " + d.MainRouteName });

            return this.Json(list.OrderBy(arr => arr[0]), JsonRequestBehavior.AllowGet);
        }
    }
}