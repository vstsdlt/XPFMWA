using FACTS.Framework.Lookup;
using FACTS.Framework.Service;
using PFML.Shared.LookupTable;
using PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission;
using System;
using System.Collections.Generic;
using System.Linq;
using DbContext = PFML.DAL.Model.DbContext;
using PFML.DAL.Model.DbEntities;
using FACTS.Framework.Utility;
using PFML.Shared.Utility;

namespace PFML.BusinessLogic.Premium.WageDetail
{

    public static class WageSubmissionLogic
    {
        /// <summary>
        /// To do 
        /// </summary>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel GetEmpWageDetails()
        {
            WageSubmissionViewModel wage = new WageSubmissionViewModel();
            
            return wage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel GetWagePeriod()
        {
            WageSubmissionViewModel wage = new WageSubmissionViewModel();


            using (DbContext context = new DbContext())
            {
                //To do Map the Employerid from context.
                wage.Employer = context.Employers.FirstOrDefault(emp => emp.EmployerId == 19).ToDto();
            }
            
            for (int i = 1; i <= 25; i++)
            {
                WageSubmissionViewModel.WageUnitCustomDto wageUnitDetailDto = new WageSubmissionViewModel.WageUnitCustomDto();
                wageUnitDetailDto.SrNo = i;
                wageUnitDetailDto.EmployerId = wage.Employer.EmployerId;
                wageUnitDetailDto.Employer= wage.Employer;
                wage.ListWageUnitDetailDto.Add(wageUnitDetailDto);
            }

            return wage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel ValidateSelectionMethod(WageSubmissionViewModel wageDetail)
        {
            wageDetail.AdjReasonDisplay = LookupUtil.GetLookupCode(LookupTable.WageDetailAdjReasonCode, LookupTable_WageDetailAdjReasonCode.Original).Display.ToString();
            wageDetail.AdjReasonCode = LookupTable_WageDetailAdjReasonCode.Original;
            wageDetail.FilingMethod = LookupTable_WageDetailFilingMethod.ManualEntry;
            return wageDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel ValidateManualEntrySubmission(WageSubmissionViewModel wageDetail)
        {
            wageDetail.ListWageEmployerUnitSummary = new List<WageSubmissionViewModel.WageDetailSummaryViewModel>();

            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.FilingMethod = wageDetail.FilingMethod);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.AdjReasonCode = wageDetail.AdjReasonCode);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.ReportingQuarter = wageDetail.ReportingQuarter);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.ReportingYear = wageDetail.ReportingYear);

            foreach (var unit in wageDetail.ListWageUnitDetailDto.Select(x => x.EmployerUnitId).Distinct())
            {
                if (unit > 0)
                {
                    Decimal grossWage = wageDetail.ListWageUnitDetailDto.Where(x => x.EmployerUnitId == unit).Select(x => x.WageAmount).Sum();
                    int month1 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth1 == true && x.EmployerUnitId==unit).Count();
                    int month2 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth2 == true && x.EmployerUnitId == unit).Count();
                    int month3 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth3 == true && x.EmployerUnitId == unit).Count();
                    int NumberofRecords = wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null && x.EmployerUnitId == unit).Count();

                    wageDetail.ListWageEmployerUnitSummary.Add(new WageSubmissionViewModel.WageDetailSummaryViewModel { EmployerUnitNo = unit, EntityName = wageDetail.Employer.EntityName, NumberofRecords = NumberofRecords, GrossWage = grossWage, QtrMonth1RecordsCount = month1, QtrMonth2RecordsCount = month2, QtrMonth3RecordsCount = month3 });
                }
            }
            wageDetail.GrossWages = wageDetail.ListWageEmployerUnitSummary.Select(x => x.GrossWage).Sum();
            wageDetail.NumberofRecords = wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).Count(); 
            wageDetail.EmployerAccountTransactionDto.OwedAmount = Decimal.Multiply(wageDetail.GrossWages, (decimal)0.0040);
            wageDetail.EmployerAccountTransactionDto.UnpaidAmount = wageDetail.EmployerAccountTransactionDto.OwedAmount;
            return wageDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        
        public static WageSubmissionViewModel ValidateTax(WageSubmissionViewModel wageDetail)
        {

            wageDetail.EmployerAccountTransactionDto.EmployerId = wageDetail.Employer.EmployerId;
            wageDetail.EmployerAccountTransactionDto.ReportingQuarter = wageDetail.ReportingQuarter;
            wageDetail.EmployerAccountTransactionDto.ReportingYear = wageDetail.ReportingYear;
            wageDetail.EmployerAccountTransactionDto.TypeCode = Constants.TypeCode_OUUU;
            wageDetail.EmployerAccountTransactionDto.StatusCode = Constants.StatusCode_UnPaid;
            wageDetail.EmployerAccountTransactionDto.DueDate = DateTimeUtil.Now.AddDays(10);

            using (DbContext context = new DbContext())
            {
                
                foreach (var wageUnitDetails in wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null))
                {
                    wageUnitDetails.Employer = null;
                    WageUnitDetail.FromDto(context, wageUnitDetails);
                    context.SaveChanges();
                    
                }
                
                EmployerAccountTransaction.FromDto(context, wageDetail.EmployerAccountTransactionDto);
                context.SaveChanges();

            }
           
            return wageDetail;
        }


    }


}

