using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    //Only Admin should have access
    [Authorize(Roles = "Admin")]
    public class ErrorCheckerController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: ErrorChecker
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InvalidCulvertInspectionDate()
        {
            var result = (from cul in db.Culverts
                          join inv in db.InvalidCulvertInspectionDates on cul.CulvertId equals inv.CulvertId
                          select new InvalidInspectionDateViewModel
                          {
                              Culverts = cul,
                              InvalidInspDate = inv
                          }
                          ).ToList();

            return PartialView(result);
        }

        public ActionResult DuplicateBridgeNo()
        {
            var duplicatesSummary = db.Bridges.GroupBy(s => s.BridgeNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<Bridge>> duplicatesList = new List<List<Bridge>>();
            foreach (var bridge in duplicatesSummary)
            {
                List<Bridge> brg = db.Bridges.Where(br => br.BridgeNo == bridge.Element).ToList();
                duplicatesList.Add(brg);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }

        public ActionResult DuplicateCulvertNo()
        {
            var duplicatesSummary = db.Culverts.GroupBy(s => s.CulvertNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<Culvert>> duplicatesList = new List<List<Culvert>>();
            foreach (var culvert in duplicatesSummary)
            {
                List<Culvert> cul = db.Culverts.Where(br => br.CulvertNo == culvert.Element).ToList();
                duplicatesList.Add(cul);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }

        public ActionResult DuplicateMainRouteNo()
        {
            var duplicatesSummary = db.MainRoutes.GroupBy(s => s.MainRouteNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<MainRoute>> duplicatesList = new List<List<MainRoute>>();
            foreach (var Mainroute in duplicatesSummary)
            {
                List<MainRoute> mainRt = db.MainRoutes.Where(br => br.MainRouteNo == Mainroute.Element).ToList();
                duplicatesList.Add(mainRt);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }

        public ActionResult DuplicateSubRouteNo()
        {
            var duplicatesSummary = db.SubRoutes.GroupBy(s => s.SubRouteNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<SubRoute>> duplicatesList = new List<List<SubRoute>>();
            foreach (var subroute in duplicatesSummary)
            {
                List<SubRoute> subRt = db.SubRoutes.Where(br => br.SubRouteNo == subroute.Element).ToList();
                duplicatesList.Add(subRt);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }

        public ActionResult DuplicateSectionNo()
        {
            var duplicatesSummary = db.Sections.GroupBy(s => s.SectionNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<Section>> duplicatesList = new List<List<Section>>();
            foreach (var Section in duplicatesSummary)
            {
                List<Section> sec = db.Sections.Where(br => br.SectionNo == Section.Element).ToList();
                duplicatesList.Add(sec);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }

        public ActionResult DuplicateSegmentNo()
        {
            var duplicatesSummary = db.Segments.GroupBy(s => s.SegmentNo)
                                .Where(s => s.Count() > 1)
                                .Select(s => new { Element = s.Key, Counter = s.Count() })
                                .ToList();

            List<List<Segment>> duplicatesList = new List<List<Segment>>();
            foreach (var Mainroute in duplicatesSummary)
            {
                List<Segment> seg = db.Segments.Where(br => br.SegmentNo == Mainroute.Element).ToList();
                duplicatesList.Add(seg);
            }

            ViewBag.DuplicatesList = duplicatesList;

            return PartialView();
        }
    }
}