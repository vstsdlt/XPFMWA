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

    /// <summary>DTO object: [VoluntaryPlanWaiverRequest]</summary>
    [Serializable]
    public class VoluntaryPlanWaiverRequestDto : FormDto
    {



        /// <summary>[EmployerId]</summary>
        public int EmployerId { get; set; }

        /// <summary>[EndDate]</summary>
        public DateTime EndDate { get; set; }


        /// <summary>[IsVoluntaryPlanWaiverRequestAcknowledged]</summary>
        public bool IsVoluntaryPlanWaiverRequestAcknowledged { get; set; }

        /// <summary>[StartDate]</summary>
        public DateTime StartDate { get; set; }





        //Navigational properties
        public virtual EmployerDto Employer { get; set; }

        //Reverse navigational properties
        public virtual List<VoluntaryPlanWaiverRequestTypeDto> VoluntaryPlanWaiverRequestTypes { get; private set; } = new List<VoluntaryPlanWaiverRequestTypeDto>();

    }

}