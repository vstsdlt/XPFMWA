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

namespace PFML.Web.ServiceProxies.Premium.Waiver.VPRequest
{

    public static class VPRequestProxy
    {

        ///<summary>URL base path location for web service</summary>
        private const string servicePath = "Premium/Waiver/VPRequest";

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto>> GetListAsync(int EmployerId)
        {
            using (OperationClient client = new OperationClient(servicePath, "GetList"))
            {
                client.Value = EmployerId;
                return await client.ReadAsync<System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto>>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto> GetList(int EmployerId)
        {
            return GetListAsync(EmployerId).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto> GetRequestAsync(int RequestId)
        {
            using (OperationClient client = new OperationClient(servicePath, "GetRequest"))
            {
                client.Value = RequestId;
                return await client.ReadAsync<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto GetRequest(int RequestId)
        {
            return GetRequestAsync(RequestId).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto>> SubmitRequestAsync(PFML.Shared.ViewModels.Premium.Waiver.VPRequestForm VPRequestForm)
        {
            using (OperationClient client = new OperationClient(servicePath, "SubmitRequest"))
            {
                client.Value = VPRequestForm;
                return await client.ReadAsync<System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto>>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static System.Collections.Generic.List<PFML.Shared.Model.DbDtos.VoluntaryPlanWaiverRequestDto> SubmitRequest(PFML.Shared.ViewModels.Premium.Waiver.VPRequestForm VPRequestForm)
        {
            return SubmitRequestAsync(VPRequestForm).Result;
        }

        ///<summary>ASYNC Web Service call: unknown</summary>
        ///<returns>ASYNC task: unknown</returns>
        public static async Task<PFML.Shared.ViewModels.Premium.Waiver.VPRequestForm> SetDataAsync(int EmployerId)
        {
            using (OperationClient client = new OperationClient(servicePath, "SetData"))
            {
                client.Value = EmployerId;
                return await client.ReadAsync<PFML.Shared.ViewModels.Premium.Waiver.VPRequestForm>();
            }
        }

        ///<summary>Web Service call: unknown</summary>
        public static PFML.Shared.ViewModels.Premium.Waiver.VPRequestForm SetData(int EmployerId)
        {
            return SetDataAsync(EmployerId).Result;
        }

    }

}
