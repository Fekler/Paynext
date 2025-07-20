using Mapster;
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
        }
    }
}