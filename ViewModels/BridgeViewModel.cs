using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeViewModel
    {
        public Bridge Bridge { get; set; }
        public BridgeGeneralInfo GenInfo { get; set; }
        public SuperStructure SuperStr { get; set; }
        public Abutment Abutment { get; set; }
        public List<Pier> Piers { get; set; }
        public Ancillary Ancillaries { get; set; }
        public List<BridgeDoc> BridgeDoc { get; set; }
        public List<BridgeMedia> BridgeMedia { get; set; }
        public List<BridgeMedia> BridgeImage { get; set; }
        public List<BridgeMedia> BridgeVideo { get; set; }
        public List<BridgeMedia> BridgeDrawing { get; set; }
        public List<BridgeMedia> BridgeDamageImage { get; set; }
    }
}