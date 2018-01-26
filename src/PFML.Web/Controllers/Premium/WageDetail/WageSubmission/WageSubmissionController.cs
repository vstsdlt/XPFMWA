using FACTS.Framework.Lookup;
using FACTS.Framework.Support;
using FACTS.Framework.Utility;
using FACTS.Framework.Web;
using PFML.Shared.LookupTable;
using PFML.Shared.Utility;
using PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PFML.Web.Controllers.Premium.WageDetail.WageSubmission
{
    public class WageSubmissionController : Controller
    {
        #region Page Load
        /// <summary>
        /// Method executes during page load
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
        /// Method executes during page load for Detailed Summary Page
        /// </summary>
        public void DetailedSummaryExecute()
        {
            int selQtr = Convert.ToInt16(Machine["WageUnitDetail.ReportingQuarter"].ToString().Remove(0, 1));
            if (selQtr == 1)
            {
                Machine["FirstMonthOfQuater"] = Constants.January12thoftheMonth;
                Machine["SecondMonthOfQuater"] = Constants.February12thoftheMonth;
                Machine["ThirdMonthOfQuater"] = Constants.March12thoftheMonth;
            }
            if (selQtr == 2)
            {
                Machine["FirstMonthOfQuater"] = Constants.April12thoftheMonth;
                Machine["SecondMonthOfQuater"] = Constants.May12thoftheMonth;
                Machine["ThirdMonthOfQuater"] = Constants.June12thoftheMonth;
            }
            if (selQtr == 3)
            {
                Machine["FirstMonthOfQuater"] = Constants.July12thoftheMonth;
                Machine["SecondMonthOfQuater"] = Constants.August12thoftheMonth;
                Machine["ThirdMonthOfQuater"] = Constants.September12thoftheMonth;
            }
            if (selQtr == 4)
            {
                Machine["FirstMonthOfQuater"] = Constants.October12thoftheMonth;
                Machine["SecondMonthOfQuater"] = Constants.November12thoftheMonth;
                Machine["ThirdMonthOfQuater"] = Constants.December12thoftheMonth;
            }
        }

        /// <summary>
        /// Method executes during page load for Confirmation Page
        /// </summary>
        public void WageDetailConfirmationExecute()
        {
            Machine["ReportingQuatertoDispay"] = LookupUtil.GetLookupCode(LookupTable.Quarter, Machine["WageUnitDetail.ReportingQuarter"].ToString()).Display;


        }

        #endregion

        #region Wizard Navigation methods
        public void DetailedSummaryNext()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ProcessandCalculate);
        }

        public void DetailedSummaryPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SubmitWageInformation);
        }

        public void ProcessAndCalculateTaxPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ConfirmSubmission);
        }

        public void SelectFilingMethodNext()
        {
            int currentQtr = Convert.ToInt16(DateUtil.GetQuarterNumber(DateTimeUtil.Now.Month));
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

        public void ManualEntryNext()
        {
            WageSubmissionViewModel wageSubmissionViewModel = new WageSubmissionViewModel();
            wageSubmissionViewModel.ListWageUnitDetailDto = (List<WageSubmissionViewModel.WageUnitCustomDto>)Machine["WageUnitDetail.ListWageUnitDetailDto"];

            if (wageSubmissionViewModel.ListWageUnitDetailDto.Where(x => x.Ssn != null).Count() == 0)
            {
                Context.ValidationMessages.AddError("Please enter atleast one record.");
            }
            foreach (var wageUnitDetail in wageSubmissionViewModel.ListWageUnitDetailDto)
            {
                if (wageSubmissionViewModel.ListWageUnitDetailDto.Where((x => (x.Ssn!=null) && (x.Ssn == wageUnitDetail.Ssn)&& 
                (x.EmployerUnitId==wageUnitDetail.EmployerUnitId) && x.SrNo!=wageUnitDetail.SrNo)).Count() > 0)
                {
                    Context.ValidationMessages.AddError(String.Format("Duplicate Social Security Number {0} within Unit {1} on line {2}.",
                    wageUnitDetail.Ssn, wageUnitDetail.EmployerUnitId,wageUnitDetail.SrNo));
                }
                if ((String.IsNullOrWhiteSpace(wageUnitDetail.Ssn)) && (!String.IsNullOrWhiteSpace(wageUnitDetail.LastName) || !String.IsNullOrWhiteSpace(wageUnitDetail.FirstName) ||
                                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                                    || wageUnitDetail.HoursWorked.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName)
                                    || !String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) || wageUnitDetail.IsOwnerOrOfficer.HasValue || wageUnitDetail.WageAmount > 0))

                {
                    Context.ValidationMessages.AddError(String.Format("Missing SSN on line {0}.",
                     wageUnitDetail.SrNo));
                }
                if ((String.IsNullOrWhiteSpace(wageUnitDetail.LastName)) && (!String.IsNullOrWhiteSpace(wageUnitDetail.Ssn) || !String.IsNullOrWhiteSpace(wageUnitDetail.FirstName) ||
                    wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                    || wageUnitDetail.HoursWorked.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName)
                    || !String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) || wageUnitDetail.IsOwnerOrOfficer.HasValue || wageUnitDetail.WageAmount > 0))
                {
                    Context.ValidationMessages.AddWarning(String.Format("Missing last name on line {0}.",
                         wageUnitDetail.SrNo));
                }

                if ((String.IsNullOrWhiteSpace(wageUnitDetail.FirstName)) && (!String.IsNullOrWhiteSpace(wageUnitDetail.Ssn) || !String.IsNullOrWhiteSpace(wageUnitDetail.LastName) ||
                                   wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                                   || wageUnitDetail.HoursWorked.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName)
                                   || !String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) || wageUnitDetail.IsOwnerOrOfficer.HasValue || wageUnitDetail.WageAmount > 0))
                {
                    Context.ValidationMessages.AddWarning(String.Format("Missing first name on line {0}.",
                     wageUnitDetail.SrNo));
                }

                if ((wageUnitDetail.WageAmount == 0) && (!String.IsNullOrWhiteSpace(wageUnitDetail.Ssn) || !String.IsNullOrWhiteSpace(wageUnitDetail.LastName) ||
                                                  wageUnitDetail.IsEmploymentMonth1 == true || wageUnitDetail.IsEmploymentMonth2 == true || wageUnitDetail.IsEmploymentMonth3 == true
                                                  || wageUnitDetail.HoursWorked.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName)
                                                  || !String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) || wageUnitDetail.IsOwnerOrOfficer.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.FirstName)))
                {
                    Context.ValidationMessages.AddError(String.Format("Missing Gross Wages on line {0}.",
                     wageUnitDetail.SrNo));
                }

                if ((!(wageUnitDetail.IsEmploymentMonth1 ?? false) && !(wageUnitDetail.IsEmploymentMonth2 ?? false) && !(wageUnitDetail.IsEmploymentMonth3 ?? false) ) && (!String.IsNullOrWhiteSpace(wageUnitDetail.LastName)
                   || !String.IsNullOrWhiteSpace(wageUnitDetail.FirstName) || wageUnitDetail.HoursWorked.HasValue || !String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName) || !String.IsNullOrWhiteSpace(wageUnitDetail.Ssn)
                  || !String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) || wageUnitDetail.IsOwnerOrOfficer.HasValue || wageUnitDetail.WageAmount > 0))
                {
                    Context.ValidationMessages.AddError(String.Format("Select atleast one Employment Month on line {0}.",
                     wageUnitDetail.SrNo));
                }
                if (wageUnitDetail.WageAmount > Convert.ToDecimal(999999999.99))
                {
                    Context.ValidationMessages.AddError(String.Format("Gross Wages cannot exceed 999999999.99 on line {0}.",
                         wageUnitDetail.SrNo));
                }

            }
            
           
            if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) > 0)
            {
                return;
            }
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.ConfirmSubmission);
        }

        public void ManualEntryPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerWageFiling, LookupTable_WizEmployerWageFiling.SelectFilingMethod);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Set Default Values for Year and Quater Selectbox.
        /// </summary>
        public void SetReportingYearAndQuarterDefaultValues()
        {
            Machine["WageFilingDefaultQuarter"] = DateUtil.GetQuarter(DateTimeUtil.Now.Month);
            Machine["WageFilingDefaultYear"] = Convert.ToString(DateTimeUtil.Now.Year);
        }

        #endregion

        #region UI Validations
        /// <summary>
        ///  Validates Add' functionality step
        /// </summary>
        public void ManualEntryAdd()
        {
            WageSubmissionViewModel wageSubmissionViewModel = new WageSubmissionViewModel();
            wageSubmissionViewModel.ListWageUnitDetailDto = (List<WageSubmissionViewModel.WageUnitCustomDto>)Machine["WageUnitDetail.ListWageUnitDetailDto"];

            foreach (var wageUnitDetail in wageSubmissionViewModel.ListWageUnitDetailDto)
            {
                if ((String.IsNullOrWhiteSpace(wageUnitDetail.Ssn) && String.IsNullOrWhiteSpace(wageUnitDetail.LastName)
                    && String.IsNullOrWhiteSpace(wageUnitDetail.FirstName) &&
                    !(wageUnitDetail.IsEmploymentMonth1??false) && !(wageUnitDetail.IsEmploymentMonth2??false) && !(wageUnitDetail.IsEmploymentMonth3??false) &&
                    !wageUnitDetail.HoursWorked.HasValue && String.IsNullOrWhiteSpace(wageUnitDetail.MiddleInitialName) &&
                    String.IsNullOrWhiteSpace(wageUnitDetail.Occupation) && !wageUnitDetail.IsOwnerOrOfficer.HasValue && wageUnitDetail.WageAmount == 0))
                {
                    Context.ValidationMessages.AddError(String.Format("You must complete page in full, before selecting 'Add'. If you have finished adding employees select 'Save' or 'Next'.",
                     wageUnitDetail.SrNo));
                    return;
                }
            }
        }

        #endregion
    }
}
