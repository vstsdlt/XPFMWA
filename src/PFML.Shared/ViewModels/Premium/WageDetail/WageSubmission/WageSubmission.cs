using PFML.Shared.Model.DbDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.ViewModels.Premium.WageDetail.WageSubmission
{
    [Serializable]
    public class WageSubmissionViewModel
    {
        public WageSubmissionViewModel()
        {
            ListWageUnitDetailDto = new List<WageUnitCustomDto>();
            ListWageEmployerUnitSummary = new List<WageDetailSummaryViewModel>();
            Employer = new EmployerDto();
            EmployerAccountTransactionDto = new EmployerAccountTransactionDto();
        }

        /// <summary>
        /// ReportingYear
        /// </summary>
        public short ReportingYear { get; set; }
        /// <summary>
        /// ReportingQuarter
        /// </summary>
        public string ReportingQuarter { get; set; }
        /// <summary>
        /// AdjReasonCode
        /// </summary>
        public string AdjReasonCode { get; set; }
        /// </summary>
        public string AdjReasonDisplay { get; set; }
        /// <summary>
        /// FilingMethod
        /// </summary>
        public string FilingMethod { get; set; }
        /// <summary>
        /// EmployerId
        /// </summary>
        public int EmployerId { get; set; }
        /// <summary>
        /// EmployerUnitId
        /// </summary>
        public int EmployerUnitId { get; set; }

        /// <summary>
        /// GrossWages
        /// </summary>
        public decimal GrossWages { get; set; }

        /// <summary>
        /// Number of Records
        /// </summary>
        public int NumberofRecords { get; set; }

        public EmployerDto Employer { get; set; }

        public EmployerAccountTransactionDto EmployerAccountTransactionDto { get; set; }

        [Serializable]
        public class WageUnitCustomDto: WageUnitDetailDto
        {
            public int SrNo { get; set; }
           
        }

        /// <summary>
        /// List of Employee Wages
        /// </summary>
        public List<WageUnitCustomDto> ListWageUnitDetailDto { get; set; }
        /// <summary>
        /// Employer Unit Wage Summary
        /// </summary>
        public List<WageDetailSummaryViewModel> ListWageEmployerUnitSummary { get; set; }

        [Serializable]
        public class WageDetailSummaryViewModel
        {
            public int EmployerUnitNo { get; set; }
            public string EntityName { get; set; }
            public int NumberofRecords { get; set; }
            public decimal GrossWage { get; set; }
            public decimal TaxableWage { get; set; }
            public decimal AmountDue { get; set; }
            public int QtrMonth1RecordsCount { get; set; }
            public int QtrMonth2RecordsCount { get; set; }
            public int QtrMonth3RecordsCount { get; set; }
        }
    }
}
