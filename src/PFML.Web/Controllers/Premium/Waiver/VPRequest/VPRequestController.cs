using System;
using System.Collections.Generic;
using FACTS.Framework.Web;
using FACTS.Framework.Lookup;
using FACTS.Framework.Support;
using PFML.Shared.LookupTable;
using PFML.Shared.Model.DbDtos;
using PFML.Shared.ViewModels.Premium.Registration;
using PFML.Shared.ViewModels.Premium.Waiver;

namespace PFML.Web.Controllers.Premium.Waiver.VPRequest
{

	public class VPRequestController : Controller
	{
		public void MachineExecute()
		{
			Machine["EmployerId"] = 1;
		}
		public void RequestView()
		{
			Machine["RequestId"] = Machine["VPWaiver[0].FormId"];
		}
		public void NewRequestUpload()
		{
			Machine["DocumentDescription"] = string.Empty;
			Machine["DocumentName"] = string.Empty;
		}
		public void UploadDocumentNext()
		{
			VPRequestForm vPRequestForm;
			vPRequestForm = (VPRequestForm)Machine["VPRequestForm"];
			vPRequestForm.Document = vPRequestForm.Document ?? new List<DocumentDto>();
			vPRequestForm.Document.Add(new DocumentDto()
			{
				DocumentDescription = Machine["DocumentDescription"].ToString(),
				DocumentName = Machine["DocumentName"].ToString()
			});
			vPRequestForm.EmployerId = (int)Machine["EmployerId"];
			Machine["VPRequestForm"] = vPRequestForm;
		}
		public void NewRequestSubmit()
		{
			if ((DateTime)Machine["VPRequestForm.StartDate"] == DateTime.MinValue)
			{
				Context.ValidationMessages.AddError("Start Date is required");
			}
			if ((DateTime)Machine["VPRequestForm.EndDate"] == DateTime.MinValue)
			{
				Context.ValidationMessages.AddError("End Date is required");
			}

			if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) > 0)
			{
				Context.ValidationMessages.ThrowCheck(ValidationMessageSeverity.Error);

			}

			if (!(bool)Machine["VPRequestForm.IsAcknolwedged"])
			{
				Context.ValidationMessages.AddError("You must agree to the T&C to submit your request");
			}
			else
			{
				VPRequestForm vPRequestForm;
				vPRequestForm = (VPRequestForm)Machine["VPRequestForm"];
				vPRequestForm.EmployerId = (int)Machine["EmployerId"];
			}
		}		
	}
}



