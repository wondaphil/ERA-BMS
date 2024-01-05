using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using ERA_BMS.Models;
using System.ComponentModel.DataAnnotations;

namespace ERA_BMS.ViewModels
{
    public enum StatusMessageId
    {
        AddDataSuccess,
        ChangeDataSuccess,
        DeleteDataSuccess,
        DuplicateDataError,
        Error
    }

    public static class StatusMessageViewModel
    {
        public static string GetMessage(StatusMessageId? statusId)
        {
            return
                statusId == StatusMessageId.AddDataSuccess ? "Data has been added successfully."
                : statusId == StatusMessageId.ChangeDataSuccess ? "Data has been changed successfully."
                : statusId == StatusMessageId.DeleteDataSuccess ? "Data has been deleted successfully."
                : statusId == StatusMessageId.DuplicateDataError ? "Duplicate data error has occurred and operation is unsuccessful."
                : statusId == StatusMessageId.Error ? "An error has occurred and operation is unsuccessful."
                : statusId == null ? ""
                : "";
        }
    }
}