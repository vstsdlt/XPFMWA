using System;
using System.Collections.Generic;
using System.Linq;
using FACTS.Framework.DAL;
using FACTS.Framework.Service;
using PFML.DAL.Model.DbEntities;
using PFML.Shared.Model.DbDtos;
using DbContext = PFML.DAL.Model.DbContext;
using PFML.Shared.ViewModels.Revenue;
using PFML.Shared.LookupTable;
using PFML.Shared.ViewModels.Premium.Waiver;
using FACTS.Framework.Support;

namespace PFML.BusinessLogic.Premium.Waiver.VPRequest
{

	public static class VPRequestLogic
	{

		[OperationMethod]
		public static List<VoluntaryPlanWaiverRequestDto> GetList(int EmployerId)
		{
			return GetRequestColl(EmployerId);
		}

		[OperationMethod]
		public static VoluntaryPlanWaiverRequestDto GetRequest(int RequestId)
		{

			using (DbContext context = new DbContext())
			{				
				var record = (from vpwr in context.VoluntaryPlanWaiverRequests.Include("VoluntaryPlanWaiverRequestTypes").Include("Form.FormAttachments.Document")
							  where vpwr.FormId == RequestId
							  select vpwr).FirstOrDefault();
				return record.ToDtoDeep(context);

			}
		}

		[OperationMethod]
		public static List<VoluntaryPlanWaiverRequestDto> SubmitRequest(VPRequestForm VPRequestForm)
		{			
			ExecuteSubmitRequestRules(VPRequestForm);
			if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) == 0)
			{
				using (DbContext context = new DbContext())
				{
					FormDto FormReq = new FormDto()
					{
						FormTypeCode = LookupTable_FormType.VPWaiverForm,
						StatusCode = LookupTable_WaiverRequestStatus.SavedasDraft

					};
					Form.FromDto(context, FormReq);

					VoluntaryPlanWaiverRequestDto VPWReq = new VoluntaryPlanWaiverRequestDto()
					{
						EmployerId = VPRequestForm.EmployerId,
						StartDate = VPRequestForm.StartDate,
						EndDate = VPRequestForm.EndDate,
						IsVoluntaryPlanWaiverRequestAcknowledged = VPRequestForm.IsAcknolwedged
					};
					VoluntaryPlanWaiverRequest.FromDto(context, VPWReq);


					VPRequestForm.RequestType.RemoveAll(p => string.IsNullOrWhiteSpace(p.LeaveTypeCode));
					foreach (var requestType in VPRequestForm.RequestType)
					{
						VoluntaryPlanWaiverRequestType VPWReqType = new VoluntaryPlanWaiverRequestType()
						{
							LeaveTypeCode = requestType.LeaveTypeCode,
							PercentagePaid = requestType.PercentagePaid,
							DurationInWeeksCode = requestType.DurationInWeeksCode
						};
						context.VoluntaryPlanWaiverRequestTypes.Add(VPWReqType);
					}

					if (VPRequestForm.Document != null)
					{
						foreach (var docItem in VPRequestForm.Document)
						{
							Document doc = new Document()
							{
								DocumentName = docItem.DocumentName,								
								DocumentDescription = docItem.DocumentDescription

							};
							context.Documents.Add(doc);
							context.FormAttachments.Add(new FormAttachment());
						}

					}

					context.SaveChanges();
				}
			}
			else
			{
				Context.ValidationMessages.ThrowCheck(ValidationMessageSeverity.Error);
			}
			return GetRequestColl(VPRequestForm.EmployerId);
		}

		[OperationMethod]
		public static VPRequestForm SetData(int EmployerId)
		{
			return SetLeaveType();
		}



		#region Private Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="EmployerId"></param>
		/// <returns></returns>
		internal static List<VoluntaryPlanWaiverRequestDto> GetRequestColl(int? EmployerId)
		{
			if (EmployerId is null)
			{
				return new List<VoluntaryPlanWaiverRequestDto>();
			}
			else
			{
				using (DbContext context = new DbContext())
				{
					var query = (from vpwr in context.VoluntaryPlanWaiverRequests.Include("Form")
								 where vpwr.EmployerId == EmployerId
								 orderby vpwr.Form.UpdateDateTime descending
								 select vpwr)?.ToDtoListDeep(context);

					return query;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="VPRequestForm"></param>
		/// <returns></returns>
		internal static VPRequestForm SetLeaveType(VPRequestForm VPRequestForm = null)
		{
			List<VoluntaryPlanWaiverRequestTypeDto> VoluntaryPlanWaiverRequestType = new List<VoluntaryPlanWaiverRequestTypeDto>();
			for (int i = 0; i < 4; i++)
			{
				VoluntaryPlanWaiverRequestType.Add(new VoluntaryPlanWaiverRequestTypeDto());
			}
			if (VPRequestForm is null)
			{
				return new VPRequestForm()
				{
					RequestType = VoluntaryPlanWaiverRequestType,
					StartDate = DateTime.Now.Date,
					EndDate = new DateTime(DateTime.Now.Year, 12, 31)

				};
			}
			else
			{
				VPRequestForm.RequestType = VoluntaryPlanWaiverRequestType;
				return VPRequestForm;
			}
		}
		#endregion

		#region Business Rules
		internal static void ExecuteSubmitRequestRules(VPRequestForm VPRequestForm)
		{
			if (VPRequestForm.StartDate > DateTime.Today)
			{
				Context.ValidationMessages.AddError("Start Date cannot be before today.");
			}

			if (VPRequestForm.EndDate < VPRequestForm.StartDate)
			{
				Context.ValidationMessages.AddError("End date cannot be before Start date.");
			}

			if (VPRequestForm.EndDate > VPRequestForm.StartDate.AddYears(1))
			{
				Context.ValidationMessages.AddError("End Date cannot be more than 1 year after the Start Date");
			}

			if (!VPRequestForm.IsAcknolwedged)
			{
				Context.ValidationMessages.AddError("You must agree to the T&C to submit your request");
			}

			bool leaveTypeChecked = false;

			foreach (var item in VPRequestForm.RequestType)
			{
				if (!string.IsNullOrWhiteSpace(item.LeaveTypeCode))
				{
					leaveTypeChecked = true;
					if (string.IsNullOrWhiteSpace(item.DurationInWeeksCode) && (item.PercentagePaid <= 0))
					{
						Context.ValidationMessages.AddError("Weeks Available annually and Percentage of Wages Paid are required fields for the checked leave type");
						break;
					}
					if (string.IsNullOrWhiteSpace(item.DurationInWeeksCode) && (item.PercentagePaid >= 0))
					{
						Context.ValidationMessages.AddError("Please select Weeks Available annually for the checked leave type");
					}
					if (!string.IsNullOrWhiteSpace(item.DurationInWeeksCode) && (item.PercentagePaid <= 0))
					{
						Context.ValidationMessages.AddError("Please provide Percentage of wages paid for the checked leave type");
						break;
					}
				}
			}
			if (!leaveTypeChecked)
			{
				Context.ValidationMessages.AddError("Atleast one Leave Type should be provided");
				//Available annually and Percentage of Wages Paid in the type of leave available section should be provided
			}

		}
		#endregion
	}
}
