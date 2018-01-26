using FACTS.Framework.Service;
using PFML.Shared.ViewModels.Premium.Payments.MakePayment;
using DbContext = PFML.DAL.Model.DbContext;
using System.Linq;
using PFML.Shared.Model.DbDtos;
using System.Collections.Generic;
using System;
using PFML.Shared.Utility;
using PFML.DAL.Model.DbEntities;
using FACTS.Framework.Lookup;
using PFML.Shared.LookupTable;
using System.Data.Entity;

namespace PFML.BusinessLogic.Premium.MakePayment
{
    public static class MakePaymentLogic
    {
        /// <summary>
        /// This method will fetch the Employer details, payment details on the basis of Employer ID
        /// </summary>
        /// <param name="EmployerID"></param>
        /// <returns></returns>
        [OperationMethod]
        public static MakePaymentViewModel GetEmployerDueAmount(int EmployerID)
        {
            MakePaymentViewModel LocalPaymentViewModel = new MakePaymentViewModel();
            EmployerDto localEmployerDto = new EmployerDto();
            List<EmployerAccountTransactionDto> colEmployerAccountTransactionDto = new List<EmployerAccountTransactionDto>();
            List<PaymentMainDto> colPaymentMainDto = new List<PaymentMainDto>();
            List<PaymentProfileDto> colPaymentProfileDto = new List<PaymentProfileDto>();

            using (DbContext context = new DbContext())
            {

                var localEmployer = context.Employers.FirstOrDefault();
                if (!(localEmployer is null))
                {
                    EmployerID = localEmployer.EmployerId;

                    localEmployerDto = localEmployer.ToDto();
                    LocalPaymentViewModel.EmployerDto = localEmployerDto;

                    var localEmployerAccountTransactionsItems = context.EmployerAccountTransactions.Where(x => x.EmployerId == EmployerID).OrderByDescending(y => y.EmployerId);
                    foreach (var localEmployerAccountTransactionsItem in localEmployerAccountTransactionsItems)
                    {
                        colEmployerAccountTransactionDto.Add(localEmployerAccountTransactionsItem.ToDto());
                    }
                    LocalPaymentViewModel.CollectionEmployerAccountTransactionDto = colEmployerAccountTransactionDto;

                    var localPaymentMainDtoItems = context.PaymentMains.Where(x => x.EmployerId == EmployerID).OrderByDescending(y => y.PaymentMainId);
                    foreach (var localPaymentMainDtoItem in localPaymentMainDtoItems)
                    {
                        colPaymentMainDto.Add(localPaymentMainDtoItem.ToDto());
                    }
                    LocalPaymentViewModel.CollectionPaymentMainDto = colPaymentMainDto;


                    var localPaymentProfileDtoItems = context.PaymentProfiles.Where(x => x.EmployerId == EmployerID).OrderByDescending(y => y.PaymentProfileId).FirstOrDefault();
                    if (localPaymentProfileDtoItems != null)
                    { colPaymentProfileDto.Add(localPaymentProfileDtoItems.ToDto()); }
                    LocalPaymentViewModel.CollectionPaymentProfileDto = colPaymentProfileDto;

                    CalculateAmount(ref LocalPaymentViewModel);
                }
            }

            return LocalPaymentViewModel;
        }

        /// <summary>
        /// This method saves Payment Profile Information
        /// </summary>
        /// <param name="PaymentProfileDetails"></param>
        /// <returns>bool</returns>
        [OperationMethod]
        public static PaymentProfileDto SavePaymentProfile(PaymentProfileDto PaymentProfileDetails)
        {
            //DeletePreviousPaymentProfile(PaymentProfileDetails);
            using (DbContext context = new DbContext())
            {
                PaymentProfile localPaymentProfile = new PaymentProfile()
                {
                    EmployerId = PaymentProfileDetails.EmployerId,
                    PaymentTypeCode = PaymentProfileDetails.PaymentTypeCode,
                    PaymentAccountTypeCode = PaymentProfileDetails.PaymentAccountTypeCode,
                    BankAccountNumber = PaymentProfileDetails.BankAccountNumber,
                    RoutingTransitNumber = PaymentProfileDetails.RoutingTransitNumber
                };
                context.PaymentProfiles.Add(localPaymentProfile);
                context.SaveChanges();
                return localPaymentProfile.ToDto();
            }
        }



        /// <summary>
        /// This method saves Payment done into Payment table
        /// </summary>
        /// <param name="PaymentMainDetails"></param>
        /// <returns>bool</returns>
        [OperationMethod]
        public static PaymentMainDto SavePaymentDetail(PaymentMainDto PaymentMainDetails)
        {
            using (DbContext context = new DbContext())
            {
                PaymentMain localPaymentDetail = new PaymentMain()
                {
                    IsAgent = false,
                    EmployerId = PaymentMainDetails.EmployerId,
                    PaymentMethodCode = PaymentMainDetails.PaymentMethodCode,
                    PaymentAmount = PaymentMainDetails.PaymentAmount,
                    PaymentTransactionDate = PaymentMainDetails.PaymentTransactionDate,
                    RoutingTransitNumber = PaymentMainDetails.RoutingTransitNumber,
                    BankAccountNumber = PaymentMainDetails.BankAccountNumber,
                    BankAccountTypeCode = PaymentMainDetails.BankAccountTypeCode,
                    PaymentStatusCode = PaymentMainDetails.PaymentStatusCode,
                    PaymentStatusDate = PaymentMainDetails.PaymentStatusDate,
                    PaymentSubmittedDate = PaymentMainDetails.PaymentSubmittedDate
                };
                context.PaymentMains.Add(localPaymentDetail);
                context.SaveChanges();
                return localPaymentDetail.ToDto();
            }
        }

        #region Private Methods

        private static void DeletePreviousPaymentProfile(PaymentProfileDto paymentProfileDetails)
        {
            using (DbContext context = new DbContext())
            {
                //int paymentProfileID = context.PaymentProfiles.Where(x => x.EmployerId == paymentProfileDetails.EmployerId).Max(u => u.PaymentProfileId);
                //var updateTime = context.PaymentProfiles.Where(x => x.PaymentProfileId == paymentProfileID).Select(y => y.UpdateDateTime).FirstOrDefault();
                //var payment = new PaymentProfile { EmployerId = paymentProfileDetails.EmployerId, PaymentProfileId = paymentProfileID, UpdateDateTime = updateTime };
                //context.Entry(payment).State = EntityState.Deleted;
                //context.SaveChanges();
                var EntityObject = context.PaymentProfiles.Where(x => x.EmployerId == paymentProfileDetails.EmployerId).FirstOrDefault();
                if (EntityObject != null)
                {
                    context.Entry(EntityObject).State = EntityState.Modified;
                    //context.PaymentProfiles.u(EntityObject);
                    context.SaveChanges();
                }
            }
        }

        private static void CalculateAmount(ref MakePaymentViewModel localPaymentViewModel)
        {
            short rptYr = Convert.ToInt16(DateTime.Now.Year);
            string rptQtr = DateUtil.GetQuarter(DateTime.Now.Month);
            DateTime quarterEndDt = DateUtil.GetLastDayOfQuarter(rptYr, rptQtr);

            decimal postDatedPmtAm = 0;
            postDatedPmtAm = GetPostDatedPayment(localPaymentViewModel, quarterEndDt);

            decimal outstandingAmt = GetTransactionBalance(localPaymentViewModel, quarterEndDt);
            localPaymentViewModel.AmountDue = outstandingAmt - postDatedPmtAm;
            //TO DO
            localPaymentViewModel.PrePaymentAmount = 0;
        }

        /// <summary>
        /// Return Post Dated Payment Amount
        /// </summary>
        /// <param name="localPaymentViewModel"></param>
        /// <param name="startDt"></param>
        /// <param name="quarterEndDt"></param>
        /// <returns>returns Payment Amount.</returns>
        private static decimal GetPostDatedPayment(MakePaymentViewModel localPaymentViewModel, DateTime quarterEndDt)
        {
            var EmployerID = localPaymentViewModel.EmployerDto.EmployerId;
            decimal PaymentMainAmount;
            if (quarterEndDt != null)
            {
                var colPaymentMainDetails = localPaymentViewModel.CollectionPaymentMainDto
                    .Where(x => x.EmployerId == EmployerID
                           && (x.PaymentStatusCode == Constants.Payment_Status_Pending | x.PaymentStatusCode == Constants.Payment_Status_Processed)
                           && x.PaymentTransactionDate < quarterEndDt.AddDays(1).Date
                           && x.PaymentMethodCode == LookupUtil.GetLookupCode(LookupTable.PaymentMethodType, LookupTable_PaymentMethodType.ACHDebit).Code
                            ).ToList();


                PaymentMainAmount = colPaymentMainDetails.Sum(y => y.PaymentAmount);

                return (PaymentMainAmount > 0) ? PaymentMainAmount : 0;
            }
            return 0;
        }

        /// <summary>
        /// Return transaction balance for a given employer.
        /// </summary>
        /// <param name="localPaymentViewModel"></param>
        /// <param name="startDt"></param>
        /// <param name="quarterEndDt"></param>
        /// <returns>returns transaction balance.</returns>
        public static decimal GetTransactionBalance(MakePaymentViewModel localPaymentViewModel, DateTime quarterEndDt)
        {
            var EmployerID = localPaymentViewModel.EmployerDto.EmployerId;
            decimal OwedSum;
            if (quarterEndDt != null)
            {

                var amountDetails = localPaymentViewModel.CollectionEmployerAccountTransactionDto
                    .Where(x => x.EmployerId == EmployerID
                           && x.CreateDateTime.Date < quarterEndDt.AddDays(1)
                            ).ToList();

                OwedSum = amountDetails.Sum(y => y.OwedAmount);
                return (amountDetails != null) ? (OwedSum) : 0;

            }

            return 0;
        }

        #endregion


    }

}

