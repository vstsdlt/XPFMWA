using FACTS.Framework.Lookup;
using FACTS.Framework.Support;
using FACTS.Framework.Utility;
using FACTS.Framework.Web;
using PFML.Shared.LookupTable;
using PFML.Shared.Utility;
using PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission;
using System;
using System.Collections.Generic;

namespace PFML.Web.Controllers.Premium.WageDetail.WageSubmission
{
    public class WageSubmissionController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        public void MachineExecute()
        {
            Machine["ListSection"] = LookupUtil.GetValues(LookupTable.WizEmployerWageFiling, null, "{DisplaySortOrder}", null);
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SelectFilingMethod);

            Machine["ShowHeader"] = true;
            Machine["ReportingYearList"] = DateUtil.PopulateYears(Convert.ToInt16(LookupTable_WageDetailUnitYears.YearsForWageFiling), 0, false);

            //Set Default Values for Year and Quarter
            SetReportingYearAndQuarterDefaultValues();

        }


        /// <summary>
        /// 
        /// </summary>
        public void DetailedSummaryNext()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ProcessandCalculate);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DetailedSummaryPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SubmitWageInformation);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessAndCalculateTaxExecute()
        {
            Machine["ContributionRate"] = 0.4;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessAndCalculateTaxPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ConfirmSubmission);
        }


        /// <summary>
        /// 
        /// </summary>
        public void SelectFilingMethodNext()
        {
            int currentQtr = Convert.ToInt16(DateUtil.GetQuarter(DateTimeUtil.Now.Month).ToString().Remove(0, 1));
            DateTime subjDt = DateTimeUtil.Now;
            int selYr = Convert.ToInt16(Machine["WageUnitDetail.ReportingYear"]);
            int selQtr = Convert.ToInt16(Machine["WageUnitDetail.ReportingQuarter"].ToString().Remove(0, 1));

            bool isValidSubjDate = ((selYr > subjDt.Year) || (subjDt.Year == selYr && selQtr <= currentQtr));

            if (subjDt.Year <= selYr)
            {
                if (currentQtr < selQtr)
                {
                    Context.ValidationMessages.AddError("Submission not allowed for future quarter.");
                    return;
                }

            }
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SubmitWageInformation);
        }


        /// <summary>
        /// 
        /// </summary>
        public void ManualEntryNext()
        {
            WageSubmissionViewModel wageSubmissionViewModel = new WageSubmissionViewModel();
            wageSubmissionViewModel.ListWageUnitDetailDto = (List<WageSubmissionViewModel.WageUnitCustomDto>) Machine["WageUnitDetail.ListWageUnitDetailDto"];

            foreach (var wageUnitDetail in wageSubmissionViewModel.ListWageUnitDetailDto)
            {
                if ((wageUnitDetail.Ssn is null) && (wageUnitDetail.LastName != null || wageUnitDetail.FirstName != null ||
                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                    || wageUnitDetail.HoursWorked != null || wageUnitDetail.MiddleInitialName != null
                    || wageUnitDetail.Occupation != null || wageUnitDetail.IsOwnerOrOfficer != null || wageUnitDetail.WageAmount > 0))

                {
                    Context.ValidationMessages.AddError(String.Format("Missing SSN on line {0}.",
                         wageUnitDetail.SrNo));
                }
                if ((wageUnitDetail.LastName is null) && (wageUnitDetail.Ssn != null || wageUnitDetail.FirstName != null ||
                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                    || wageUnitDetail.HoursWorked != null || wageUnitDetail.MiddleInitialName != null
                    || wageUnitDetail.Occupation != null || wageUnitDetail.IsOwnerOrOfficer != null || wageUnitDetail.WageAmount > 0))

                {
                    Context.ValidationMessages.AddWarning(String.Format("Missing last name on line {0}. ",
                         wageUnitDetail.SrNo));
                }

                if ((wageUnitDetail.FirstName is null) && (wageUnitDetail.LastName != null || wageUnitDetail.Ssn != null ||
                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                    || wageUnitDetail.HoursWorked != null || wageUnitDetail.MiddleInitialName != null
                    || wageUnitDetail.Occupation != null || wageUnitDetail.IsOwnerOrOfficer != null || wageUnitDetail.WageAmount > 0))

                {
                    Context.ValidationMessages.AddWarning(String.Format("Missing first name on line {0}.",
                         wageUnitDetail.SrNo));
                }

                if ((wageUnitDetail.WageAmount==0) && (wageUnitDetail.LastName != null || wageUnitDetail.Ssn != null ||
                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                    || wageUnitDetail.HoursWorked != null || wageUnitDetail.MiddleInitialName != null
                    || wageUnitDetail.Occupation != null || wageUnitDetail.IsOwnerOrOfficer != null || wageUnitDetail.FirstName !=null))

                {
                    Context.ValidationMessages.AddError(String.Format("Missing Gross Wages on line {0}.",
                         wageUnitDetail.SrNo));
                }

                if (wageUnitDetail.WageAmount > Convert.ToDecimal(999999999.99))
                {
                    Context.ValidationMessages.AddError(String.Format("Gross Wages cannot exceed 999999999.99 on line {0}.",
                         wageUnitDetail.SrNo));
                }

            }

            if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error)> 0)
            {
                return;
            }
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ConfirmSubmission);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ManualEntryPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SelectFilingMethod);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetReportingYearAndQuarterDefaultValues()
        {
            Machine["WageFilingDefaultQuarter"] = DateUtil.GetQuarter(DateTimeUtil.Now.Month);
            Machine["WageFilingDefaultYear"] = Convert.ToString(DateTimeUtil.Now.Year);
        }
    }
}
