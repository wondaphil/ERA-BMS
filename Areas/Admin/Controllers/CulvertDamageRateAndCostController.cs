using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity; using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;

namespace ERA_BMS.Areas.Admin.Controllers
{
    public class CulvertDamageRateAndCostController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/CulvertDamageTypeAndUnitRate
        public ActionResult Index()
        {
            List<SelectListItem> culStructureItemList = db.culStructureItems.Select(c => new SelectListItem
                                                                {
                                                                    Text = c.StructureItemId.ToString() + " - " + c.StructureItemName,
                                                                    Value = c.StructureItemId.ToString()
                                                                }
                                                            ).ToList();

            //Add the first unselected item
            culStructureItemList.Insert(0, new SelectListItem { Text = "--Select Structure Item--", Value = "0" });
            ViewBag.StructureItems = culStructureItemList;

            return View();
        }

        public ActionResult _GetDamageRateAndCost(int structureItemid)
        {
            List<culDamageRateAndCost> culStrDamageTypeList = (from s in db.culDamageRateAndCosts
                                                               where s.StructureItemId == structureItemid
                                                               select s).OrderBy(x => x.DamageTypeId).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(culStrDamageTypeList);
        }

        [HttpPost]
        public ActionResult UpdateDamageRateAndCost(culDamageRateAndCost culDmgRateAndCost)
        {
            culDamageRateAndCost updatedCulDmgRateAndCost = (from c in db.culDamageRateAndCosts
                                                             where c.DamageTypeId == culDmgRateAndCost.DamageTypeId
                                                             select c).FirstOrDefault();

            updatedCulDmgRateAndCost.Unit = culDmgRateAndCost.Unit;
            updatedCulDmgRateAndCost.DamageValue = culDmgRateAndCost.DamageValue;
            updatedCulDmgRateAndCost.UnitRepairCost = culDmgRateAndCost.UnitRepairCost;
            
            db.SaveChanges();

            return new EmptyResult();
        }
    }
}