// ----------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a compiler emitter: FACTS.Framework.Analysis.Generators.OperationMethod.ProxyEmitter
//
// Changes to this file may cause incorrect behavior and will be lost when the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FACTS.Framework.Web;

namespace PFML.Web.ServiceProxies.Premium.WageDetail
{

    public static class WageSubmissionProxy
    {

        ///<summary>URL base path location for web service</summary>
        private const string servicePath = "Premium/WageDetail/WageSubmission";

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel> GetWagePeriodAsync()
        {
            using (OperationClient client = new OperationClient(servicePath, "GetWagePeriod"))
            {
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel GetWagePeriod()
        {
            return GetWagePeriodAsync().Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel> ValidateSelectionMethodAsync(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            using (OperationClient client = new OperationClient(servicePath, "ValidateSelectionMethod"))
            {
                client.Value = wageDetail;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel ValidateSelectionMethod(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            return ValidateSelectionMethodAsync(wageDetail).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel> ValidateManualEntrySubmissionAsync(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            using (OperationClient client = new OperationClient(servicePath, "ValidateManualEntrySubmission"))
            {
                client.Value = wageDetail;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel ValidateManualEntrySubmission(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            return ValidateManualEntrySubmissionAsync(wageDetail).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel> ValidateTaxAsync(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            using (OperationClient client = new OperationClient(servicePath, "ValidateTax"))
            {
                client.Value = wageDetail;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel ValidateTax(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            return ValidateTaxAsync(wageDetail).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel> AddAsync(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            using (OperationClient client = new OperationClient(servicePath, "Add"))
            {
                client.Value = wageDetail;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel Add(PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission.WageSubmissionViewModel wageDetail)
        {
            return AddAsync(wageDetail).Result;
        }

    }

}
