using Paynext.Domain.Entities._bases;
using SharedKernel;

namespace Paynext.Application.Interfaces._bases
{
    public interface IBusinessBase<T, TCreateDto, TUpdateDto, TGetDto> where T : EntityBase
    {
        Task<Response<Guid>> Add(TCreateDto dto);
        Task<Response<bool>> Delete(int id);
        Task<Response<bool>> Delete(Guid guid);
        Task<Response<TGetDto>> Get(int id);
        Task<Response<TGetDto>> GetDto(Guid guid);
        Task<Response<T>> GetEntity(Guid guid);
        Task<Response<bool>> Update(TUpdateDto dto);
    }
}
