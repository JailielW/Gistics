using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gistics.Models
{
    [Table("Route_Status")]
    public class RouteStatus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RouteStatusID { get; set; }

        public required string RouteStatus_ { get; set; }

        public RouteStatus()
        {
            RouteStatusID = 0;
            RouteStatus_ = string.Empty;
        }

        public RouteStatus(int routestatusid)
        {
            RouteStatusID = routestatusid;
        }
    }
}
