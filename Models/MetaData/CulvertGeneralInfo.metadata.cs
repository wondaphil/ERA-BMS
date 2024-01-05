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
    [MetadataType(typeof(CulvertGeneralInfoMetadata))]
    public partial class CulvertGeneralInfo
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
                return (Coord != null) ? @Coord.Latitude.DecimalDegree : (double?) null;
            }
        }

        // Longitude in the format 38.765345
        public double? LongitudeDecimal
        {
            get
            {
                return (Coord != null) ? @Coord.Longitude.DecimalDegree : (double?)null;
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

    public class CulvertGeneralInfoMetadata
    {
        [Key, ForeignKey("Culvert"),Required]
        [DisplayName("Culvert Id")]
        public int CulvertId { get; set; }

        [DisplayName("Dist. From AA (Km)")]
        public Nullable<double> KMFromAddis { get; set; }

        [DisplayName("X-Coord.")]
        public Nullable<double> XCoord { get; set; }

        [DisplayName("Y-Coord.")]
        public Nullable<double> YCoord { get; set; }

        [DisplayName("UTM Zone")]
        public Nullable<int> UtmZoneId { get; set; }

        [DisplayName("Construction Year")]
        public Nullable<int> ConstructionYear { get; set; }
        
        [DisplayName("Designer / Consultant")]
        public string Designer { get; set; }

        [DisplayName("Contractor")]
        public string Contractor { get; set; }

        [DisplayName("Supervisor")]
        public string Supervisor { get; set; }

        [Required]
        [DisplayName("Initial Construction Cost, ETB")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> ConstructionCost { get; set; }

        [DisplayName("Current Replacement Cost, ETB")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> AssetReplacementCost { get; set; }
        
        [DisplayName("Detour Possible")]
        public Nullable<bool> DetourPossible { get; set; }

        [DisplayName("Altitude (m)")]
        public Nullable<double> Altitude { get; set; }

        [DisplayName("Road Width (m)")]
        public Nullable<double> RoadWidth { get; set; }

        [DisplayName("Head Wall Length (m)")]
        public Nullable<double> HeadWallLength { get; set; }

        [DisplayName("Fill Height (m)")]
        public Nullable<double> FillHeight { get; set; }

        [DisplayName("Parapet Material Type")]

        public Nullable<int> ParapetMaterialTypeId { get; set; }

        [DisplayName("Parapet Length (m)")]
        public Nullable<double> ParapetLength { get; set; }

        [DisplayName("No. of Lanes")]
        public Nullable<double> NoOfLanes { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}