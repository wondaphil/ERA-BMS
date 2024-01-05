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

namespace ERA_BMS.Controllers
{
    public class SearchController : Controller
    {
        private BMSEntities db = new BMSEntities();

        public ActionResult Index()
        {
            return View();
        }
        
        public string GetBridgeIdFromNumber(string revisedbridgeno)
        {
            Bridge bridge = db.Bridges.Where(br => (br.BridgeName + " [" + br.RevisedBridgeNo + "]") == revisedbridgeno).FirstOrDefault();
            
            if (bridge == null)
            {
                return "[{\"BridgeId\": \"\"}]";
            }

            // A json string of the form [{ "BridgeId": "A5F47F3C-4F84-486E-85A6-5E1A789474B1" }]
            return "[{\"BridgeId\": \"" + bridge.BridgeId + "\"}]";
        }

        public string GetBridgeIdFromName(string bridgename)
        {
            Bridge bridge = db.Bridges.Where(br => br.BridgeName == bridgename).FirstOrDefault();

            if (bridge == null)
            {
                return "[{\"BridgeId\": \"\"}]";
            }

            // A json string of the form [{ "BridgeId": "A5F47F3C-4F84-486E-85A6-5E1A789474B1" }]
            return "[{\"BridgeId\": \"" + bridge.BridgeId + "\"}]";
        }

        public string GetCulvertIdFromNumber(string revisedculvertno)
        {
            Culvert culvert = db.Culverts.Where(cul => cul.RevisedCulvertNo == revisedculvertno).FirstOrDefault();

            if (culvert == null)
            {
                return "[{\"CulvertId\": \"\"}]";
            }

            // A json string of the form [{ "CulvertId": "A5F47F3C-4F84-486E-85A6-5E1A789474B1" }]
            return "[{\"CulvertId\": \"" + culvert.CulvertId + "\"}]";
        }

        public ActionResult _FindBridge(string term)
        {
            Bridge bridge = db.Bridges.Where(br => (br.BridgeName + " [" + br.RevisedBridgeNo + "]") == term).FirstOrDefault();
            if (bridge == null)
            {
                ViewBag.Success = false;
                ViewBag.NoInventory = true;
            }
            else
            {
                var bridgeid = bridge.BridgeId;
                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeid);
                SuperStructure supertStr = db.SuperStructures.Find(bridgeid);
                if (genInfo == null || supertStr == null) // bridge exists but no or incomplete inventory data
                    ViewBag.NoInventory = true;
                else // bridge exists and has complete inventory data
                    ViewBag.NoInventory = false;

                ViewBag.Success = true;
            }

            return PartialView(bridge);
        }

        public ActionResult _FindCulvert(string term)
        {
            Culvert culvert = db.Culverts.Where(c => c.RevisedCulvertNo == term).FirstOrDefault();
            if (culvert == null)
            {
                ViewBag.Success = false;
                ViewBag.NoInventory = true;
            }
            else
            {
                var culvertid = culvert.CulvertId;
                CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(culvertid);
                CulvertStructure culvertStr = db.CulvertStructures.Find(culvertid);
                if (genInfo == null || culvertStr == null) // culvert exists but no or incomplete inventory data
                    ViewBag.NoInventory = true;
                else // culvert exists and has complete inventory data
                    ViewBag.NoInventory = false;

                ViewBag.Success = true;
            }

            return PartialView(culvert);
        }

        // Get a list of bridge numbers starting with term (for AutoComplete)
        //public ActionResult GetBridgeNoListContaining(string term)
        //{
        //    var result = from br in db.Bridges
        //                 where br.BridgeNo.ToLower().Contains(term)
        //                 select br.BridgeNo;

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        // Get a list of bridges whose name or id starting with term (for AutoComplete)
        public ActionResult GetBridgeListContaining(string term)
        {
            var result = from br in db.Bridges.OrderBy(b => b.BridgeName)
                         where br.BridgeName.ToLower().Contains(term) || br.BridgeNo.ToLower().Contains(term) || br.RevisedBridgeNo.ToLower().Contains(term) || br.BridgeId.ToLower().Contains(term) || br.RevisedBridgeNo.ToLower().Contains(term)
                         select (br.BridgeName + " [" + br.RevisedBridgeNo + "]");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Get a list of culvers whose name or id starting with term (for AutoComplete)
        public ActionResult GetCulvertListContaining(string term)
        {
            var result = from cul in db.Culverts
                         where cul.CulvertNo.ToLower().Contains(term) || cul.CulvertId.ToLower().Contains(term) || cul.RevisedCulvertNo.ToLower().Contains(term)
                         select cul.RevisedCulvertNo;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
