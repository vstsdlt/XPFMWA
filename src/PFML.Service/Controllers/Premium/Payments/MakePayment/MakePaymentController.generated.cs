// ----------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a compiler emitter: FACTS.Framework.Analysis.Generators.OperationMethod.ControllerEmitter
//
// Changes to this file may cause incorrect behavior and will be lost when the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Mvc;
using FACTS.Framework.Service;

namespace PFML.Service.Controllers.Premium.MakePayment
{

    public class MakePaymentController : Controller
    {

        [HttpPost]
        [Route("Premium/MakePayment/GetEmployerDueAmount")]
        [OperationMethodFilter]
        public PFML.Shared.ViewModels.Premium.Payments.MakePayment.MakePaymentViewModel GetEmployerDueAmount([FromBody]int EmployerID)
        {
            return PFML.BusinessLogic.Premium.MakePayment.MakePaymentLogic.GetEmployerDueAmount(EmployerID);
        }

        [HttpPost]
        [Route("Premium/MakePayment/SavePaymentProfile")]
        [OperationMethodFilter]
        public PFML.Shared.Model.DbDtos.PaymentProfileDto SavePaymentProfile([FromBody]PFML.Shared.Model.DbDtos.PaymentProfileDto PaymentProfileDetails)
        {
            return PFML.BusinessLogic.Premium.MakePayment.MakePaymentLogic.SavePaymentProfile(PaymentProfileDetails);
        }

        [HttpPost]
        [Route("Premium/MakePayment/SavePaymentDetail")]
        [OperationMethodFilter]
        public PFML.Shared.Model.DbDtos.PaymentMainDto SavePaymentDetail([FromBody]PFML.Shared.Model.DbDtos.PaymentMainDto PaymentMainDetails)
        {
            return PFML.BusinessLogic.Premium.MakePayment.MakePaymentLogic.SavePaymentDetail(PaymentMainDetails);
        }

    }

}
