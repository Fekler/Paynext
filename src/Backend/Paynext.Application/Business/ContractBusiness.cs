using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Interfaces;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Application.Business
{
    public class ContractBusiness(IContractRepository repository) : IContractBusiness
    {
        private readonly IContractRepository _repository = repository;


        public Task<Response<Guid>> Add(CreateContractDto dto)
        {
            if (dto == null)
            {
                return Task.FromResult(new Response<Guid>
                {
                    Success = false,
                    Message = "Contract data cannot be null."
                });
            }
            var contract = new Contract
            {
                ContractNumber = dto.ContractNumber,
                Description = dto.Description,
                InitialAmount = dto.Amount,
                StartDate = dto.StartDate,
                UserUuid = dto.UserUuid,
                IsActive = dto.IsActive,
            };
            var result = _repository.Add(contract);
            return Task.FromResult(new Response<Guid>
            {
                ApiReponse.Sucess = true,
                Success = true,
                Data = contract.Guid
            });
        }

        public Task<Response<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ContractDto>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<ContractDto>>> GetByUser(Guid userUuid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ContractDto>> GetDto(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Contract>> GetEntity(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> Update(UpdateContractDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
