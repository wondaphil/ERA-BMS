using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using Newtonsoft.Json;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class RoadFunctionClassController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: RoadClass
        public ActionResult Index()
        {
            List <RoadClass> roadClasses = db.RoadClasses.ToList();
                              
            return View(roadClasses);
        }

        public ActionResult UpdateRoadFunctionClass(RoadClass rdClass)
        {
            RoadClass roadClass = db.RoadClasses.Find(rdClass.RoadClassId);

            roadClass.RoadClassName = rdClass.RoadClassName;
            roadClass.Remark = rdClass.Remark;
            
            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult DeleteRoadFunctionClass(int id)
        {
            RoadClass roadClass = db.RoadClasses.Find(id);
            if (roadClass != null)
            {
                db.RoadClasses.Remove(roadClass);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        public ActionResult NewRoadFunctionClassPartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRoadFunctionClassPartial([Bind(Include = "RoadClassId,RoadClassName,Remark")] RoadClass rdClass)
        {
            if (ModelState.IsValid)
            {
                db.RoadClasses.Add(rdClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rdClass);
        }

        public string LastRoadFunctionClassId()
        {
            // increment one to the last road class id to get new road class id
            int newRoadClassId = db.RoadClasses.Select(s => s.RoadClassId).Max() + 1;

            // The returned result is a json string like [{"RoadClassId": 6}]
            return "[{\"RoadClassId\": " + newRoadClassId + "}]";
        }
    }
}