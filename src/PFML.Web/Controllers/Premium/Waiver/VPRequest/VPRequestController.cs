using FACTS.Framework.Support;
using FACTS.Framework.Web;
using PFML.Shared.LookupTable;
using PFML.Shared.Model.DbDtos;
using System.Collections.Generic;



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
			//Machine["RequestId"] = Machine["VPWaiver.FormId"];
		}
		public void NewRequestUpload()
		{
			List<DocumentDto> docs = (List<DocumentDto>)Machine["Documents"];
			if (docs.Count >= 10)
			{
				Context.ValidationMessages.AddError("You can not upload more than 10 supporting documents");
				Context.ValidationMessages.ThrowCheck(ValidationMessageSeverity.Error);
			}
			Machine["DocumentDescription"] = string.Empty;
			Machine["DocumentName"] = string.Empty;

		}
		public void UploadDocumentNext()
		{
			if (Machine["Documents"] == null)
			{
				Machine["Documents"] = new List<DocumentDto>();
			}
			List<DocumentDto> documents = (List<DocumentDto>)Machine["Documents"];
			documents.Add(new DocumentDto()
			{
				DocumentDescription = Machine["DocumentDescription"].ToString(),
				DocumentName = Machine["DocumentName"].ToString()
			});
			Machine["Documents"] = documents;

		}
		public void NewRequestSubmit()
		{
			List<DocumentDto> docs = (List<DocumentDto>)Machine["Documents"];
			if (docs.Count == 0)
			{
				Context.ValidationMessages.AddError("Atleast one supporting document should upload to submit the request");
				Context.ValidationMessages.ThrowCheck(ValidationMessageSeverity.Error);
			}
			VoluntaryPlanWaiverRequestDto voluntaryPlanWaiverRequestDto;
			voluntaryPlanWaiverRequestDto = (VoluntaryPlanWaiverRequestDto)Machine["VPRequestForm"];
			if (!voluntaryPlanWaiverRequestDto.IsVoluntaryPlanWaiverRequestAcknowledged)
			{
				Context.ValidationMessages.AddError("You must agree to the T&C to submit your request");
				Context.ValidationMessages.ThrowCheck(ValidationMessageSeverity.Error);
			}
			voluntaryPlanWaiverRequestDto.EmployerId = (int)Machine["EmployerId"];
            //Sumit Singh: Code breaking: Please check
            //voluntaryPlanWaiverRequestDto.Form = new FormDto
            //{
            //	FormTypeCode = LookupTable_FormType.VPWaiverForm,
            //	StatusCode = LookupTable_WaiverRequestStatus.SavedasDraft
            //};
            //foreach (DocumentDto document in (List<DocumentDto>)Machine["Documents"])
            //{
            //	voluntaryPlanWaiverRequestDto.Form.FormAttachments.Add(
            //		new FormAttachmentDto()
            //		{
            //			Document = document
            //		});
            //};

        }
        public void NewRequestViewDoc()
		{

		}
		public void NewRequestDeleteDoc()
		{
			List<DocumentDto> docs = (List<DocumentDto>)Machine["Documents"];
			docs.Remove((DocumentDto)Machine["Document"]);
			Machine["Documents"] = docs;
		}
		public void SetDataNext()
		{
			Machine["Documents"] = new List<DocumentDto>();
		}
	}
}



