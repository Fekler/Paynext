using Mapster;
using Microsoft.Extensions.Logging;
using Paynext.Application.Dtos.Entities.User;
using Paynext.Application.Interfaces;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Domain.Interfaces.Repositories;
using Paynext.Domain.Validations;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Business
{
    public class UserBusiness(IUserRepository userRepository, ILogger<UserBusiness> logger) : IUserBusiness
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<UserBusiness> _logger = logger;

        public async Task<Response<Guid>> Add(CreateUserDto createUserDto)
        {
            try
            {
                var existingUserByEmail = await _userRepository.GetByEmail(createUserDto.Email);
                if (existingUserByEmail != null)
                {
                    return new Response<Guid>().Failure(default, message: Const.CREATE_SUCCESS, statusCode: HttpStatusCode.Conflict);
                }

                var existingUserByDocument = await _userRepository.GetByDocument(createUserDto.Document);
                if (existingUserByDocument != null)
                {
                    return new Response<Guid>().Failure(default, message: "Já existe um usuário cadastrado com este documento.", statusCode: HttpStatusCode.Conflict);
                }

                var existingUserByPhone = await _userRepository.GetByPhone(createUserDto.Phone);
                if (existingUserByPhone != null)
                {
                    return new Response<Guid>().Failure(default, message: "Já existe um usuário cadastrado com este telefone.", statusCode: HttpStatusCode.Conflict);
                }

                var user = createUserDto.Adapt<User>();
                user.Validate();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash da senha antes de salvar
                await _userRepository.Add(user);
                _logger.LogInformation($"Usuário criado com UUID: {user.UUID}");
                return new Response<Guid>().Success(user.UUID, statusCode: HttpStatusCode.Created);
            }
            catch(DomainExceptionValidation ex)
            {
                return new Response<Guid>().Failure(default, message: ex.Message, statusCode: HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar novo usuário.");
                return new Response<Guid>().Failure(default, message: "Erro ao adicionar novo usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                // Implementar lógica de exclusão por ID (se necessário)
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar deletar usuário com ID: {id}.");
                return new Response<bool>().Failure(false, message: "Erro ao deletar usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Delete(Guid guid)
        {
            try
            {
                var userToDelete = await _userRepository.Get(guid);
                if (userToDelete == null)
                {
                    return new Response<bool>().Failure(false, message: "Usuário não encontrado.", statusCode: HttpStatusCode.NotFound);
                }

                var result = await _userRepository.Delete(userToDelete.UUID);
                if (result)
                {
                    _logger.LogInformation($"Usuário com UUID: {guid} deletado com sucesso.");
                    return new Response<bool>().Success(result, statusCode: HttpStatusCode.OK);
                }
                else
                {
                    _logger.LogWarning($"Falha ao deletar usuário com UUID: {guid}.");
                    return new Response<bool>().Failure(false, message: "Falha ao deletar usuário.", statusCode: HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar deletar usuário com UUID: {guid}.");
                return new Response<bool>().Failure(false, message: "Erro ao deletar usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<UserDto>> Get(int id)
        {
            try
            {
                // Implementar lógica para obter DTO por ID (se necessário)
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar obter usuário com ID: {id}.");
                return new Response<UserDto>().Failure(default, message: "Erro ao obter usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<UserDto>> GetDto(Guid guid)
        {
            try
            {
                var user = await _userRepository.Get(guid);
                if (user == null)
                {
                    return new Response<UserDto>().Failure(default, message: "Usuário não encontrado.", statusCode: HttpStatusCode.NotFound);
                }
                var userDto = user.Adapt<UserDto>();
                return new Response<UserDto>().Success(userDto, statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar obter usuário com UUID: {guid} como DTO.");
                return new Response<UserDto>().Failure(default, message: "Erro ao obter usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<User>> GetEntity(Guid guid)
        {
            try
            {
                var user = await _userRepository.Get(guid);
                if (user == null)
                {
                    return new Response<User>().Failure(default, message: "Usuário não encontrado.", statusCode: HttpStatusCode.NotFound);
                }
                return new Response<User>().Success(user, statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar obter usuário com UUID: {guid} como entidade.");
                return new Response<User>().Failure(default, message: "Erro ao obter usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> Update(UpdateUserDto updateUserDto)
        {
            try
            {
                var existingUser = await _userRepository.Get(updateUserDto.UUID);
                if (existingUser == null)
                {
                    return new Response<bool>().Failure(false, message: "Usuário não encontrado.", statusCode: HttpStatusCode.NotFound);
                }

                updateUserDto.Adapt(existingUser);
                // Adicionar lógica para não permitir atualização de senha aqui, criar método específico se necessário

                var result = await _userRepository.Update(existingUser);
                if (result)
                {
                    _logger.LogInformation($"Usuário com UUID: {updateUserDto.UUID} atualizado com sucesso.");
                    return new Response<bool>().Success(result, statusCode: HttpStatusCode.OK);
                }
                else
                {
                    _logger.LogWarning($"Falha ao atualizar usuário com UUID: {updateUserDto.UUID}.");
                    return new Response<bool>().Failure(false, message: "Falha ao atualizar usuário.", statusCode: HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar atualizar usuário com UUID: {updateUserDto.UUID}.");
                return new Response<bool>().Failure(false, message: "Erro ao atualizar usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<User>> Get(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmail(email);
                if (user == null)
                {
                    return new Response<User>().Failure(default, message: "Usuário não encontrado com este e-mail.", statusCode: HttpStatusCode.NotFound);
                }
                return new Response<User>().Success(user, message: Const.MESSAGE_USER_FOUND, statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar obter usuário com e-mail: {email}.");
                return new Response<User>().Failure(default, message: "Erro ao obter usuário.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var existingUser = await _userRepository.Get(changePasswordDto.UserUuid);
                if (existingUser == null)
                {
                    return new Response<bool>().Failure(false, message: "Usuário não encontrado.", statusCode: HttpStatusCode.NotFound);
                }

                if (!BCryptService.VerifyPassword(changePasswordDto.OldPassword, existingUser.Password))
                {
                    return new Response<bool>().Failure(false, message: "A senha antiga está incorreta.", statusCode: HttpStatusCode.Unauthorized);
                }

                string newHashedPassword = BCryptService.HashPassword(changePasswordDto.Password);

                existingUser.Password = newHashedPassword;
                var updateResult = await _userRepository.Update(existingUser);

                if (updateResult)
                {
                    _logger.LogInformation($"Senha do usuário com UUID: {changePasswordDto.UserUuid} alterada com sucesso.");
                    return new Response<bool>().Success(true, message: "Senha alterada com sucesso.", statusCode: HttpStatusCode.OK);
                }
                else
                {
                    _logger.LogWarning($"Falha ao alterar a senha do usuário com UUID: {changePasswordDto.UserUuid}.");
                    return new Response<bool>().Failure(false, message: "Falha ao alterar a senha.", statusCode: HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao tentar alterar a senha do usuário com UUID: {changePasswordDto.UserUuid}.");
                return new Response<bool>().Failure(false, message: "Erro ao alterar a senha.", statusCode: HttpStatusCode.InternalServerError);
            }
        }
        public async Task<Response<IEnumerable<UserDto>>> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAll();
                var userDtos = users.Adapt<IEnumerable<UserDto>>();
                return new Response<IEnumerable<UserDto>>().Success(userDtos, statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar obter todos os usuários.");
                return new Response<IEnumerable<UserDto>>().Failure(default, message: "Erro ao obter todos os usuários.", statusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<IEnumerable<UserDto>>> GetAllByRole(string role)
        {
            try
            {
                var roleEnum = Enum.Parse<UserRole>(role, true);
                var users = await _userRepository.GetAllByRole(roleEnum);


                var userDtos = users.Adapt<IEnumerable<UserDto>>();
                return new Response<IEnumerable<UserDto>>().Success(userDtos, statusCode: HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar obter todos os usuários.");
                return new Response<IEnumerable<UserDto>>().Failure(default, message: "Erro ao obter todos os usuários.", statusCode: HttpStatusCode.InternalServerError);
            }
        }
    }
}
