using ERA_BMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class DamagePriorityController : Controller
    {
        private BMSEntities db = new BMSEntities();
        
        // GET: Admin/DamagePriority
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CriteriaWeightsSum()
        {
            List<DamagePriorityCriteriaAndSum> dmgPriorityCriteriaList = db.DamagePriorityCriteriaAndSums.ToList();

            return PartialView(dmgPriorityCriteriaList);
        }

        public ActionResult CriteriaTypes()
        {
            List<SelectListItem> damagePriorityList = db.DamagePrioritizationCriterias.Select(c => new SelectListItem
                                                                        {
                                                                            Text = c.PrioritizationCriteriaName,
                                                                            Value = c.PrioritizationCriteriaId.ToString()
                                                                        }
                                                               ).ToList();

            //Add the first unselected item
            damagePriorityList.Insert(0, new SelectListItem { Text = "--Select Criteria--", Value = "0" });
            ViewBag.PrioritizationCriteria = damagePriorityList;

            return PartialView();
        }

        [HttpPost]
        public ActionResult UpdatePriority(DamagePriority dmgPriority)
        {
            DamagePriority updatedDmgPriority = (from c in db.DamagePriorities
                                                 where c.Id == dmgPriority.Id
                                                 select c).FirstOrDefault();

            updatedDmgPriority.DmgWtVal = dmgPriority.DmgWtVal;

            db.SaveChanges();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UpdatePriorityFromTo(DamagePriority dmgPriority)
        {
            DamagePriority updatedDmgPriority = (from c in db.DamagePriorities
                                                 where c.Id == dmgPriority.Id
                                                      select c).FirstOrDefault();

            updatedDmgPriority.DmgValFrom = dmgPriority.DmgValFrom;
            updatedDmgPriority.DmgValTo = dmgPriority.DmgValTo;
            updatedDmgPriority.DmgWtVal = dmgPriority.DmgWtVal;

            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult _GetADTDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();

            return PartialView(damagePriorityList);
        }

       public ActionResult _GetBridgeLengthDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();

            return PartialView(damagePriorityList);
        }

        public ActionResult _GetConstructionYearDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();

            return PartialView(damagePriorityList);
        }

        public ActionResult _GetDamagePercDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();

            return PartialView(damagePriorityList);
        }

        public ActionResult _GetRoadClassWeightDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).OrderBy(p => p.CriteriaSerNo).ToList();
            ViewBag.RoadClass = db.RoadClasses.ToList();
            
            return PartialView(damagePriorityList);
        }

        public ActionResult _GetMaintenanceUrgencyWeightDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();
            ViewBag.MaintenanceUrgency = db.MaintenanceUrgencies.ToList();
            
            return PartialView(damagePriorityList);
        }

        public ActionResult _GetYesNoWeightDetails(int priorCriteriaId)
        {
            List<DamagePriority> damagePriorityList = (from s in db.DamagePriorities
                                                       where s.PrioritizationCriteriaId == priorCriteriaId
                                                       select s).ToList();

            return PartialView(damagePriorityList);
        }

        public ActionResult RequiredAction()
        {
            return PartialView(db.RequiredActions.ToList());
        }

        [HttpPost]
        public ActionResult UpdateRequiredAction(RequiredAction reqAction)
        {
            RequiredAction updatedReqAction = (from c in db.RequiredActions
                                               where c.RequiredActionId == reqAction.RequiredActionId
                                               select c).FirstOrDefault();

            updatedReqAction.ValueFrom = reqAction.ValueFrom;
            updatedReqAction.ValueTo = reqAction.ValueTo;

            db.SaveChanges();

            return new EmptyResult();
        }
    }
}