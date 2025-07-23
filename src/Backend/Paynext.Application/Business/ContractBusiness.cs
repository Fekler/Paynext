using Mapster;
using Microsoft.Extensions.Logging;
using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Interfaces;
using Paynext.Domain.Entities;
using Paynext.Domain.Interfaces.Repositories;
using SharedKernel;
using System.Net;

namespace Paynext.Application.Business
{
    public class ContractBusiness(IContractRepository repository, ILogger<ContractBusiness> logger) : IContractBusiness
    {
        private readonly IContractRepository _repository = repository;
        private readonly ILogger<ContractBusiness> _logger = logger;


        public async Task<Response<Guid>> Add(CreateContractDto dto)
        {
            try
            {
                //    var contract = dto.Adapt<Contract>();

                //    contract.Validate();

                //    var result = await _repository.Add(contract);
                //    _logger.LogInformation($"Contract created with ID: {result}");
                //    return new Response<Guid>().Sucess(data: contract.UUID, message: "Contract created successfully", statusCode: HttpStatusCode.Created);
                var contract = dto.Adapt<Contract>();
                contract.Validate();
                List<Installment> installments = [];
                for (int i = 0; i < dto.InstallmentCount; i++)
                {
                    var installment = new Installment
                    {
                        ContractUuid = contract.UUID,
                        DueDate = contract.StartDate.AddMonths(i + 1),
                        Value = contract.InitialAmount / dto.InstallmentCount,
                        Status = Domain.Entities._bases.Enums.InstallmentStatus.Open,
                        UUID = Guid.NewGuid(),
                        CreateAt = DateTime.UtcNow,

                    };
                    installments.Add(installment);
                }
                contract.Installments = installments;
                await _repository.Add(contract);
                return new Response<Guid>().Sucess(data: contract.UUID, message: "Contract created successfully", statusCode: HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contract");
                return new Response<Guid>().Failure(default, message: "An error occurred while creating the contract", statusCode: HttpStatusCode.InternalServerError);
            }

        }

        public async Task GenerateContract(CreateContractDto dto)
        {
            try
            {
                var contract = dto.Adapt<Contract>();
                contract.Validate();
                List<Installment> installments = [];
                for (int i = 0; i < dto.InstallmentCount; i++)
                {
                    var installment = new Installment
                    {
                        ContractUuid = contract.UUID,
                        DueDate = contract.StartDate.AddMonths(i + 1),
                        Value = contract.InitialAmount / dto.InstallmentCount,
                        Status = Domain.Entities._bases.Enums.InstallmentStatus.Open,
                        UUID = Guid.NewGuid(),
                        CreateAt = DateTime.UtcNow,

                    };
                    installments.Add(installment);
                }
                contract.Installments = installments;
                await _repository.Add(contract);

                _logger.LogInformation($"Contract created with UUID: {contract.UUID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating contract");
                throw new Exception("An error occurred while generating the contract", ex);
            }
        }
        public Task<Response<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> Delete(Guid guid)
        {
            try
            {
                var contractToDelete = await _repository.Get(guid);
                var deleted = await _repository.Delete(guid);

                return new Response<bool>()
                    .Sucess(data: deleted, message: "Contract deleted successfully", statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar deletar contract com UUID: {guid}.");
                return new Response<bool>().Failure(false, message: "Erro ao deletar contract.", statusCode: HttpStatusCode.InternalServerError);
            }

        }

        public async Task<Response<ContractDto>> Get(int id)
        {
            try
            {
                var contract = await _repository.Get(id);

                return new Response<ContractDto>()
                    .Sucess(data: contract.Adapt<ContractDto>(), message: "Contract retrieved successfully", statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving contract with ID {id}.");
                return new Response<ContractDto>()
                    .Failure(default, message: $"Error retrieving contract with ID {id}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<ContractDto>>> GetByUser(Guid userUuid)
        {
            try
            {
                var contracts = await _repository.GetByUserUuid(userUuid);
                var contractsDto = contracts.Adapt<List<ContractDto>>();
                return new Response<List<ContractDto>>()
                    .Sucess(data: contractsDto, message: "Contracts retrieved successfully", statusCode: HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving contracts for user with UUID {userUuid}.");
                return new Response<List<ContractDto>>()
                    .Failure(default, message: $"Error retrieving contracts for user with UUID {userUuid}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }
        public async Task<Response<ContractDto>> GetFullInformation(Guid guid)
        {
            try
            {
                var contract = await _repository.GetFullInformationByUuid(guid);
                var contractDto = contract.Adapt<ContractDto>();
                return new Response<ContractDto>()
                    .Sucess(data: contractDto, message: "Contracts retrieved successfully", statusCode: HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving contracts  with UUID {guid}.");
                return new Response<ContractDto>()
                    .Failure(default, message: $"Error retrieving contractswith UUID {guid}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<ContractDto>>> GetAllFullInformation()
        {
            try
            {
                //if (uuid.HasValue)
                //{
                //    var contract = await _repository.GetAllFullInformationByUuid(uuid.Value);
                //    var contractDto = contract.Adapt<ContractDto>();
                //    List<ContractDto> data = [contractDto];
                //    return new Response<List<ContractDto>>()
                //        .Sucess(data: data, message: "Contracts retrieved successfully", statusCode: HttpStatusCode.OK);
                //}
                var contracts = await _repository.GetAllFullInformation();
                var contracstDto = contracts.Adapt<List<ContractDto>>();
                return new Response<List<ContractDto>>()
                    .Sucess(data: contracstDto, message: "Contracts retrieved successfully", statusCode: HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving contracts");
                return new Response<List<ContractDto>>()
                    .Failure(default, message: $"Error retrieving {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<ContractDto>> GetDto(Guid guid)
        {
            try
            {
                var contract = await _repository.Get(guid);
                if (contract == null)
                {
                    return new Response<ContractDto>()
                        .Failure(default, message: $"Contract with UUID {guid} not found.", statusCode: HttpStatusCode.NotFound);
                }
                return new Response<ContractDto>()
                    .Sucess(data: contract.Adapt<ContractDto>(), message: "Contract retrieved successfully", statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving contract with UUID {guid}.");
                return new Response<ContractDto>()
                    .Failure(default, message: $"Error retrieving contract with UUID {guid}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<Contract>> GetEntity(Guid guid)
        {
            try
            {
                var contract = await _repository.Get(guid);
                if (contract == null)
                {
                    return new Response<Contract>()
                        .Failure(default, message: $"Contract with UUID {guid} not found.", statusCode: HttpStatusCode.NotFound);
                }
                return new Response<Contract>()
                    .Sucess(data: contract, message: "Contract retrieved successfully", statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return new Response<Contract>()
                    .Failure(default, message: $"Error retrieving contract with UUID {guid}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Update(UpdateContractDto dto)
        {
            try
            {
                var contractEntity = await _repository.Get(dto.Uuid);
                if (contractEntity == null)
                {
                    return new Response<bool>()
                        .Failure(false, message: $"Contract with UUID {dto.Uuid} not found.", statusCode: HttpStatusCode.NotFound);
                }
                var contract = dto.Adapt(contractEntity);
                contract.Validate();
                var updated = await _repository.Update(contract);
                if (updated)
                {
                    _logger.LogInformation($"Contract with UUID {dto.Uuid} updated successfully.");
                    return new Response<bool>()
                        .Sucess(data: true, message: "Contract updated successfully", statusCode: HttpStatusCode.OK);
                }
                else
                {
                    _logger.LogWarning($"Failed to update contract with UUID {dto.Uuid}.");
                    return new Response<bool>()
                        .Failure(false, message: "Failed to update contract", statusCode: HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating contract with UUID {dto.Uuid}.");
                return new Response<bool>()
                    .Failure(false, message: $"Error updating contract with UUID {dto.Uuid}: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }
        }
    }
}
