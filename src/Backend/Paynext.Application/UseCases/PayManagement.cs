﻿using Mapster;
using Paynext.Application.Dtos.Entities;
using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Interfaces;
using SharedKernel;
using System.Net;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.UseCases
{
    public class PayManagement(IContractBusiness contractBusiness, IInstallmentBusiness installmentBusiness, IUserBusiness userBusiness) : IPayManagement
    {
        private readonly IContractBusiness _contractBusiness = contractBusiness;
        private readonly IInstallmentBusiness _installmentBusiness = installmentBusiness;
        private readonly IUserBusiness _userBusiness = userBusiness;

        public async Task<Response<bool>> AntecipationInstallmentRequest(Guid installmentUuid, Guid userUuid)
        {
            var installment = await _installmentBusiness.GetEntity(installmentUuid);
            if (!installment.ApiReponse.OK || installment.ApiReponse is null || installment.ApiReponse.Data is null)
            {
                return new Response<bool>().Failure(data: default, message: "Installment not found.", statusCode: HttpStatusCode.NotFound);
            }
            if (installment.ApiReponse.Data.IsAntecipated)
            {
                //throw new InvalidOperationException("Installment is already antecipated.");
                return new Response<bool>().Failure(data: default, message: "Installment is already antecipated.", statusCode: HttpStatusCode.Forbidden);

            }
            var contract = await _contractBusiness.GetFullInformation(installment.ApiReponse.Data.ContractUuid, userUuid, false);
            if (!contract.ApiReponse.OK || contract.ApiReponse is null)
            {
                return new Response<bool>().Failure(data: default, message: "Contract not found.", statusCode: HttpStatusCode.Forbidden);
            }
            if (contract.ApiReponse.Data?.UserUuid != userUuid)
            {
                return new Response<bool>().Failure(data: default, message: "Contract Without user.", statusCode: HttpStatusCode.InternalServerError);
            }
            if (installment.ApiReponse.Data.Status != Domain.Entities._bases.Enums.InstallmentStatus.Open)
            {
                return new Response<bool>().Failure(data: default, message: "Installment is paid.", statusCode: HttpStatusCode.BadRequest);
            }
            if (installment.ApiReponse.Data.DueDate < DateTime.UtcNow)
            {
                return new Response<bool>().Failure(data: default, message: "Installment is overdue and cannot be antecipated.", statusCode: HttpStatusCode.Forbidden);
            }
            if (installment.ApiReponse.Data.DueDate < DateTime.UtcNow.AddDays(30))
            {
                return new Response<bool>().Failure(data: default, message: "Installment is not too far to be antecipated.", statusCode: HttpStatusCode.Forbidden);
            }
            var contracts = await _contractBusiness.GetByUser(userUuid);
            var hasOtherAntecipationRequests = contracts?.ApiReponse?.Data?.Any(c => c.Installments.Any(i => i.AntecipationStatus == AntecipationStatus.Pending && i.Status == InstallmentStatus.Open));
            if (hasOtherAntecipationRequests.HasValue && hasOtherAntecipationRequests.Value)
            {
                return new Response<bool>().Failure(data: default, message: "User already has an antecipation request for another installment.", statusCode: HttpStatusCode.Forbidden);
            }
            var installmentToUpdate = installment.ApiReponse.Data;
            UpdateInstallmentDto updateInstallmentDto = new()
            {
                UUID = installmentToUpdate.UUID,
                Status = installmentToUpdate.Status,
                ContractUuid = installmentToUpdate.ContractUuid,
                DueDate = installmentToUpdate.DueDate,
                AntecipationStatus = AntecipationStatus.Pending,
            };
            var updateResponse = await _installmentBusiness.Update(updateInstallmentDto);
            if (!updateResponse.ApiReponse.OK || updateResponse.ApiReponse is null)
            {
                return new Response<bool>().Failure(data: default, message: "Failed to update installment.", statusCode: HttpStatusCode.InternalServerError);
            }
            return new Response<bool>().Success(data: true, message: "Antecipation request created successfully.", statusCode: HttpStatusCode.OK);
        }


        public async Task<Response<bool>> ActioneAntecipationRequests(List<ActioneInstallment> installments, Guid userUuid)
        {
            try
            {
                bool IsOK = false;
                foreach (var installment in installments)
                {
                    var installmentEntity = await _installmentBusiness.GetEntity(installment.InstallmentUuid);
                    if (!installmentEntity.ApiReponse.OK || installmentEntity.ApiReponse is null || installmentEntity.ApiReponse.Data is null)
                    {
                        continue; // Skip if installment not found
                    }
                    var installmentData = installmentEntity.ApiReponse.Data;
                    if (installmentData.IsAntecipated || installmentData.ActionedByUser != null)
                    {
                        continue; // Skip if not antecipated or already actioned
                    }
                    installmentData.ActionedByUserUuiD = userUuid;
                    installmentData.AntecipationStatus = installment.IsAccepted ? AntecipationStatus.Approved : AntecipationStatus.Rejected;
                    installmentData.IsAntecipated = installment.IsAccepted;
                    UpdateInstallmentDto updateInstallmentDto = new()
                    {
                        UUID = installmentData.UUID,
                        Status = installmentData.Status,
                        ContractUuid = installmentData.ContractUuid,
                        DueDate = installmentData.DueDate,
                        AntecipationStatus = installmentData.AntecipationStatus,
                        IsAntecipated = installmentData.IsAntecipated,
                    };
                    var updated = await _installmentBusiness.Update(updateInstallmentDto);
                    IsOK = updated.ApiReponse?.OK ?? false;
                }
                return new Response<bool>()
                    .Success(data: IsOK, message: IsOK ? "Antecipation requests processed successfully." : "Failed to process some antecipation requests.", statusCode: IsOK ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {

                return new Response<bool>()
                    .Failure(data: false, message: $"An error occurred while processing antecipation requests: {ex.Message}", statusCode: HttpStatusCode.InternalServerError);
            }

        }
        public async Task<Response<List<ContractInformationDto>>> ListUserAntecipationRequests(Guid guid, int pageNumber, int pageSize)
        {
            var installments = await _installmentBusiness.GetUserAntecipateToActione(guid, pageNumber, pageSize);
            if (installments.ApiReponse is null || !installments.ApiReponse.OK)
            {
                return new Response<List<ContractInformationDto>>().Success(data: default, message: "No antecipation requests found.", statusCode: HttpStatusCode.OK);
            }
            List<ContractInformationDto> contracts = [];
            foreach (var installment in installments?.ApiReponse?.Data)
            {
                var installmentDto = installment.Adapt<InstallmentDto>();
                ContractInformationDto contract = new()
                {
                    ClientId = installment.Contract.User.UUID,
                    ContractNumber = installment.Contract.ContractNumber,
                    ClientName = installment.Contract.User.FullName,
                    ContractId = installment.Contract.UUID,
                    Installments = [installmentDto]
                };
                contracts.Add(contract);
            }

            return new Response<List<ContractInformationDto>>()
                .Success(data: contracts, message: "Antecipation requests retrieved successfully.", statusCode: HttpStatusCode.OK);

        }
        public async Task<Response<List<ContractInformationDto>>> ListAllAntecipationRequests(int pageNumber, int pageSize)
        {
            var installments = await _installmentBusiness.GetAllAntecipateToActione(pageNumber, pageSize);
            if (installments.ApiReponse is null || !installments.ApiReponse.OK)
            {
                return new Response<List<ContractInformationDto>>().Success(data: default, message: "No antecipation requests found.", statusCode: HttpStatusCode.OK);
            }
            List<ContractInformationDto> contracts = [];
            foreach (var installment in installments?.ApiReponse?.Data)
            {
                var installmentDto = installment.Adapt<InstallmentDto>();
                ContractInformationDto contract = new()
                {
                    ClientId = installment.Contract.User.UUID,
                    ContractNumber = installment.Contract.ContractNumber,
                    ClientName = installment.Contract.User.FullName,
                    ContractId = installment.Contract.UUID,
                    Installments = [installmentDto]
                };
                contracts.Add(contract);
            }

            return new Response<List<ContractInformationDto>>()
                .Success(data: contracts, message: "Antecipation requests retrieved successfully.", statusCode: HttpStatusCode.OK);

        }
        public async Task<Response<ContractInformationDto>> GetInstallment(Guid guid)
        {
            var installmentResponse = await _installmentBusiness.GetEntity(guid);

            var installment = installmentResponse.ApiReponse?.Data;
            var contractResponse = await _contractBusiness.GetEntity(installment.ContractUuid);
            var userResponse = await _userBusiness.GetEntity(contractResponse.ApiReponse.Data.UserUuid);
            installment.Contract ??= contractResponse.ApiReponse.Data;
            installment.Contract.User ??= userResponse?.ApiReponse?.Data;
            if (installmentResponse.ApiReponse is null || !installmentResponse.ApiReponse.OK)
            {
                return new Response<ContractInformationDto>().Success(data: default, message: "No antecipation requests found.", statusCode: HttpStatusCode.OK);
            }

            var installmentDto = installment.Adapt<InstallmentDto>();
            ContractInformationDto contract = new()
            {
                ClientId = installment.Contract.User.UUID,
                ContractNumber = installment.Contract.ContractNumber,
                ClientName = installment.Contract.User.FullName,
                ContractId = installment.Contract.UUID,
                Installments = [installmentDto]
            };


            return new Response<ContractInformationDto>()
                .Success(data: contract, message: "Antecipation requests retrieved successfully.", statusCode: HttpStatusCode.OK);

        }
    }
}
