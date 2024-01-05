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
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class DamageWeightAndCostRateController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/DamageWeightAndCostRate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BridgePartDmgWt()
        {
            var bridgePartDmgWts = db.BridgePartDmgWts.Include(b => b.BridgePart);
            return PartialView(bridgePartDmgWts.ToList());
        }

        [HttpPost]
        public ActionResult UpdateBridgePartDmgWt(BridgePartDmgWt bridgePartDmgWt)
        {
            BridgePartDmgWt updatedBridgePartDmgWt = (from c in db.BridgePartDmgWts
                                                      where c.BridgePartId == bridgePartDmgWt.BridgePartId
                                                      select c).FirstOrDefault();

            updatedBridgePartDmgWt.DmgWt = bridgePartDmgWt.DmgWt;
            updatedBridgePartDmgWt.Remark = bridgePartDmgWt.Remark;

            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult StructureItemDmgWt()
        {
            var structureItemDmgWts = db.StructureItemDmgWts.Include(s => s.StructureItem);

            ViewBag.StructureItems = db.StructureItems;
            ViewBag.BridgeParts = db.BridgeParts;

            return PartialView(structureItemDmgWts.ToList());
        }

        [HttpPost]
        public ActionResult UpdateStructureItemDmgWt(StructureItemDmgWt structureItemDmgWt)
        {
            StructureItemDmgWt updatedStructureItemDmgWt = (from c in db.StructureItemDmgWts
                                                            where c.StructureItemId == structureItemDmgWt.StructureItemId
                                                            select c).FirstOrDefault();

            updatedStructureItemDmgWt.ConcreteDmgWt = structureItemDmgWt.ConcreteDmgWt;
            updatedStructureItemDmgWt.SteelDmgWt = structureItemDmgWt.SteelDmgWt;
            updatedStructureItemDmgWt.BridgePartDmgWt = structureItemDmgWt.BridgePartDmgWt;
            updatedStructureItemDmgWt.TotalStructureDmgWt = structureItemDmgWt.TotalStructureDmgWt;
            updatedStructureItemDmgWt.Remark = structureItemDmgWt.Remark;

            db.SaveChanges();

            return new EmptyResult();
        }

        // GET: Admin/DamageRanges
        public ActionResult DamageConditionRange()
        {
            return PartialView(db.DamageConditionRanges.ToList());
        }

        [HttpPost]
        public ActionResult UpdateDamageConditionRange(DamageConditionRange damageConditionRange)
        {
            DamageConditionRange updatedDamageConditionRange = (from c in db.DamageConditionRanges
                                                                where c.DamageConditionId == damageConditionRange.DamageConditionId
                                                                select c).FirstOrDefault();

            updatedDamageConditionRange.DamageConditionName = damageConditionRange.DamageConditionName;
            updatedDamageConditionRange.ValueFrom = damageConditionRange.ValueFrom;
            updatedDamageConditionRange.ValueTo = damageConditionRange.ValueTo;
            updatedDamageConditionRange.Action = damageConditionRange.Action;
            updatedDamageConditionRange.Remark = damageConditionRange.Remark;

            db.SaveChanges();

            return new EmptyResult();
        }

        // GET: Admin/DamageRateAndCosts
        public ActionResult DamageRateAndCost()
        {
            List<SelectListItem> structureItemList = db.StructureItems.Select(c => new SelectListItem
            {
                Text = c.StructureItemId.ToString() + " - " + c.StructureItemName,
                Value = c.StructureItemId.ToString()
            }
                                                            ).ToList();

            //Add the first unselected item
            structureItemList.Insert(0, new SelectListItem { Text = "--Select Structure Item--", Value = "0" });
            ViewBag.StructureItems = structureItemList;

            return PartialView();
        }

        public ActionResult _GetDamageType(int structureItemid /* drop down value */)
        {
            List<DamageType> damageTypeList = db.DamageTypes.Where(s => s.StructureItemId == structureItemid).ToList();

            //return SelectList item in Json format
            //e.g. [{"Disabled":false,"Group":null,"Selected":false,"Text":"Sodo","Value":"100"},
            //      {"Disabled":false,"Group":null,"Selected":false,"Text":"Konso","Value":"111"}]
            return this.Json(new SelectList(damageTypeList.ToArray(), "DamageTypeId", "DamageTypeName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetDamageWeight(int damageTypeid, int structureItemid)
        {
            List<DamageRateAndCost> damageRateAndCostList = (from s in db.DamageRateAndCosts
                                                             where (s.DamageTypeId == damageTypeid) && (s.StructureItemId == structureItemid) && (s.DamageRankId != 5)
                                                             select s).OrderBy(x => x.DamageRank.DamageRankId).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(damageRateAndCostList);
        }

        [HttpPost]
        public ActionResult UpdateDamageRateAndCost(DamageRateAndCost dmgRateAndCost)
        {
            DamageRateAndCost updatedDmgRateAndCost = (from c in db.DamageRateAndCosts
                                                       where c.Id == dmgRateAndCost.Id
                                                       select c).FirstOrDefault();

            updatedDmgRateAndCost.Unit = dmgRateAndCost.Unit;
            updatedDmgRateAndCost.DamagePercentValue = dmgRateAndCost.DamagePercentValue;
            updatedDmgRateAndCost.UnitRepairCost = dmgRateAndCost.UnitRepairCost;
            updatedDmgRateAndCost.Description = dmgRateAndCost.Description;

            db.SaveChanges();

            return new EmptyResult();
        }
    }
}