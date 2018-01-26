// ----------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a compiler emitter: FACTS.Framework.Analysis.Generators.DAL.DtoEmitter
//
// Changes to this file may cause incorrect behavior and will be lost when the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FACTS.Framework.Dto;

namespace PFML.Shared.Model.DbDtos
{

    /// <summary>DTO object: [PaymentProfile]</summary>
    [Serializable]
    public class PaymentProfileDto : BaseDto
    {

        /// <summary>[AgentId]</summary>
        public int? AgentId { get; set; }

        /// <summary>[BankAccountNumber]</summary>
        public string BankAccountNumber { get; set; }

        /// <summary>[CreateDateTime]</summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>[CreateUserId]</summary>
        public string CreateUserId { get; set; }

        /// <summary>[EmployerId]</summary>
        public int? EmployerId { get; set; }

        /// <summary>[PaymentAccountTypeCode]</summary>
        public string PaymentAccountTypeCode { get; set; }

        /// <summary>[PaymentProfileId]</summary>
        public int PaymentProfileId { get; set; }

        /// <summary>[PaymentTypeCode]</summary>
        public string PaymentTypeCode { get; set; }

        /// <summary>[RoutingTransitNumber]</summary>
        public string RoutingTransitNumber { get; set; }

        /// <summary>[UpdateDateTime]</summary>
        public DateTime UpdateDateTime { get; set; }

        /// <summary>[UpdateNumber]</summary>
        public int? UpdateNumber { get; set; }

        /// <summary>[UpdateProcess]</summary>
        public string UpdateProcess { get; set; }

        /// <summary>[UpdateUserId]</summary>
        public string UpdateUserId { get; set; }

        //Navigational properties
        public virtual EmployerDto Employer { get; set; }

    }

}
