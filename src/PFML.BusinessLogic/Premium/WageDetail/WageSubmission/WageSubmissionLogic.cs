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
        /// Get Wage Period details.
        /// </summary>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel GetWagePeriod()
        {
            WageSubmissionViewModel wage = new WageSubmissionViewModel();
            wage.ReportingYear = (short)DateTimeUtil.Now.Year;
            wage.ContributionRate = GetCurrentContributionRate();

            using (DbContext context = new DbContext())
            {
                //To do Map the Employerid from context.
                //wage.Employer = context.Employers.FirstOrDefault(emp => emp.EmployerId == 19).ToDto();
                wage.Employer = context.Employers.FirstOrDefault().ToDto();
            }

            for (int i = 1; i <= 25; i++)
            {
                WageSubmissionViewModel.WageUnitCustomDto wageUnitDetailDto = new WageSubmissionViewModel.WageUnitCustomDto();
                wageUnitDetailDto.SrNo = i;
                wageUnitDetailDto.EmployerId = wage.Employer.EmployerId;
                wageUnitDetailDto.Employer = wage.Employer;
                wage.ListWageUnitDetailDto.Add(wageUnitDetailDto);
            }

            return wage;
        }

        /// <summary>
        /// Validate Wage Submission Method
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel ValidateSelectionMethod(WageSubmissionViewModel wageDetail)
        {
            using (DbContext context = new DbContext())
            {
                if (context.TaxableAmountSums.Any(x => x.ReportingQuarter == wageDetail.ReportingQuarter && x.ReportingYear == wageDetail.ReportingYear && x.EmployerId == wageDetail.Employer.EmployerId))
                {
                    Context.ValidationMessages.AddError("Original Employment and Wage Detail Report for this year " + wageDetail.ReportingYear + " and quarter " + LookupUtil.GetLookupCode(LookupTable.Quarter, wageDetail.ReportingQuarter).Display + " has already been submitted");
                }
            }

            wageDetail.AdjReasonDisplay = LookupUtil.GetLookupCode(LookupTable.WageDetailAdjReasonCode, LookupTable_WageDetailAdjReasonCode.Original).Display.ToString();
            wageDetail.AdjReasonCode = LookupTable_WageDetailAdjReasonCode.Original;

            return wageDetail;
        }

        /// <summary>
        /// Validate Manual Entry Submission
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel ValidateManualEntrySubmission(WageSubmissionViewModel wageDetail)
        {
            wageDetail.ListWageEmployerUnitSummary = new List<WageSubmissionViewModel.WageDetailSummaryViewModel>();
            wageDetail.FilingMethod = LookupTable_WageDetailFilingMethod.ManualEntry;
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.FilingMethod = wageDetail.FilingMethod);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.AdjReasonCode = wageDetail.AdjReasonCode);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.ReportingQuarter = wageDetail.ReportingQuarter);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).ToList().ForEach(assign => assign.ReportingYear = wageDetail.ReportingYear);
            wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null && x.EmployerUnitId == 0).ToList().ForEach(assign => assign.EmployerUnitId = Constants.DefaultUnitId);

            foreach (var unit in wageDetail.ListWageUnitDetailDto.Select(x => x.EmployerUnitId).Distinct())
            {
                if (unit > 0)
                {
                    Decimal grossWage = wageDetail.ListWageUnitDetailDto.Where(x => x.EmployerUnitId == unit).Select(x => x.WageAmount).Sum();
                    int month1 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth1 == true && x.EmployerUnitId == unit).Count();
                    int month2 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth2 == true && x.EmployerUnitId == unit).Count();
                    int month3 = wageDetail.ListWageUnitDetailDto.Where(x => x.IsEmploymentMonth3 == true && x.EmployerUnitId == unit).Count();
                    int NumberofRecords = wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null && x.EmployerUnitId == unit).Count();

                    wageDetail.ListWageEmployerUnitSummary.Add(new WageSubmissionViewModel.WageDetailSummaryViewModel { EmployerUnitNo = unit, EntityName = wageDetail.Employer.EntityName, NumberofRecords = NumberofRecords, GrossWage = grossWage, QtrMonth1RecordsCount = month1, QtrMonth2RecordsCount = month2, QtrMonth3RecordsCount = month3 });
                }
            }
            wageDetail.GrossWages = wageDetail.ListWageEmployerUnitSummary.Select(x => x.GrossWage).Sum();
            wageDetail.NumberofRecords = wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null).Count();
            wageDetail.EmployerAccountTransactionDto.OwedAmount = Decimal.Multiply(wageDetail.GrossWages, Decimal.Multiply(wageDetail.ContributionRate, (decimal)0.01));
            wageDetail.EmployerAccountTransactionDto.UnpaidAmount = wageDetail.EmployerAccountTransactionDto.OwedAmount;
            return wageDetail;
        }

        /// <summary>
        /// Validate and Calculate Tax
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel ValidateTax(WageSubmissionViewModel wageDetail)
        {

            wageDetail.EmployerAccountTransactionDto.EmployerId = wageDetail.Employer.EmployerId;
            wageDetail.EmployerAccountTransactionDto.ReportingQuarter = wageDetail.ReportingQuarter;
            wageDetail.EmployerAccountTransactionDto.ReportingYear = wageDetail.ReportingYear;
            wageDetail.EmployerAccountTransactionDto.TypeCode = LookupTable_TransactionType.ContributionsPrinciple;
            wageDetail.EmployerAccountTransactionDto.StatusCode = LookupTable_PaymentStatus.PaymentStatusUnpaid;
            wageDetail.EmployerAccountTransactionDto.DueDate = DateTimeUtil.Now.AddDays(Convert.ToDouble(LookupTable_PremiumPaymentDueDays.PremiumPaymentDueDays));

            wageDetail.TaxableAmountSumDto.EmployerId = wageDetail.Employer.EmployerId;
            wageDetail.TaxableAmountSumDto.ReportingQuarter = wageDetail.ReportingQuarter;
            wageDetail.TaxableAmountSumDto.ReportingYear = wageDetail.ReportingYear;
            wageDetail.TaxableAmountSumDto.StatusCode = LookupTable_TaxableAmSumStatus.Submitted;
            wageDetail.TaxableAmountSumDto.TaxableAmountSeqNo = GetMaxTaxableAmountSumSeqNu(wageDetail.Employer.EmployerId, wageDetail.ReportingYear);
            int quaterNumber = Convert.ToInt16(wageDetail.ReportingQuarter.ToString().Remove(0, 1));
            if (quaterNumber == 1)
            {
                wageDetail.TaxableAmountSumDto.Quarter1TaxableAmt = wageDetail.GrossWages;
                wageDetail.TaxableAmountSumDto.Quarter1GrossAmt = wageDetail.GrossWages;
            }
            if (quaterNumber == 2)
            {
                wageDetail.TaxableAmountSumDto.Quarter2TaxableAmt = wageDetail.GrossWages;
                wageDetail.TaxableAmountSumDto.Quarter2GrossAmt = wageDetail.GrossWages;
            }
            if (quaterNumber == 3)
            {
                wageDetail.TaxableAmountSumDto.Quarter3TaxableAmt = wageDetail.GrossWages;
                wageDetail.TaxableAmountSumDto.Quarter3GrossAmt = wageDetail.GrossWages;
            }
            if (quaterNumber == 4)
            {
                wageDetail.TaxableAmountSumDto.Quarter4TaxableAmt = wageDetail.GrossWages;
                wageDetail.TaxableAmountSumDto.Quarter4GrossAmt = wageDetail.GrossWages;
            }


            using (DbContext context = new DbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var wageUnitDetails in wageDetail.ListWageUnitDetailDto.Where(x => x.Ssn != null))
                        {
                            wageUnitDetails.Employer = null;
                            WageUnitDetail.FromDto(context, wageUnitDetails);
                            context.SaveChanges();

                        }

                        EmployerAccountTransaction.FromDto(context, wageDetail.EmployerAccountTransactionDto);
                        context.SaveChanges();

                        TaxableAmountSum.FromDto(context, wageDetail.TaxableAmountSumDto);
                        context.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }

                }
            }

            return wageDetail;
        }


        /// <summary>
        ///  Add more 25 wageunitdetails
        /// </summary>
        /// <param name="wageDetail"></param>
        /// <returns></returns>
        [OperationMethod]
        public static WageSubmissionViewModel Add(WageSubmissionViewModel wageDetail)
        {
            int rowCount = wageDetail.ListWageUnitDetailDto.Count;

            for (int i = rowCount + 1; i <= rowCount + 25; i++)
            {
                WageSubmissionViewModel.WageUnitCustomDto wageUnitDetailDto = new WageSubmissionViewModel.WageUnitCustomDto();
                wageUnitDetailDto.SrNo = i;
                wageUnitDetailDto.EmployerId = wageDetail.Employer.EmployerId;
                wageUnitDetailDto.Employer = wageDetail.Employer;
                wageDetail.ListWageUnitDetailDto.Add(wageUnitDetailDto);
            }

            return wageDetail;
        }

        #region Private Mehtods
        /// <summary>
        /// Get Current Tax Rate.
        /// </summary>
        /// <returns></returns>
        public static decimal GetCurrentContributionRate()
        {
            DateTime currentdate = DateTimeUtil.Now;
            using (DbContext context = new DbContext())
            {

                var taxRate = (from rates in context.TaxRates
                               where rates.EffectiveBeginDate <= currentdate & currentdate <= rates.EffectiveEndDate
                               select rates._TaxRate).FirstOrDefault();


                return Convert.ToDecimal(taxRate);
            }
        }


        /// <summary>
        /// GeNext TaxableAmountSum SeqNumber
        /// </summary>
        /// <param name="emprAcctId"></param>
        /// <param name="rptYr"></param>
        /// <param name="rptQtr"></param>
        /// <returns></returns>
        public static short GetMaxTaxableAmountSumSeqNu(int emprAcctId, short rptYr)
        {
            short maxTaxableSeqNu;

            using (DbContext context = new DbContext())
            {
                maxTaxableSeqNu = context.TaxableAmountSums.Where(x => x.EmployerId == emprAcctId && x.ReportingYear == rptYr).OrderByDescending(X => X.TaxableAmountSeqNo).Select(x => x.TaxableAmountSeqNo).FirstOrDefault();
            }

            return ++maxTaxableSeqNu;
        }
        #endregion
    }


}

