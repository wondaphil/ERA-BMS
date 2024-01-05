using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CoordinateSharp;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeGeneralInfoMetadata))]
    public partial class BridgeGeneralInfo
    {
        private Coordinate Coord
        {
            get
            {
                if (XCoord != null && YCoord != null && UtmZoneId != null)
                {
                    // Find the UTM zone
                    int utmZone = new BMSEntities().UtmZoneEthiopias.Find(UtmZoneId).UtmZone;
                    UniversalTransverseMercator utm = new UniversalTransverseMercator("N", utmZone, (double)XCoord, (double)YCoord);

                    return UniversalTransverseMercator.ConvertUTMtoLatLong(utm); // a coordinate variable
                }
                else
                    return null;
            }
        }

        // Latitude in the format 8.934422
        public double? LatitudeDecimal
        {
            get
            {
                return (Coord != null) ? Coord.Latitude.DecimalDegree : (double?)null;
            }
        }

        // Longitude in the format 38.765345
        public double? LongitudeDecimal
        {
            get
            {
                return (Coord != null) ? Coord.Longitude.DecimalDegree : (double?)null;
            }
        }

        // Latitude in the format N 8º 56' 3.92"
        public string LatitudeDMS
        {
            get
            {
                return (Coord != null) ? @Coord.Latitude.ToString() : null;
            }
        }

        // Longitude in the format E 38º 45' 55.241"
        public string LongitudeDMS
        {
            get
            {
                return (Coord != null) ? @Coord.Longitude.ToString() : null;
            }
        }
    }

    public class BridgeGeneralInfoMetadata
    {
        [Key, ForeignKey("Bridge"), Required]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Dist. From AA (Km)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> KMFromAddis { get; set; }

        [DisplayName("X-Coord.")]
        public Nullable<double> XCoord { get; set; }

        [DisplayName("Y-Coord.")]
        public Nullable<double> YCoord { get; set; }

        [DisplayName("UTM Zone")]
        public Nullable<int> UtmZoneId { get; set; }

        [Required]
        [DisplayName("Structure Length (m)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> BridgeLength { get; set; }
        
        [DisplayName("Width (m)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> BridgeWidth { get; set; }
        
        [DisplayName("River Width (m)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> RiverWidth { get; set; }
        
        [DisplayName("Present Water Level (m)")]
        public Nullable<double> PresentWaterLevel { get; set; }
        
        [DisplayName("Highest Water Level (m)")]
        public Nullable<double> HighestWaterLevel { get; set; }
        
        [DisplayName("Design Capacity (Ton)")]
        public Nullable<double> DesignCapacity { get; set; }

        [DisplayName("Bearing Capacity")]
        public Nullable<bool> BearingCapacity { get; set; }
        
        [DisplayName("Topography")]
        public string Topography { get; set; }
        
        [DisplayName("Altitude (m)")]
        public Nullable<double> Altitude { get; set; }
        
        [DisplayName("Road Alignment")]
        public Nullable<int> RoadAlignmentId { get; set; }

        [Required]
        [DisplayName("Original Construction Year")]
        public Nullable<int> ConstructionYear { get; set; }

        [Required]
        [DisplayName("Replaced Year")]
        public Nullable<int> ReplacedYear { get; set; }
        
        [DisplayName("Before 1935")]
        public Nullable<bool> Before1935 { get; set; }
        
        [DisplayName("Contractor")]
        public string Contractor { get; set; }

        [DisplayName("Designer / Supervisor")]
        public string Designer { get; set; }

        [Required]
        [DisplayName("Initial Construction Cost, ETB")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        //[DisplayFormat(DataFormatString = "{0:0,0.00}")]
        public Nullable<double> ConstructionCost { get; set; }

        [DisplayName("Current Replacement Cost, ETB")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> AssetReplacementCost { get; set; }

        [DisplayName("Safety Sign")]
        public Nullable<bool> SafetySign { get; set; }

        [DisplayName("Detour Possible")]
        public Nullable<bool> DetourPossible { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        
    }
}