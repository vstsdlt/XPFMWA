using FACTS.Framework.Service;
using FACTS.Framework.Support;
using PFML.DAL.Model.DbEntities;
using PFML.Shared.LookupTable;
using PFML.Shared.Model.DbDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using DbContext = PFML.DAL.Model.DbContext;

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
		public static VoluntaryPlanWaiverRequestDto GetRequest(VoluntaryPlanWaiverRequestDto VoluntaryPlanWaiverRequestDto)
		{

			using (DbContext context = new DbContext())
			{
				var record = (from vpwr in context.VoluntaryPlanWaiverRequests.Include("VoluntaryPlanWaiverRequestTypes").Include("Form.FormAttachments.Document")
							  where vpwr.FormId == VoluntaryPlanWaiverRequestDto.FormId
							  select vpwr).FirstOrDefault();
				return record.ToDtoDeep(context);

			}
		}

		[OperationMethod]
		public static List<VoluntaryPlanWaiverRequestDto> SubmitRequest(VoluntaryPlanWaiverRequestDto VPRequestForm)
		{
			ExecuteSubmitRequestRules(VPRequestForm);
			if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) == 0)
			{
				using (DbContext context = new DbContext())
				{
					VPRequestForm.VoluntaryPlanWaiverRequestTypes.RemoveAll(r => r.LeaveTypeCode == null);
					VoluntaryPlanWaiverRequest.FromDto(context, VPRequestForm);
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
		public static VoluntaryPlanWaiverRequestDto SetData()
		{
			VoluntaryPlanWaiverRequestDto voluntaryPlanWaiverRequestDto = new VoluntaryPlanWaiverRequestDto
			{
				StartDate = DateTime.Now.Date,
				EndDate = new DateTime(DateTime.Now.Year, 12, 31)
			};
			return SetLeaveType(voluntaryPlanWaiverRequestDto);
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
                //Sumit Singh: Code breaking: Please check
				//using (DbContext context = new DbContext())
				//{
				//	var query = (from vpwr in context.VoluntaryPlanWaiverRequests.Include("Form")
				//				 where vpwr.EmployerId == EmployerId
				//				 orderby vpwr.Form.UpdateDateTime, vpwr.FormId descending
				//				 select vpwr)?.ToDtoListDeep(context);

				//	return query;
				//}
                return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="VPRequestForm"></param>
		/// <returns></returns>
		internal static VoluntaryPlanWaiverRequestDto SetLeaveType(VoluntaryPlanWaiverRequestDto voluntaryPlanWaiverRequestDto)
		{
			foreach (var waiverLeaveType in LookupTable.WaiverLeaveType)
			{
				voluntaryPlanWaiverRequestDto.VoluntaryPlanWaiverRequestTypes.Add(new VoluntaryPlanWaiverRequestTypeDto());
			}
			return voluntaryPlanWaiverRequestDto;
		}
		#endregion

		#region Business Rules
		internal static void ExecuteSubmitRequestRules(VoluntaryPlanWaiverRequestDto VPRequestForm)
		{
			if (VPRequestForm.StartDate.Date >= DateTime.Today.Date)
			{
				Context.ValidationMessages.AddError("Start Date cannot be before today.");
			}

			if (VPRequestForm.EndDate.Date < VPRequestForm.StartDate.Date)
			{
				Context.ValidationMessages.AddError("End date cannot be before Start date.");
			}

			if (VPRequestForm.EndDate.Date > VPRequestForm.StartDate.Date.AddYears(1).AddDays(-1))
			{
				Context.ValidationMessages.AddError("End Date cannot be more than 1 year after the Start Date");
			}

			if (!VPRequestForm.IsVoluntaryPlanWaiverRequestAcknowledged)
			{
				Context.ValidationMessages.AddError("You must agree to the T&C to submit your request");
			}

			bool leaveTypeChecked = false;

			foreach (var item in VPRequestForm.VoluntaryPlanWaiverRequestTypes)
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
				Context.ValidationMessages.AddError("Atleast one Leave Type should be selected");
				//Available annually and Percentage of Wages Paid in the type of leave available section should be provided
			}

		}
		#endregion
	}
}
