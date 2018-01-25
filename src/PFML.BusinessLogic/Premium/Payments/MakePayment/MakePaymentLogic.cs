using FACTS.Framework.Service;
using PFML.Shared.ViewModels.Premium.Payments.MakePayment;
using DbContext = PFML.DAL.Model.DbContext;
using System.Linq;
using PFML.Shared.Model.DbDtos;
using System.Collections.Generic;
using System;
using PFML.Shared.Utility;

namespace PFML.BusinessLogic.Premium.MakePayment
{
    public static class MakePaymentLogic
    {
        /// <summary>
        /// This method will fetch the Employer details, payment details on the
        /// basis of Employer Account ID
        /// </summary>
        /// <param name="emprAccountID"></param>
        /// <returns></returns>
        [OperationMethod]
        public static MakePaymentViewModel GetEmployerDueAmount(int emprAccountID)
        {
            MakePaymentViewModel LocalPaymentViewModel = new MakePaymentViewModel();
            EmployerDto localEmployerDto = new EmployerDto();
            List<EmployerAccountTransactionDto> colEmployerAccountTransactionDto = new List<EmployerAccountTransactionDto>();
            List<PaymentMainDto> colPaymentMainDto = new List<PaymentMainDto>();
            List<PaymentProfileDto> colPaymentProfileDto = new List<PaymentProfileDto>();

            using (DbContext context = new DbContext())
            {
                localEmployerDto = context.Employers.Where(x => x.EmployerId == emprAccountID).FirstOrDefault().ToDto();
                LocalPaymentViewModel.EmployerDto = localEmployerDto;


                var localEmployerAccountTransactionsItems = context.EmployerAccountTransactions.Where(x => x.EmployerId == emprAccountID);
                foreach (var localEmployerAccountTransactionsItem in localEmployerAccountTransactionsItems)
                {
                    colEmployerAccountTransactionDto.Add(localEmployerAccountTransactionsItem.ToDto());
                }
                LocalPaymentViewModel.CollectionEmployerAccountTransactionDto = colEmployerAccountTransactionDto;

                var localPaymentMainDtoItems = context.PaymentMains.Where(x => x.EmployerId == emprAccountID);
                foreach (var localPaymentMainDtoItem in localPaymentMainDtoItems)
                {
                    colPaymentMainDto.Add(localPaymentMainDtoItem.ToDto());
                }
                LocalPaymentViewModel.CollectionPaymentMainDto = colPaymentMainDto;


                var localPaymentProfileDtoItems = context.PaymentProfiles.Where(x => x.EmployerId == emprAccountID);
                foreach (var localPaymentProfileDtoItem in localPaymentProfileDtoItems)
                {
                    colPaymentProfileDto.Add(localPaymentProfileDtoItem.ToDto());
                }
                LocalPaymentViewModel.CollectionPaymentProfileDto = colPaymentProfileDto;

                CalculateAmount(ref LocalPaymentViewModel);
            }

            return LocalPaymentViewModel;
        }

        private static void CalculateAmount(ref MakePaymentViewModel localPaymentViewModel)
        {
            //decimal localAmountDue = GetOutStandingBalanceWithoutDefAm(localPaymentViewModel.EmployerDto.EmployerId);

            short rptYr = Convert.ToInt16(DateTime.Now.Year);
            string rptQtr = DateUtil.GetQuarter(DateTime.Now.Month);
            DateTime quarterEndDt = DateUtil.GetLastDayOfQuarter(rptYr, rptQtr);

            DateTime startDt = DateTime.Today;
            decimal postDatedPmtAm = 0;
            postDatedPmtAm = GetPostDatedPayment(localPaymentViewModel, startDt, quarterEndDt);
            decimal outstandingAmt = GetTransactionBalance(localPaymentViewModel, startDt, quarterEndDt);
            localPaymentViewModel.AmountDue = outstandingAmt;
        }

        private static decimal GetPostDatedPayment(MakePaymentViewModel localPaymentViewModel, DateTime startDt, DateTime quarterEndDt)
        {
            var EmployerID = localPaymentViewModel.EmployerDto.EmployerId;
            decimal PaymentMainAmount;
            var colPaymentMainDetails = localPaymentViewModel.CollectionPaymentMainDto
                .Where(x => x.EmployerId == EmployerID
                && x.PaymentStatusCode == "UNPD"
                && x.PaymentTransactionDate >= startDt.Date
            && x.PaymentTransactionDate < quarterEndDt.AddDays(1).Date && x.PaymentMethodCode == "ACHD").ToList();


            PaymentMainAmount = colPaymentMainDetails.Sum(y => y.PaymentAmount);

            return (PaymentMainAmount > 0) ? PaymentMainAmount : 0;
        }

        /// <summary>Return transaction balance for a given employer.</summary>
        /// <param name="emprAcctId">Employer Accout ID.</param>
        /// <param name="quarterEndDt">Quarter end date.</param>
        /// <returns>returns transaction balance.</returns>
        public static decimal GetTransactionBalance(MakePaymentViewModel localPaymentViewModel, Nullable<DateTime> startDt, DateTime quarterEndDt)
        {
            var EmployerID = localPaymentViewModel.EmployerDto.EmployerId;
            decimal OwedSum, UnpaidAmt;
            if (quarterEndDt != null)
            {

                var amountDetails = localPaymentViewModel.CollectionEmployerAccountTransactionDto
                    .Where(x => x.EmployerId == EmployerID
                    && x.CreateDateTime.Date >= startDt
                    && x.CreateDateTime.Date < quarterEndDt.AddDays(1)
                    ).ToList();

                OwedSum = amountDetails.Sum(y => y.OwedAmount);
                UnpaidAmt = amountDetails.Sum(z => z.UnpaidAmount);
                return (amountDetails != null) ? (OwedSum - UnpaidAmt) : 0;

            }

            return 0;
        }
    }

}

