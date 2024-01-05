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
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class MediaTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/MediaTypes
        public ActionResult Index(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            return View(db.MediaTypes.ToList());
        }

        public ActionResult UpdateMediaType(MediaType mediaType)
        {
            MediaType medType = db.MediaTypes.Find(mediaType.MediaTypeId);

            medType.MediaTypeName = mediaType.MediaTypeName;
            medType.Remark = mediaType.Remark;
            
            try
            {
                db.SaveChanges();
            }
            catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
            }

            return new EmptyResult();
        }

        public ActionResult DeleteMediaType(int id)
        {
            MediaType mediaType = db.MediaTypes.Find(id);
            if (mediaType != null)
            {
                db.MediaTypes.Remove(mediaType);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        public ActionResult NewMediaTypePartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMediaTypePartial([Bind(Include = "MediaTypeId,MediaTypeName,Remark")] MediaType medType)
        {

            if (ModelState.IsValid)
            {
                db.MediaTypes.Add(medType);
                
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index", "MediaTypes", new { messageId = StatusMessageId.AddDataSuccess });
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }
            }

            return View(medType);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
