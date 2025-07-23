using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Application.UseCases
{
    public class PayManagement(IContractBusiness contractBusiness, IInstallmentBusiness installmentBusiness, IUserBusiness userBusiness) : IPayManagement
    {
        private readonly IContractBusiness _contractBusiness = contractBusiness;
        private readonly IInstallmentBusiness _installmentBusiness = installmentBusiness;
        private readonly IUserBusiness _userBusiness = userBusiness;

        public async Task AntecipationInstallmentRequest(Guid installmentUuid)
        {
            throw new NotImplementedException("AntecipationInstallmentRequest method is not implemented yet.");
        }

        public async Task ListAllAntecipationRequests()
        {
            throw new NotImplementedException("ListAllAntecipationRequests method is not implemented yet.");
        }

        public async Task ActioneAntecipationRequests(List<ActioneInstallment> installments, Guid userUuid)
        {
            throw new NotImplementedException("ActioneAntecipationRequests method is not implemented yet.");
        }
    }
}
