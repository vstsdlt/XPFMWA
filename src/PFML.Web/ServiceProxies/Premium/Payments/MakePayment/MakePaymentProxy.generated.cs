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

namespace PFML.Web.ServiceProxies.Premium.MakePayment
{

    public static class MakePaymentProxy
    {

        ///<summary>URL base path location for web service</summary>
        private const string servicePath = "Premium/MakePayment";

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.Payments.MakePayment.MakePaymentViewModel> GetEmployerDueAmountAsync(int emprAccountID)
        {
            using (OperationClient client = new OperationClient(servicePath, "GetEmployerDueAmount"))
            {
                client.Value = emprAccountID;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.Payments.MakePayment.MakePaymentViewModel>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.Payments.MakePayment.MakePaymentViewModel GetEmployerDueAmount(int emprAccountID)
        {
            return GetEmployerDueAmountAsync(emprAccountID).Result;
        }

    }

}
