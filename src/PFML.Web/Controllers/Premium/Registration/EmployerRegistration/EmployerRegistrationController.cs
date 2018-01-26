using System;
using System.Collections.Generic;
using FACTS.Framework.Web;
using FACTS.Framework.Lookup;
using FACTS.Framework.Support;
using PFML.Shared.LookupTable;
using PFML.Shared.Model.DbDtos;
using PFML.Shared.ViewModels.Premium.Registration;
using PFML.Shared.Utility;

namespace PFML.Web.Controllers.Premium.Registration
{

    public class EmployerRegistrationController : Controller
    {
        /// <summary>
        /// Method executes during page load
        /// </summary>
        public void MachineExecute()
        {
            //used to set wizard header
            Machine["ListSection"] = LookupUtil.GetValues(LookupTable.WizEmployerRegistration, null, "{DisplaySortOrder}", null);
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterLiabilityInformation);
            Machine["ShowHeader"] = false;

            //initialize EmployerRegistration object and required address objects
            Machine["EmployerRegistration"] = new EmployerRegistrationViewModel();
            Machine["EntityAddress"] = new AddressLinkDto() { Address = new AddressDto() { StateCode = LookupTable_StatePrvnc.Washington, CountryCode = LookupTable_Country.UnitedStates } ,AddressTypeCode=LookupTable_AddressType.Mailing};
            Machine["PhysicalAddress"] = new AddressLinkDto() { Address = new AddressDto() { StateCode = LookupTable_StatePrvnc.Washington, CountryCode = LookupTable_Country.UnitedStates }, AddressTypeCode = LookupTable_AddressType.Physical };
            Machine["BusinessAddress"] = new AddressLinkDto() { Address = new AddressDto() { StateCode = LookupTable_StatePrvnc.Washington, CountryCode = LookupTable_Country.UnitedStates }, AddressTypeCode = LookupTable_AddressType.Business };
            Machine["YearList"] = DateUtil.PopulateYears(Convert.ToInt16(LookupTable_WageDetailUnitYears.YearsForWageFiling), 0, false);
        }

        #region Wizard Navigation methods
        public void ValidateIntroductionNext()
        {
            Machine["ShowHeader"] = true;
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterLiabilityInformation);
        }

        public void LiabilityWagesPrevious()
        {
            Machine["ShowHeader"] = false;
        }

        public void IsLiableWeeksCheckTrue()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterUsers);
        }

        public void AdminInfoPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterLiabilityInformation);
        }

        public void ValidateAdminInfoNext()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterEmployerInformation);
        }

        public void EmpIdentificationPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterUsers);
        }

        public void EnterBusinessInfoPrevious()
        {
            Machine["IsPublicEntity"] = false;
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterEmployerInformation);
        }

        public void RegistrationSummaryPrevious()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterBusinessInformation);
        }

        public void SubmitRegistrationNext()
        {
            Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.Complete);
            
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            DateTime liabilityDate = Convert.ToDateTime(employerRegistration.EmployerDto.LiabilityDate);
            int currentQuarter = (liabilityDate.Month - 1) / 3 + 1;
            Machine["LastDayCurrentQuarter"] = new DateTime(liabilityDate.Year, 3 * currentQuarter + 1, 1).AddDays(-1);
        }
        #endregion

        #region UI Validations
        /// <summary>
        /// Validates 'Introduction' wizard step
        /// </summary>
        public void IntroductionNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            if (employerRegistration.EmployerDto.IsServiceBegin == false)
            {
                Context.ValidationMessages.AddError("You are not required to register at this time because you do not have employment in this state. Return and register once employment begins.");
            }
            if (employerRegistration.EmployerDto.IsServiceBegin == true && (employerRegistration.EmployerDto.ServiceBeginDate > employerRegistration.EmployerUnitDto.FirstWageDate))
            {
                Context.ValidationMessages.AddError("'If yes, enter the date this employer first paid wages to those individuals performing services in State' must be equal to or after 'If yes, enter the date services were first performed for the employer in State'");
            }
        }

        /// <summary>
        /// Validates 'Liability Wages' wizard step
        /// </summary>
        public void LiabilityWagesNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            List<bool?> listUserInput = new List<bool?> { employerRegistration.EmployerDto.EmployerLiability.HasPaid450RegularWages, employerRegistration.EmployerDto.EmployerLiability.HasPaid1KDomesticWages, employerRegistration.EmployerDto.EmployerLiability.HasPaid20KAgriculturalLaborWages};
            if (listUserInput.FindAll(l => l.Equals(true))?.Count >= 2)
            {
                Context.ValidationMessages.AddError("Only one answer can be Yes. Select the question that occurred first as Yes and the other(s) as No.");
            }
            else if (listUserInput.FindAll(l => l.Equals(true))?.Count >= 1)
            {
                Machine["IsLiableWages"] = true;
                Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterUsers);
            }
            else
            {
                Machine["IsLiableWages"] = false;
                employerRegistration.EmployerDto.EmployerLiability.LiabilityAmountMetYear = null;
                employerRegistration.EmployerDto.EmployerLiability.LiabilityAmountMetQuarter = null;
            }
        }

        /// <summary>
        /// Validates 'Liability Weeks' wizard step
        /// </summary>
        public void LiabilityWeeksNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            List<bool?> listUserInput = new List<bool?> { employerRegistration.EmployerDto.EmployerLiability.HasEmployed1In20Weeks, employerRegistration.EmployerDto.EmployerLiability.HasEmployed10In20Weeks };
            if (listUserInput.FindAll(l => l.Equals(true))?.Count == 2)
            {
                Context.ValidationMessages.AddError("Only one answer can be Yes. Select the question that occurred first as Yes and the other(s) as No.");
                return;
            }
            else
            {
                Machine["IsLiableWeeks"] = (bool)employerRegistration.EmployerDto.EmployerLiability.HasEmployed1In20Weeks || (bool)employerRegistration.EmployerDto.EmployerLiability.HasEmployed10In20Weeks;
            }
        }

        /// <summary>
        /// Validates 'Administrator Info' wizard step
        /// </summary>
        public void AdminInfoNext()
        {
            string email = Machine["EmployerRegistration.EmployerContactDto.Email"]?.ToString();
            string reEmail = Machine["ReEmail"]?.ToString();
            ValidateEmail(email, reEmail);
        }

        /// <summary>
        /// Validates 'Employer Identification' wizard step
        /// </summary>
        public void EmpIdentificationNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            if (employerRegistration.EmployerDto.EmployerLiability.HasPaid450RegularWages==true || employerRegistration.EmployerDto.EmployerLiability.HasEmployed1In20Weeks==true)
            {
                employerRegistration.EmployerDto.BusinessTypeCode = LookupTable_BusinessType.NonAgriculturalRegularEmployment;
            }
            if (employerRegistration.EmployerDto.EmployerLiability.HasPaid20KAgriculturalLaborWages==true || employerRegistration.EmployerDto.EmployerLiability.HasEmployed10In20Weeks==true)
            {
                employerRegistration.EmployerDto.BusinessTypeCode = LookupTable_BusinessType.Agricultural;
            }
            if (employerRegistration.EmployerDto.EmployerLiability.HasPaid1KDomesticWages==true)
            {
                employerRegistration.EmployerDto.BusinessTypeCode = LookupTable_BusinessType.Domestic;
            }
            string entityTypeCode = employerRegistration.EmployerDto.EntityTypeCode;
            Machine["IsPublicEntity"] = entityTypeCode.Equals(LookupTable_LegalEntity.City) || entityTypeCode.Equals(LookupTable_LegalEntity.GovernmentStateAgency) || entityTypeCode.Equals(LookupTable_LegalEntity.LocalPublicBody) || entityTypeCode.Equals(LookupTable_LegalEntity.USMilitary) || entityTypeCode.Equals(LookupTable_LegalEntity.County) ? true : false;
            ValidateAddress((AddressDto)Machine["EntityAddress.Address"], Machine["LegalAddrReEmail"]?.ToString());

            if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) == 0)
            {
                Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.EnterBusinessInformation);
            }
            
        }

        /// <summary>
        /// Validates 'Business Information Continued' wizard step
        /// </summary>
        public void EnterBusinessInfoContNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            if ((bool)Machine["IsPublicEntity"] && employerRegistration.EmployerDto.IsExemptUnderIRS501C3==true)
            {
                Context.ValidationMessages.AddError("'Is this employer a non-profit organization (hospitals, schools, municipalities & counties) that holds an exemption from federal income taxes?' cannot be 'Yes' for the selected Legal Entity Type.");
            }
            if (!employerRegistration.EmployerDto.IsExemptUnderIRS501C3==true && employerRegistration.EmployerDto.IsApplyingForREIM==true)
            {
                Context.ValidationMessages.AddError("Business type is not eligible for Reimbursable Cost Basis Financing.");
            }
            if (!employerRegistration.EmployerDto.IsPresentInMultipleLoc==true && !(employerRegistration.EmployerDto.NoOfLocation is null))
            {
                Context.ValidationMessages.AddError("'If yes, how many?' cannot be provided if 'Is there more than one physical location in State?' is selected as 'No'.");
            }
        }

        /// <summary>
        /// Validates 'Physical Address' wizard step
        /// </summary>
        public void EnterPhysicalAddrNext()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            var physicalAddress = (AddressLinkDto)Machine["PhysicalAddress"];
            if (employerRegistration.EmployerDto.HasPhysicalLocation == false && Machine["SameAsPhyAddr"]?.ToString().Equals(LookupTable_AddressType.Mailing) == true)
            {
                Context.ValidationMessages.AddError("Cannot select mailing address if no physical address in Washington is selected.");
                return;
            }
            else if (employerRegistration.EmployerDto.HasPhysicalLocation == false && (!(physicalAddress?.Address?.AddressLine1 is null) || !(physicalAddress?.Address?.AddressLine2 is null) || !(physicalAddress?.Address?.City is null) || !(physicalAddress?.Address?.Zip is null) || !(physicalAddress?.Address?.PhoneNumber is null) || !(physicalAddress?.Address?.PhoneNumberExtn is null) || !(physicalAddress?.Address?.FaxNumber is null) || !(physicalAddress?.Address?.Email is null)))
            {
                Context.ValidationMessages.AddError("Address fields must be blank if no physical address in Washington is selected.");
                return;
            }
            if (employerRegistration.EmployerDto.HasPhysicalLocation == true && Machine["SameAsPhyAddr"]?.ToString().Equals(LookupTable_AddressType.Mailing) == true && (Machine["EntityAddress.Address.StateCode"] is null || !Machine["EntityAddress.Address.StateCode"]?.Equals(LookupTable_StatePrvnc.Washington)==true))
            {
                Context.ValidationMessages.AddError("Cannot select mailing address with State other than Washington, if physical address in Washington is selected as 'Yes'.");
                return;
            }
            else if (Machine["SameAsPhyAddr"]?.ToString().Equals(LookupTable_AddressType.Mailing) == true)
            {
                Machine["PhysicalAddress"] = Machine["EntityAddress"];
                Machine["PhysicalAddress.Address.BusinessWebAddress"] = null;
                Machine["PhysicalAddress.AddressTypeCode"] = LookupTable_AddressType.Physical;
            }
            else
            {
                ValidateAddress((AddressDto)Machine["PhysicalAddress.Address"], Machine["PhyAddrReEmail"]?.ToString(),false);
            }
        }

        /// <summary>
        /// Validates 'Business Address' wizard step
        /// </summary>
        public void EnterBusiRcdsAddrNext()
        {
            if (Machine["SameAsBusiAddr"]?.ToString().Equals(LookupTable_AddressType.Mailing) == true)
            {
                Machine["BusinessAddress"] = Machine["EntityAddress"];
                Machine["BusinessAddress.Address.BusinessWebAddress"] = null;
                Machine["BusinessAddress.AddressTypeCode"] = LookupTable_AddressType.Business;
            }
            else if (Machine["SameAsBusiAddr"]?.ToString().Equals(LookupTable_AddressType.Physical) == true)
            {
                Machine["BusinessAddress"] = Machine["PhysicalAddress"];
                Machine["BusinessAddress.AddressTypeCode"] = LookupTable_AddressType.Business;
            }
            else
            {
                ValidateAddress((AddressDto)Machine["BusinessAddress.Address"], Machine["BusiAddrReTypeEmail"]?.ToString(),false);
            }

            if (Context.ValidationMessages.Count(ValidationMessageSeverity.Error) == 0)
            {
                Machine["CurrentSection"] = LookupUtil.GetLookupCode(LookupTable.WizEmployerRegistration, LookupTable_WizEmployerRegistration.Summary);
            }
        }

        /// <summary>
        /// Fills Employer Registration object with required details, to be passed to BusinessLogic for submition  
        /// </summary>
        public void RegistrationSummarySubmit()
        {
            var employerRegistration = (EmployerRegistrationViewModel)Machine["EmployerRegistration"];
            employerRegistration.ListAddressLinkDto = new List<AddressLinkDto>();
            employerRegistration.ListAddressLinkDto.Add((AddressLinkDto)Machine["EntityAddress"]);
            if (!string.IsNullOrEmpty(((AddressLinkDto)Machine["PhysicalAddress"]).Address.AddressLine1))
            {
                employerRegistration.ListAddressLinkDto.Add((AddressLinkDto)Machine["PhysicalAddress"]);
            }
            if (!string.IsNullOrEmpty(((AddressLinkDto)Machine["PhysicalAddress"]).Address.AddressLine1))
            {
                employerRegistration.ListAddressLinkDto.Add((AddressLinkDto)Machine["BusinessAddress"]);
            }
        }
        #endregion

        #region Misc Methods
        /// <summary>
        /// Validate Address fields
        /// </summary>
        /// <param name="address"></param>
        /// <param name="reEmail"></param>
        private void ValidateAddress(AddressDto address, String reEmail, bool checkPhone=true)
        {
            ValidateEmail(address.Email, reEmail);

            if (!address.CountryCode.Equals(LookupTable_Country.UnitedStates) && !address.CountryCode.Equals(LookupTable_Country.Canada) && !(address.StateCode is null))
            {
                Context.ValidationMessages.AddError("State should be blank if country is not United States or Canada.");
            }

            if ((address.CountryCode.Equals(LookupTable_Country.UnitedStates) || address.CountryCode.Equals(LookupTable_Country.Canada)) && address.StateCode is null)
            {
                Context.ValidationMessages.AddError("State is required if country is United States or Canada.");
            }
            if (!string.IsNullOrEmpty(address.AddressLine1))
            {
                if ((address.CountryCode.Equals(LookupTable_Country.UnitedStates) || address.CountryCode.Equals(LookupTable_Country.Canada)) && address.PhoneNumber is null)
                {
                    Context.ValidationMessages.AddError("Phone Number is required if country is United States or Canada.");
                }
            }
        }

        /// <summary>
        /// Validate Email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="reEmail"></param>
        private static void ValidateEmail(string email, string reEmail)
        {
            if ((!(reEmail is null) && email is null) || (!(email is null) && !email.Equals(reEmail, StringComparison.CurrentCultureIgnoreCase)))
            {
                Context.ValidationMessages.AddError("'E-Mail' and 'Re-Enter E-Mail' must be same.");
            }
        }
        #endregion
    }
}