using PFML.Shared.LookupTable;
using PFML.Shared.Model.DbDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.ViewModels.Premium.Registration
{
    [Serializable]
    public class EmployerRegistrationViewModel
    {
        public EmployerRegistrationViewModel()
        {
            EmployerDto = new EmployerDto() { EmployerLiability = new EmployerLiabilityDto(), EmployerPreference = new EmployerPreferenceDto(), IsProfessionalEmployerOrg=false, IsClientOfPEO=false, IsIndividualContractor=false, IsExemptUnderIRS501C3=false, IsApplyingForREIM=false, IsAcquired=false, IsPresentInMultipleLoc=false };
            EmployerContactDto = new EmployerContactDto();
            EmployerUnitDto = new EmployerUnitDto();
            ListAddressLinkDto = new List<AddressLinkDto> { new AddressLinkDto() };

        }
        public EmployerDto EmployerDto { get; set; }
        public EmployerContactDto EmployerContactDto { get; set; }
        public EmployerUnitDto EmployerUnitDto { get; set; }
        public List<AddressLinkDto> ListAddressLinkDto { get; set; }
    }
}
