using Mapster;
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
                .Map(dest => dest.CreateAt, src => DateTime.UtcNow)
                .Map(dest => dest.UUID, src => Guid.NewGuid());

            TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreateAt)
                .Map(dest => dest.UpdateAt, src => DateTime.UtcNow);

            #endregion
            #region Contract
            TypeAdapterConfig<ContractDto, Contract>.NewConfig().TwoWays();

            TypeAdapterConfig<CreateContractDto, Contract>.NewConfig()
                .Map(dest => dest.ContractNumber, src => src.ContractNumber.Trim())
                .Map(dest => dest.Description, src => src.Description.Trim())
                .Map(dest => dest.InitialAmount, src => src.Amount)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.UserUuid, src => src.UserUuid)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.UUID, src => Guid.NewGuid());

            TypeAdapterConfig<UpdateContractDto, Contract>.NewConfig()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.ContractNumber, src => src.ContractNumber.Trim())
                .Map(dest => dest.Description, src => src.Description.Trim())
                .Map(dest => dest.InitialAmount, src => src.Amount)
                .Map(dest => dest.StartDate, src => src.StartDate)
                .Map(dest => dest.EndDate, src => src.EndDate)
                .Map(dest => dest.UserUuid, src => src.UserUuid)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.UUID, src => src.Uuid);
            #endregion

            #region Installment
            TypeAdapterConfig<InstallmentDto, Installment>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateInstallmentDto, Installment>.NewConfig()
                //.Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.DueDate, src => src.DueDate)
                .Map(dest => dest.ContractUuid, src => src.ContractUuid)
                .Map(dest => dest.UUID, src => Guid.NewGuid());
            TypeAdapterConfig<UpdateInstallmentDto, Installment>.NewConfig()
                .Ignore(dest => dest.Id)
                //.Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.DueDate, src => src.DueDate)
                .Map(dest => dest.ContractUuid, src => src.ContractUuid)
                .Map(dest => dest.UUID, src => src.Uuid);
            #endregion
        }
    }
}