using System;
using System.Collections.Generic;
using System.Linq;
using FACTS.Framework.DAL;
using FACTS.Framework.Service;
using PFML.DAL.Model.DbEntities;
using PFML.Shared.Model.DbDtos;
using DbContext = PFML.DAL.Model.DbContext;
using PFML.Shared.ViewModels.Premium.Registration;
using PFML.Shared.Utility;
using PFML.Shared.LookupTable;
using FACTS.Framework.Utility;

namespace PFML.BusinessLogic.Premium.Registration
{

    public static class EmployerRegistrationLogic
    {
        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateIntroduction(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            using (DbContext context = new DbContext())
            {
                if (context.Employers.Any(emp => emp.Fein == employerRegistrationViewModel.EmployerDto.Fein))
                {
                    Context.ValidationMessages.AddError("The FEIN entered already exists in the system for another employer account. Verify your records to ensure the information you entered is correct and re-enter the FEIN.");
                }
            }
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateLiabilityWeeks(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateEnterPhysicalAddr(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateEnterBusiRcdsAddr(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel SubmitRegistration(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            //using (DbContext createNewEmployerCtx = new DbContext())
            //{
            //    //Employer
            //    Employer employer = CreateNewEmployer(employerRegistrationViewModel.EmployerDto);

            //    //EmployerLiability
            //    employer.EmployerLiability = AddEmployerLiability(employerRegistrationViewModel.EmployerDto.EmployerLiability);

            //    //EmployerContact
            //    AddEmployerContacts(employer, employerRegistrationViewModel.EmployerContactDto);

            //    //EmployerUnit
            //    employer.EmployerUnits.Add(AddEmployerUnit(employerRegistrationViewModel.EmployerUnitDto));

            //    //EmployerAddress - Address
            //    AddEmployerAddresses(employer, employerRegistrationViewModel.ListAddressLinkDto);

            //    //EmployerPreference
            //    employer.EmployerPreference = AddEmployerPreference(employerRegistrationViewModel.EmployerDto.EmployerPreference);

            //    createNewEmployerCtx.SaveChanges();
            //}
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateEnterBusinessInfoCont(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateEnterBusinessInfo(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateEmpIdentification(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            return employerRegistrationViewModel;
        }

        [OperationMethod]
        public static EmployerRegistrationViewModel ValidateAdminInfo(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            using (DbContext context = new DbContext())
            {
                if (context.Employers.Any(emp => emp.UserName == employerRegistrationViewModel.EmployerDto.UserName))
                {
                    Context.ValidationMessages.AddError("The Username entered already exists in the system for another employer account. Please re-enter different Username.");
                }
            }
            return employerRegistrationViewModel;
        }

        #region Private Mehtods

        static Employer CreateNewEmployer(EmployerDto employerDto)
        {
            int liabilityIncurredYear = employerDto.EmployerLiability.LiabilityAmountMetYear.HasValue ? employerDto.EmployerLiability.LiabilityAmountMetYear.Value : 0;
            int liabilityIncurredQtr = employerDto.EmployerLiability.LiabilityAmountMetYear.HasValue ? Convert.ToInt32(employerDto.EmployerLiability.LiabilityAmountMetQuarter) : 0;
            DateTime? liabilityDate = GetLiabilityDate(liabilityIncurredYear, liabilityIncurredQtr, employerDto.EntityTypeCode, employerDto.EmployerUnits[0].FirstWageDate);
            string paymentMehtodCode = GetPaymentMethod();

            Employer employer = new Employer
            {
                Fein = employerDto.Fein,
                EntityName = employerDto.EntityName,
                EntityTypeCode = employerDto.EntityTypeCode,
                BusinessTypeCode = employerDto.BusinessTypeCode,
                NoOfEmployeesPaid = employerDto.NoOfEmployeesPaid,
                ServiceBeginDate = employerDto.ServiceBeginDate,
                IsIndividualContractor = employerDto.IsIndividualContractor,
                IsAcquired = employerDto.IsAcquired,
                IsPresentInMultipleLoc = employerDto.IsPresentInMultipleLoc,
                IsClientOfPEO = employerDto.IsClientOfPEO,
                IsApplyingForREIM = employerDto.IsApplyingForREIM,
                NoOfLocation = employerDto.NoOfLocation,
                HasPhysicalLocation = employerDto.HasPhysicalLocation,
                IsExemptUnderIRS501C3 = employerDto.IsExemptUnderIRS501C3,
                IsProfessionalEmployerOrg = employerDto.IsProfessionalEmployerOrg,
                RegistrationDate = DateTimeUtil.Now,
                LiabilityDate = liabilityDate,
                LiabilityIncurredDate = liabilityDate,
                ReportMethodCode = paymentMehtodCode,
                SubjectivityCode = GetSubjectivityCode(paymentMehtodCode),
                StatusCode = LookupTable_EmployerStatus.Active,
                StatusDate = DateTimeUtil.Now
            };

            return employer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjIncurYr"></param>
        /// <param name="subjIncurQtr"></param>
        /// <param name="entityTypeCd"></param>
        /// <param name="firstWageDate"></param>
        /// <param name="useSubCode"></param>
        /// <returns></returns>
        static DateTime? GetLiabilityDate(int subjIncurYr, int subjIncurQtr, string entityTypeCd, DateTime? firstWageDate, bool useSubCode = true)
        {
            DateTime? liabilityDate = null;
            DateTime? liabilityIncurredDate = null;
            DateTime? firstdateofquarter = null;

            short liableYear = 0;
            sbyte liableQtr = 0;

            if (firstWageDate.HasValue)
            {
                liableYear = Convert.ToInt16(firstWageDate.Value.Year);
                liableQtr = Convert.ToSByte(DateUtil.GetQuarter(firstWageDate.Value.Month));
            }

            firstdateofquarter = DateUtil.GetFirstDayOfQuarter(liableYear, liableQtr);

            if (entityTypeCd == LookupTable_LegalEntity.GovernmentStateAgency)
            {
                liabilityDate = firstdateofquarter;
                liabilityIncurredDate = firstdateofquarter;
            }
            else if (firstWageDate.HasValue)
            {
                liabilityDate = firstdateofquarter;
                liabilityIncurredDate = DateUtil.GetFirstDayOfQuarter(subjIncurYr, subjIncurQtr);

                if (liabilityDate.Value.Year != liabilityIncurredDate.Value.Year)
                {
                    liabilityDate = new DateTime(liabilityIncurredDate.Value.Year, 1, 1);
                }
                else
                {
                    int currentQuarter = DateUtil.GetQuarterNumber(DateTimeUtil.Now.Month);
                    int yearPast = DateTimeUtil.Now.Year - 4;
                    int firstWageDateQuarter = DateUtil.GetQuarterNumber(firstWageDate.Value.Month);
                    DateTime pastDate = new DateTime(yearPast, DateTimeUtil.Now.Month, DateTimeUtil.Now.Day);
                    if (firstWageDate.Value < new DateTime(yearPast, DateTimeUtil.Now.Month, DateTimeUtil.Now.Day))
                    {
                        liabilityIncurredDate = DateUtil.GetFirstDayOfQuarter(DateTimeUtil.Now.Year - 4, currentQuarter);
                        liabilityDate = DateUtil.GetFirstDayOfQuarter(DateTimeUtil.Now.Year - 4, currentQuarter);
                    }
                    else
                    {
                        liabilityDate = firstdateofquarter;
                        liabilityIncurredDate = DateUtil.GetFirstDayOfQuarter(subjIncurYr, subjIncurQtr);
                    }
                }
            }

            return liabilityIncurredDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employerLiabilityDto"></param>
        /// <returns></returns>
        static EmployerLiability AddEmployerLiability(EmployerLiabilityDto employerLiabilityDto)
        {
            EmployerLiability employerLiability = new EmployerLiability
            {
                HasPaid1KDomesticWages = employerLiabilityDto.HasPaid1KDomesticWages,
                HasPaid20KAgriculturalLaborWages = employerLiabilityDto.HasPaid20KAgriculturalLaborWages,
                HasPaid450RegularWages = employerLiabilityDto.HasPaid450RegularWages,
                HasEmployed1In20Weeks = employerLiabilityDto.HasEmployed1In20Weeks,
                HasEmployed10In20Weeks = employerLiabilityDto.HasEmployed10In20Weeks,
                GrossWagesPaid = employerLiabilityDto.GrossWagesPaid,
                LiabilityAmountMetQuarter = employerLiabilityDto.LiabilityAmountMetQuarter,
                LiabilityAmountMetYear = employerLiabilityDto.LiabilityAmountMetYear
            };

            return employerLiability;
        }

        /// <summary>
        /// Adds the employer's contact
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="employerContact"></param>
        static void AddEmployerContacts(Employer employer, EmployerContactDto employerContact)
        {
            int contactSeqNo = 0;

            employer.EmployerContacts.Add(new EmployerContact
            {
                ContactTypeCode = employerContact.ContactTypeCode,
                ContactSeqNo = ++contactSeqNo,
                LastName = employerContact.LastName,
                FirstName = employerContact.FirstName,
                MiddleInitial = employerContact.MiddleInitial,
                Title = employerContact.Title,
                PhoneNumber = employerContact.PhoneNumber,
                PhoneNumberExtn = employerContact.PhoneNumberExtn,
                SecondaryPhoneNumber = employerContact.SecondaryPhoneNumber,
                SecondaryPhoneNumberExtn = employerContact.SecondaryPhoneNumberExtn,
                Email = employerContact.Email,
                StatusCode = LookupTable_EmployerStatus.Active
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employerUnitDto"></param>
        /// <returns></returns>
        static EmployerUnit AddEmployerUnit(EmployerUnitDto employerUnitDto)
        {
            string countyCode = "0";
            AddressLinkDto physicalAddress = employerUnitDto.Employer.AddressLinks.FirstOrDefault(addressLink => addressLink.AddressTypeCode == LookupTable_AddressType.Physical);
            EmployerUnit employerUnit = new EmployerUnit
            {
                EmployerUnitSeqNo = 1,
                DoingBusinessAsName = Shared.Utility.StringUtil.ToUpper(employerUnitDto.DoingBusinessAsName),
                FirstWageDate = employerUnitDto.FirstWageDate,
                StatusCode = LookupTable_EmployerStatus.Active,
                StatusDate = DateTimeUtil.Now,
                //TODO: when CountyCode is NULL, county code is determined based on the zip code
                //County zip mapping for WA needs to be populated in the lookup data
                CountyCode = physicalAddress.Address.CountyCode 
            };

            return employerUnit;
        }

        static void AddEmployerAddresses(Employer employer, List<AddressLinkDto> employerAddresseLinks)
        {
            employerAddresseLinks.ForEach(employerAddressLink =>
            {
                AddressDto employerAddress = employerAddressLink.Address;
                employer.AddressLinks.Add(new AddressLink
                {
                    Address = new Address
                    {
                        AddressLine1 = Shared.Utility.StringUtil.ToUpper(employerAddress.AddressLine1),
                        AddressLine2 = Shared.Utility.StringUtil.ToUpper(employerAddress.AddressLine2),
                        City = Shared.Utility.StringUtil.ToUpper(employerAddress.City),
                        Zip = employerAddress.Zip,
                        CountryCode = employerAddress.CountryCode,
                        PhoneNumber = employerAddress.PhoneNumber,
                        PhoneNumberExtn = employerAddress.PhoneNumberExtn,
                        FaxNumber = employerAddress.FaxNumber,
                        Email = employerAddress.Email,
                    },
                    StatusCode = LookupTable_EmployerStatus.Active,
                    EmployerUnit = employer.EmployerUnits.First(),
                    AddressTypeCode = employerAddressLink.AddressTypeCode
                });
            });
        }

        static EmployerPreference AddEmployerPreference(EmployerPreferenceDto employerPreferenceDto)
        {
            EmployerPreference employerPreference = new EmployerPreference
            {
                CorrespondanceTypeCode = employerPreferenceDto.CorrespondanceTypeCode,
                Email = employerPreferenceDto.Email
            };

            return employerPreference;
        }


        /// <summary>
        /// Holds the business logic to determine the payment method based on
        /// IsExemptUnderIRS501C3, IsApplyingForREIM, EntityTypeCode, NAICS_CD
        /// TODO: This can be 'Reimbursing To Taxpaying' or 'Contributory'. It will be 'Contributory' always in MVP
        /// </summary>
        /// <returns></returns>
        static string GetPaymentMethod()
        {
            return LookupTable_EmployerReportMethod.Contributory;
        }

        /// <summary>
        /// Determines the SubjectivityCode based on the payment method
        /// TODO: It will be 'Contributory' always in MVP
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        static string GetSubjectivityCode(string paymentMethod)
        {
            return LookupTable_EmployerReportMethod.Contributory;
        }



        #endregion

    }

}
