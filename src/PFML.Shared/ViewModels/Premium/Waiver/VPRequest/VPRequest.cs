using PFML.Shared.Model.DbDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.ViewModels.Premium.Waiver
{
	[Serializable]
	public class VPRequestViewModel
	{
		public int RequestId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string StatusCode { get; set; }
        public string TypesofLeaveAvailable { get; set; }
        public string WeeksAvailableAnnually { get; set; }
        public Decimal? PercentageofWagesPaid { get; set; }
        public string DocumentName { get; set; }
    }

	[Serializable]
	public class VPRequestForm
	{
		public int EmployerId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<VoluntaryPlanWaiverRequestTypeDto> RequestType { get; set; }
		public List<DocumentDto> Document { get; set; }
		public bool IsAcknolwedged { get; set; }
	}
	
	[Serializable]
	public class VPViewRequest
	{
		public int RequestId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }      
        public string TypesofLeaveAvailable { get; set; }
        public string WeeksAvailableAnnually { get; set; }
        public Decimal? PercentageofWagesPaid { get; set; }
        public string DocumentName { get; set; }
    }
}
