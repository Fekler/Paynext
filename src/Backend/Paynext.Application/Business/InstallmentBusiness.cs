using Mapster;
using Microsoft.Extensions.Logging;
using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Interfaces;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Domain.Interfaces.Repositories;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Application.Business
{
    public class InstallmentBusiness(IInstallmentRepository repository, ILogger<InstallmentBusiness> logger) : IInstallmentBusiness
    {
        private readonly IInstallmentRepository _repository = repository;
        private readonly ILogger<InstallmentBusiness> _logger = logger;

        public async Task<Response<Guid>> Add(CreateInstallmentDto dto)
        {
            try
            {
                var installment = dto.Adapt<Installment>();
                installment.Validate();
                var result = await _repository.Add(installment);
                _logger.LogInformation($"Installment created with ID: {result}");
                return new Response<Guid>().Sucess(data: installment.UUID, message: "Installment created successfully", statusCode: System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating installment");
                return new Response<Guid>().Failure(default, message: "An error occurred while creating the installment", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                var installmentToDelete = await _repository.Get(id);
                if (installmentToDelete == null)
                {
                    return new Response<bool>().Failure(false, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var deleted = await _repository.Delete(id);
                return new Response<bool>()
                    .Sucess(data: deleted, message: "Installment deleted successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error trying to delete installment with ID: {id}.");
                return new Response<bool>().Failure(false, message: "Error deleting installment.", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Delete(Guid guid)
        {
            try
            {
                var installmentToDelete = await _repository.Get(guid);
                if (installmentToDelete == null)
                {
                    return new Response<bool>().Failure(false, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var deleted = await _repository.Delete(guid);
                return new Response<bool>()
                    .Sucess(data: deleted, message: "Installment deleted successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error trying to delete installment with UUID: {guid}.");
                return new Response<bool>().Failure(false, message: "Error deleting installment.", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<InstallmentDto>> Get(int id)
        {
            try
            {
                var installment = await _repository.Get(id);
                if (installment == null)
                {
                    return new Response<InstallmentDto>().Failure(null, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var installmentDto = installment.Adapt<InstallmentDto>();
                return new Response<InstallmentDto>().Sucess(data: installmentDto, message: "Installment retrieved successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving installment with ID {id}.");
                return new Response<InstallmentDto>().Failure(null, message: $"Error retrieving installment: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<InstallmentDto>>> GetByContract(Guid contractUuid)
        {
            try
            {
                var installments = await _repository.GetAllByContractId(contractUuid);
                if (installments == null || !installments.Any())
                {
                    return new Response<List<InstallmentDto>>().Failure(null, message: "No installments found for the specified contract", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var installmentDtos = installments.Adapt<List<InstallmentDto>>();
                return new Response<List<InstallmentDto>>().Sucess(data: installmentDtos, message: "Installments retrieved successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving installments for contract UUID {contractUuid}.");
                return new Response<List<InstallmentDto>>().Failure(null, message: $"Error retrieving installments: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<InstallmentDto>>> GetAllByContractIdAndStatus(Guid contractUuid, Enums.InstallmentStatus status)
        {
            try
            {
                var installments = await _repository.GetAllByContractIdAndStatus(contractUuid, status);
                if (installments == null || !installments.Any())
                {
                    return new Response<List<InstallmentDto>>().Failure(null, message: "No installments found for the specified contract and status", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var installmentDtos = installments.Adapt<List<InstallmentDto>>();
                return new Response<List<InstallmentDto>>().Sucess(data: installmentDtos, message: "Installments retrieved successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving installments for contract UUID {contractUuid} with status {status}.");
                return new Response<List<InstallmentDto>>().Failure(null, message: $"Error retrieving installments: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<InstallmentDto>> GetDto(Guid guid)
        {
            try
            {
                var installment = await _repository.Get(guid);
                if (installment == null)
                {
                    return new Response<InstallmentDto>().Failure(null, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                var installmentDto = installment.Adapt<InstallmentDto>();
                return new Response<InstallmentDto>().Sucess(data: installmentDto, message: "Installment retrieved successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving installment with UUID {guid}.");
                return new Response<InstallmentDto>().Failure(null, message: $"Error retrieving installment: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }


        public async Task<Response<Installment>> GetEntity(Guid guid)
        {
            try
            {
                var installment = await _repository.Get(guid);
                if (installment == null)
                {
                    return new Response<Installment>().Failure(null, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                return new Response<Installment>().Sucess(data: installment, message: "Installment retrieved successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving installment with UUID {guid}.");
                return new Response<Installment>().Failure(null, message: $"Error retrieving installment: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Update(UpdateInstallmentDto dto)
        {
            try
            {
                var installment = await _repository.Get(dto.Uuid);
                if (installment == null)
                {
                    return new Response<bool>().Failure(false, message: "Installment not found", statusCode: System.Net.HttpStatusCode.NotFound);
                }
                installment = dto.Adapt(installment);
                installment.Validate();
                var updated = await _repository.Update(installment);
                return new Response<bool>().Sucess(data: updated, message: "Installment updated successfully", statusCode: System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating installment with UUID {dto.Uuid}.");
                return new Response<bool>().Failure(false, message: $"Error updating installment: {ex.Message}", statusCode: System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
