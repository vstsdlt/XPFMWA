using FACTS.Framework.Lookup;
using FACTS.Framework.Web;
using PFML.Shared.LookupTable;
using System;
using System.Collections.Generic;
using PFML.Shared.Model.DbDtos;
using PFML.Shared.ViewModels.Premium.Payments.MakePayment;

namespace PFML.Web.Controllers.Premium.Payments
{
    /// <summary>
    /// Make Payment Contoller Method
    /// </summary>
    public class MakePaymentController : Controller
    {

        private const int unMaskedDigitCount = 4;
        private const char maskChar = '*';
        //check for lookup
        private const string dueDateDetailsText = "<b>Quarter 1 - April 30<br/>Quarter 2 - July 31<br/>Quarter 3 - October 31<br/>Quarter 4 - January 31</b>";
        //move to cshtml
        private const string prepaymentDueDates = "<b>10 calendar days after the start of the quarter</b>";

        public void MachineExecute()
        {
            //TO DO
            //Get Values from the DB Context
            Machine["emprAccountID"] = 1;
            Machine["emprName"] = "Test Employer";
            Machine["dbaName"] = "Test DBA";

            Machine["ListSection"] = LookupUtil.GetValues(LookupTable.MakePaymentWizardFlow, null, "{DisplaySortOrder}", null);
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.MakePaymentWizardFlow, LookupTable_MakePaymentWizardFlow.SelectPaymentMethod);
            Machine["ShowHeader"] = true;
        }


        public void SelectPaymentMethodExecute()
        {
            MakePaymentViewModel employerPaymentDetailsMain = (MakePaymentViewModel)Machine["EmployerPaymentDetails"];
            decimal TotalAmountDue = 0;
            decimal AmountDue = 0;
            if (employerPaymentDetailsMain != null)
            {
                TotalAmountDue = employerPaymentDetailsMain.AmountDue + employerPaymentDetailsMain.PrePaymentAmount;
                AmountDue = employerPaymentDetailsMain.AmountDue;
            }

            Machine["PaymentDueDates"] = dueDateDetailsText;
            Machine["PrepaymentDueDates"] = prepaymentDueDates;

            //date util method 
            Machine["ActualDueDate"] = DateTime.Today;
            Machine["AmountDue"] = AmountDue;
            Machine["TotalAmount"] = TotalAmountDue;
            Machine["PmtAmount"] = TotalAmountDue;


            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.MakePaymentWizardFlow, LookupTable_MakePaymentWizardFlow.SelectPaymentMethod);
            Machine["CheckPayment"] = Machine["SearchType"];
        }

        public void SelectPaymentMethodNext()
        {
            decimal pmtAmount = Convert.ToDecimal(Machine["PmtAmount"]);
            decimal totalAmountDue = Convert.ToDecimal(Machine["TotalAmount"]);
            string pmtMethod = Machine["CheckPaymentMethodType"].ToString();
            string OnlinePaymentMthod = LookupUtil.GetLookupCode(LookupTable.PaymentMethodType, LookupTable_PaymentMethodType.ACHDebit).Code;

            Machine["CheckPayment"] = ((totalAmountDue - pmtAmount) > 0) ? "Partial" : "Full";
            Machine["CheckPaymentMethodType"] = (pmtMethod.Equals(OnlinePaymentMthod, StringComparison.InvariantCultureIgnoreCase)) ? "Online" : "Paper";
        }

        public void OnlinePaymentStart()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.MakePaymentWizardFlow, LookupTable_MakePaymentWizardFlow.SubmitPaymentDetails);
        }

        public void PaymentDetailsExecute()
        {
            List<AccountType> listAccountType = new List<AccountType>();
            AccountType actype = new AccountType
            {
                AccountTypeName = "Savings"
            };
            listAccountType.Add(actype);
            actype.AccountTypeName = "Checking";
            listAccountType.Add(actype);
            //TO DO
            /*  If Employer does not have current period (based on Employer Payment Method: Contributory or Reimbursable) debt and effective date is greater than current date, 
             *  System shall not allow a payment to be scheduled for a future date and display Standard Message “You cannot post date this payment as you owe prior debt that is past due”*/
            Machine["PymtEffectiveDt"] = DateTime.Now;
            Machine["AccountTypeDDL"] = string.Empty;

        }
        public void PaymentDetailsNext()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.MakePaymentWizardFlow, LookupTable_MakePaymentWizardFlow.SubmitPaymentDetails);
            if (Machine["SaveBankInformation"] != null && Machine["SaveBankInformation"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                Machine["SaveBankInformationText"] = "Yes";
            }
            else
            {
                Machine["SaveBankInformationText"] = "No";
            }
            Machine["PaymentEffectiveDate"] = DateTime.Today;
            Machine["AccountType"] = Machine["AccountTypeDDL"].ToString().Equals(LookupTable_BankAccountType.Savings, StringComparison.InvariantCultureIgnoreCase) ? "Savings" : "Checking";
            //TO DO
            //Get Values from the DB Context
            Machine["PmtVerificationMsg"] = String.Format("By paying your State bill by way of this online service, you are authorizing State to charge your {0} account for the amount you submitted.", Machine["AccountType"]);
        }

        public void PaymentVerificationExecute()
        {
            Machine["MaskedTransitNumber"] = MaskConfidentialNumbers(Machine["RoutTranNumber"].ToString(), Machine["RoutTranNumber"].ToString().Length - unMaskedDigitCount);
            Machine["MaskedAccountNumber"] = MaskConfidentialNumbers(Machine["BankAccNumber"].ToString(), Machine["BankAccNumber"].ToString().Length - unMaskedDigitCount);
        }

        public void PaymentConfirmationExecute()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.MakePaymentWizardFlow, LookupTable_MakePaymentWizardFlow.Complete);
            decimal pmtAmount = Convert.ToDecimal(Machine["PmtAmount"]);
            decimal totalAmountDue = Convert.ToDecimal(Machine["TotalAmount"]);
            Machine["BalanceAmount"] = (totalAmountDue - pmtAmount);
        }

        public void PaperCheckVoucherExecute()
        {
            //TO DO
            //Get Values from the DB Context
            Machine["PmtAmountPaperVoucher"] = Convert.ToDecimal(Machine["PmtAmount"]);
            Machine["emprAccountIDPaperVoucher"] = Machine["emprAccountID"].ToString() + " (must be written on check)";
            Machine["ChecksPayableAt"] = "NM Department of Workforce Solutions";
            Machine["MailingAddress"] = "PO BOX 2281 AlbuQuerque, NM 87103";
        }

        #region Private Methods

        private string MaskConfidentialNumbers(string confNumber, int length)
        {
            return string.Format("{0}{1}", new string(maskChar, length), confNumber.Substring(length, 4));
        }


        #endregion
    }

    /// <summary>
    /// Get Account Type
    /// </summary>
    [Serializable]
    public class AccountType
    {
        public string AccountTypeName { get; set; }
    }
}
