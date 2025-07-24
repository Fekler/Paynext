using Mapster;
using Paynext.Application.Dtos.Entities;
using Paynext.Application.Dtos.Entities.Contract;
using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Dtos.Entities.User;
using Paynext.Domain.Entities;

namespace Paynext.Application.Profiles
{
    public class MapsterConfiguration
    {
        public static void Configure()
        {
            #region User
            TypeAdapterConfig<UserDto, User>.NewConfig().TwoWays();

            TypeAdapterConfig<CreateUserDto, User>.NewConfig()
                .Map(dest => dest.FullName, src => src.FullName.Trim())
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.CreateAt, src => DateTime.Now.ToUniversalTime())
                .Map(dest => dest.UUID, src => Guid.NewGuid());

            TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreateAt)
                .Ignore(dest => dest.Password)
                .Map(dest => dest.UpdateAt, src => DateTime.Now.ToUniversalTime());

            #endregion
            #region Contract
            TypeAdapterConfig<ContractDto, Contract>.NewConfig().TwoWays();

            TypeAdapterConfig<CreateContractDto, Contract>.NewConfig()
                .Map(dest => dest.ContractNumber, src => src.ContractNumber.Trim())
                .Map(dest => dest.Description, src => src.Description.Trim())
                .Map(dest => dest.InitialAmount, src => src.Amount)
                .Map(dest => dest.StartDate, src => src.StartDate.ToUniversalTime())
                .Map(dest => dest.EndDate, src => src.EndDate.Value.ToUniversalTime())
                .Map(dest => dest.UserUuid, src => src.UserUuid)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.CreateAt, src => DateTime.UtcNow)
                .Map(dest => dest.UUID, src => Guid.NewGuid());

            TypeAdapterConfig<UpdateContractDto, Contract>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreateAt)
                .Map(dest => dest.ContractNumber, src => src.ContractNumber.Trim())
                .Map(dest => dest.Description, src => src.Description.Trim())
                .Map(dest => dest.InitialAmount, src => src.Amount)
                .Map(dest => dest.StartDate, src => src.StartDate.ToUniversalTime())
                .Map(dest => dest.EndDate, src => src.EndDate.Value.ToUniversalTime())
                .Map(dest => dest.UserUuid, src => src.UserUuid)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.UpdateAt, src => DateTime.UtcNow)
                .Map(dest => dest.UUID, src => src.UUID);

            TypeAdapterConfig<Contract, ContractInformationDto>.NewConfig()
                .Map(dest => dest.ClientId, src => src.UserUuid)
                .Map(dest => dest.ClientName, src => src.User.FullName)
                .Map(dest => dest.ContractNumber, src => src.ContractNumber)
                .Map(dest => dest.ContractId, src => src.UUID)
                .Map(dest => dest.Installments, src => src.Installments
                    .Select(i => i.Adapt<InstallmentInformationDto>())
                    .ToList());
            #endregion

            #region Installment
            TypeAdapterConfig<InstallmentDto, Installment>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateInstallmentDto, Installment>.NewConfig()
                //.Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.DueDate, src => src.DueDate.ToUniversalTime())
                .Map(dest => dest.ContractUuid, src => src.ContractUuid)
                .Map(dest => dest.CreateAt, src => DateTime.Now.ToUniversalTime())
                .Map(dest => dest.UUID, src => Guid.NewGuid());
            TypeAdapterConfig<UpdateInstallmentDto, Installment>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreateAt)
                .Ignore(dest => dest.Value)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.DueDate, src => src.DueDate.ToUniversalTime())
                .Map(dest => dest.ContractUuid, src => src.ContractUuid)
                .Map(dest => dest.UpdateAt, src => DateTime.UtcNow)
                .Map(dest => dest.IsAntecipated, src => src.IsAntecipated)
                .Map(dest => dest.ActionedByUserUuiD, src => src.ActionedByUser)
                .Map(dest => dest.UUID, src => src.UUID).TwoWays();

            TypeAdapterConfig<Installment, InstallmentInformationDto>.NewConfig()
                .Map(dest => dest.InstallmentId, src => src.UUID)
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.Amount, src => src.Value)
                .Map(dest => dest.Antecipated, src => src.IsAntecipated)
                .Map(dest => dest.DueDate, src => src.DueDate);

            #endregion
        }
    }
}