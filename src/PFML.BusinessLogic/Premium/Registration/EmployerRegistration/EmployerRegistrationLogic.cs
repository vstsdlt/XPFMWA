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
        public static EmployerRegistrationViewModel SubmitRegistration(EmployerRegistrationViewModel employerRegistrationViewModel)
        {
            using (DbContext context = new DbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        employerRegistrationViewModel.EmployerDto = SetPendingFieldsEmployerDto(employerRegistrationViewModel.EmployerDto, employerRegistrationViewModel.EmployerUnitDto);
                        employerRegistrationViewModel.EmployerContactDto = SetPendingFieldsEmployerContactDto(employerRegistrationViewModel.EmployerContactDto);
                        employerRegistrationViewModel.EmployerUnitDto = SetPendingFieldsEmployerUnitDto(employerRegistrationViewModel.EmployerUnitDto,employerRegistrationViewModel.ListAddressLinkDto);
                        employerRegistrationViewModel.ListAddressLinkDto= SetPendingFieldsListAddressLinkDto(employerRegistrationViewModel.ListAddressLinkDto);

                        var employer=Employer.FromDto(context, employerRegistrationViewModel.EmployerDto);
                        var employerUnit = EmployerUnit.FromDto(context, employerRegistrationViewModel.EmployerUnitDto);
                        employer.EmployerContacts.Add(EmployerContact.FromDto(context, employerRegistrationViewModel.EmployerContactDto));
                        foreach (var addressLinkDto in employerRegistrationViewModel.ListAddressLinkDto)
                        {
                            employerUnit.AddressLinks.Add(AddressLink.FromDto(context, addressLinkDto));
                        }
                        employer.EmployerUnits.Add(employerUnit);

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        employerRegistrationViewModel.EmployerDto = employer.ToDto();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
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

        #region Private Methods

        /// <summary>
        /// Set Pending Fields for ListAddressLinkDto
        /// </summary>
        /// <param name="listAddressLinkDto"></param>
        /// <returns></returns>
        private static List<AddressLinkDto> SetPendingFieldsListAddressLinkDto(List<AddressLinkDto> listAddressLinkDto)
        {
            foreach (var addressLinkDto in listAddressLinkDto)
            {
                addressLinkDto.Address.AddressLine1 = Shared.Utility.StringUtil.ToUpper(addressLinkDto.Address.AddressLine1);
                addressLinkDto.Address.AddressLine2 = Shared.Utility.StringUtil.ToUpper(addressLinkDto.Address.AddressLine2);
                addressLinkDto.Address.City = Shared.Utility.StringUtil.ToUpper(addressLinkDto.Address.City);
                addressLinkDto.Address.AddressVerificationCode = "VERF";
                addressLinkDto.StatusCode = LookupTable_EmployerStatus.Active;
            }

            return listAddressLinkDto;
        }

        /// <summary>
        /// Set Pending Fields for EmployerUnitDto
        /// </summary>
        /// <param name="employerUnitDto"></param>
        /// <param name="listAddressLinkDto"></param>
        /// <returns></returns>
        private static EmployerUnitDto SetPendingFieldsEmployerUnitDto(EmployerUnitDto employerUnitDto, List<AddressLinkDto> listAddressLinkDto)
        {
            string countyCode = "0";
            AddressLinkDto physicalAddress = listAddressLinkDto.FirstOrDefault(addressLink => addressLink.AddressTypeCode == LookupTable_AddressType.Physical);
            employerUnitDto.EmployerUnitSeqNo = 1;
            employerUnitDto.DoingBusinessAsName = Shared.Utility.StringUtil.ToUpper(employerUnitDto.DoingBusinessAsName);
            employerUnitDto.StatusCode = LookupTable_EmployerStatus.Active;
            employerUnitDto.StatusDate = DateTimeUtil.Now;
            //TODO: when CountyCode is NULL, county code is determined based on the zip code
            //County zip mapping for WA needs to be populated in the lookup data
            employerUnitDto.CountyCode = physicalAddress?.Address?.CountyCode ?? countyCode;

            return employerUnitDto;
        }

        /// <summary>
        /// Set Pending Fields for EmployerContactDto
        /// </summary>
        /// <param name="employerContactDto"></param>
        /// <returns></returns>
        private static EmployerContactDto SetPendingFieldsEmployerContactDto(EmployerContactDto employerContactDto)
        {
            //TODO: As per the MVP, the registration process takes only the administrator's contact
            employerContactDto.ContactTypeCode = LookupTable_ContactType.Administrator;
            employerContactDto.ContactSeqNo = 0;
            employerContactDto.StatusCode = LookupTable_EmployerStatus.Active;

            return employerContactDto;
        }

        /// <summary>
        /// Set Pending Fields for EmployerDto
        /// </summary>
        /// <param name="employerDto"></param>
        /// <param name="employerUnitDto"></param>
        /// <returns></returns>
        private static EmployerDto SetPendingFieldsEmployerDto(EmployerDto employerDto, EmployerUnitDto employerUnitDto)
        {
            var employerLiabilityDto = employerDto.EmployerLiability;
            int liabilityIncurredYear = employerLiabilityDto.LiabilityAmountMetYear.HasValue ? employerLiabilityDto.LiabilityAmountMetYear.Value : 0;
            int liabilityIncurredQtr = employerLiabilityDto.LiabilityAmountMetYear.HasValue ? Convert.ToInt32(DateUtil.GetQuarterNumber(employerLiabilityDto.LiabilityAmountMetQuarter)) : 0;
            DateTime? liabilityDate = GetLiabilityDate(liabilityIncurredYear, liabilityIncurredQtr, employerDto.EntityTypeCode, employerUnitDto.FirstWageDate);
            string paymentMehtodCode = GetPaymentMethod();

            employerDto.RegistrationDate = DateTimeUtil.Now;
            employerDto.LiabilityDate = liabilityDate;
            employerDto.LiabilityIncurredDate = liabilityDate;
            employerDto.ReportMethodCode = paymentMehtodCode;
            employerDto.SubjectivityCode = GetSubjectivityCode(paymentMehtodCode);
            employerDto.StatusCode = LookupTable_EmployerStatus.Active;
            employerDto.StatusDate = DateTimeUtil.Now;

            return employerDto;
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
                liableQtr = Convert.ToSByte(DateUtil.GetQuarterNumber(firstWageDate.Value.Month));
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
