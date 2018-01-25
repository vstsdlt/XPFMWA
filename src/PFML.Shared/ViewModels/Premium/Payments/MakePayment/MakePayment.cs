using PFML.Shared.Model.DbDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFML.Shared.ViewModels.Premium.Payments.MakePayment
{
    /// <summary>
    /// This class model beholds the values related to Employer Payment Details
    /// </summary>
    [Serializable]
    public class MakePaymentViewModel
    {
        public MakePaymentViewModel()
        {
            EmployerDto = new EmployerDto();

        }
        public EmployerDto EmployerDto { get; set; }
        public decimal PrePaymentAmount { get; set; }
        public decimal AmountDue { get; set; }

        public List<EmployerAccountTransactionDto> CollectionEmployerAccountTransactionDto { get; set; }
        public List<PaymentMainDto> CollectionPaymentMainDto { get; set; }
        public List<PaymentProfileDto> CollectionPaymentProfileDto { get; set; }
    }
}
