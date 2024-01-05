using ERA_BMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERA_BMS.Controllers
{
    public class StatusMessageController : Controller
    {
        public string GetStatusMessage(int? id)
        {
            StatusMessageId? messsageId = null;

            if (id != null)
                messsageId = (StatusMessageId)id;

            return "[{\"Message\": " + "\"" + StatusMessageViewModel.GetMessage(messsageId).ToString() + "\"}]";
        }
    }
}