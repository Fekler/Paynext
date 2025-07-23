using Paynext.Application.Dtos.Entities.Installment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Application.Interfaces
{
    public interface IPayManagement
    {
        Task AntecipationInstallmentRequest(Guid installmentUuid);
        Task ListAllAntecipationRequests();
        Task ActioneAntecipationRequests(List<ActioneInstallment> installments, Guid userUuid);
    }
}
