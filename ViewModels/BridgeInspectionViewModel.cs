using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.ViewModels
{
    public class BridgeInspectionViewModel
    {
        public string BridgeId { get; set; }
        public int InspectionYear { get; set; }
    }

    public class DamageInspectionModel
    {
        public BridgeInspectionViewModel SelectedBridge;
        public int GirderTypeId;
        private BMSEntities db;
        public List<DamageInspMajor> MajorInspectionList;
        public List<DamageRateAndCost> RateAndCostList;
        public List<StructureItem> StructureItemsList;
        public List<ResultInspMajor> ResultList;

        public DamageInspectionModel(string bridgeid, int inspectionyear)
        {
            db = new BMSEntities();
            SelectedBridge = new BridgeInspectionViewModel();
            SelectedBridge.BridgeId = bridgeid;
            SelectedBridge.InspectionYear = inspectionyear;
            GirderTypeId = db.SuperStructures.Find(bridgeid).GirderType.GirderTypeId;

            MajorInspectionList = db.DamageInspMajors.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            RateAndCostList = db.DamageRateAndCosts.ToList();
            StructureItemsList = db.StructureItems.ToList();
            ResultList = db.ResultInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspectionyear).ToList();
        }

        public double CalculateStructureItemDamage(int stritemid)
        {
            // GirderType == 1 for Concrete and GirderType == 2 for Steel
            // stritemid == 6 (Concrete Girder / Arch) has to be ignored for Steel bridge
            // stritemid == 7 (Steel Truss / Girder) has to be ignored for Concrete bridge
            if ((stritemid == 6 && GirderTypeId == 2) || (stritemid == 7 && GirderTypeId == 1))
                return 0.0;

            // Get a list of damage major inspection for the given bridgeid, inspection year and structure item id
            // Get a list of damage rate and cost for the given structure item id
            List<DamageInspMajor> dmgInspMajor = MajorInspectionList.Where(s => s.StructureItemId == stritemid).ToList();
            List<DamageRateAndCost> dmgRateCost = RateAndCostList.Where(s => s.StructureItemId == stritemid).ToList();

            // Sum up damage rate for each damage type (e.g. Cracking, Void, etc) and damage rank (e.g. A+, A, B, etc...)
            double rate = 0.0;
            foreach (var insp in dmgInspMajor)
            {
                rate += (double)dmgRateCost.Where(rt => rt.DamageTypeId == insp.DamageTypeId && rt.DamageRankId == insp.DamageRankId).FirstOrDefault().DamagePercentValue;
            }
            return rate;
        }

        public double CalculateStructureItemCost(int stritemid)
        {
            // GirderType == 1 for Concrete and GirderType == 2 for Steel
            // stritemid == 6 (Concrete Girder / Arch) has to be ignored for Steel bridge
            // stritemid == 7 (Steel Truss / Girder) has to be ignored for Concrete bridge
            if ((stritemid == 6 && GirderTypeId == 2) || (stritemid == 7 && GirderTypeId == 1))
                return 0.0;

            // Get a list of damage major inspection for the given bridgeid, inspection year and structure item id
            // Get a list of damage rate and cost for the given structure item id
            List<DamageInspMajor> dmgInspMajor = MajorInspectionList.Where(s => s.StructureItemId == stritemid).ToList();
            List<DamageRateAndCost> dmgRateCost = RateAndCostList.Where(s => s.StructureItemId == stritemid).ToList();

            // Sum up damage cost (unit cost times damage area) for each damage type (e.g. Cracking, Void, etc) and damage rank (e.g. A+, A, B, etc...)
            double cost = 0.0;
            foreach (var insp in dmgInspMajor)
            {
                cost += (double)(dmgRateCost.Where(cst => cst.DamageTypeId == insp.DamageTypeId && cst.DamageRankId == insp.DamageRankId).FirstOrDefault().UnitRepairCost * insp.DamageArea);
            }
            return cost;
        }

        /// <summary>
        /// Calculates total damage rate of the given bridge part by summing up damage rates of structure items in the bridge part
        /// </summary>
        /// <param name="brgpartid">Bridge Part Id (1, 2 or 3)</param>
        /// <returns></returns>
        public double CalculateBridgePartDamageRate(int brgpartid)
        {
            // Get a list of structure items for the given bridge part
            //     e.g. BridgePart=1 => structure items=1,2,3,4
            //          BridgePart=2 => structure items=5,6,7
            //          BridgePart=3 => structure items=8,9,10,11,12
            List<StructureItem> strItem = StructureItemsList.Where(s => s.BridgePartId == brgpartid).ToList();
            double rate = 0.0;

            // Sum up damage rate for each structure item in the given bridge part
            foreach (var str in strItem)
            {
                rate += CalculateStructureItemDamage(str.StructureItemId);
            }

            // Get a list of bridge part damage weight
            List<BridgePartDmgWt> brgPartDmgWt = db.BridgePartDmgWts.ToList();

            // Divide rate by the corresponding bridge part damage weight (weight is in percentile form so multiply by 100)
            //rate = (rate / brgPartDmgWt.Where(s => s.BridgePartId == brgpartid).FirstOrDefault().DmgWt) * 100;

            return rate;
        }

        /// <summary>
        /// Calculates total damage cost of the given bridge part by summing up damage costs of structure items in the bridge part
        /// </summary>
        /// <param name="brgpartid">Bridge Part Id (1, 2 or 3)</param>
        /// <returns></returns>
        public double CalculateBridgePartDamageCost(int brgpartid)
        {
            // Get a list of structure items for the given bridge part
            //     e.g. BridgePart=1 => structure items=1,2,3,4
            //          BridgePart=2 => structure items=5,6,7
            //          BridgePart=3 => structure items=8,9,10,11,12
            List<StructureItem> strItem = StructureItemsList.Where(s => s.BridgePartId == brgpartid).ToList();
            double cost = 0.0;

            // Sum up damage cost for each structure item in the given bridge part
            foreach (var str in strItem)
            {
                cost += CalculateStructureItemCost(str.StructureItemId);
            }

            return cost;
        }
    }
}