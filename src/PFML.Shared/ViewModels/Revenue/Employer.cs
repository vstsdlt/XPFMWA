using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.ViewModels.Revenue
{
	[Serializable]
	public class Employer
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string AccountNumber { get; set; }

		public string AccountProfile { get; set; }
		public string EAN { get; set; }
		public string FEIN { get; set; }
		public DateTime LiabilityDate { get; set; }
		public DateTime LiabilityIncurred { get; set; }
		public string PaymentMethodType { get; set; }
		public bool ChangePaymentMethod { get; set; }
		public bool HistoryTransfer { get; set; }
		public bool Successor { get; set; }
		public bool InitiatedExistingSelfService { get; set; }
		public bool InitiatedForWageClaim { get; set; }
		public string NMPRC { get; set; }
		public string CRS { get; set; }
		public string EmployerStatus { get; set; }
		public string TaxRepresentative { get; set; }
		public DateTime ReativationDate { get; set; }
		public DateTime OutofBusinessDate { get; set; }
		public DateTime BankruptcyDate { get; set; }
		public string BankruptcyChapter { get; set; }
		public decimal CurrentUIContributionRate { get; set; }
		public decimal PreviousUIContributionRate { get; set; }
		public string LegalEntityType { get; set; }
		public string BusinessType { get; set; }
		public int IncorporationFormationState { get; set; }
		public DateTime IncorporationFormationDate { get; set; }
		public bool HasMultipleReportingUnits { get; set; }
		public DateTime NAICS { get; set; }
		public bool C501 { get; set; }
		public bool UsingLeasingCompany { get; set; }
		public string Notes { get; set; }


	}
	[Serializable]
	public class EmployerSearchResult
	{
		public List<Employer> EmployerColl { get; set; }
	}
}
