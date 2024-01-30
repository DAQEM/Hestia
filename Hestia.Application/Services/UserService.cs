using Hestia.Application.Dtos.User;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;

namespace Hestia.Application.Services;

public class UserService(IUserRepository userRepository)
{
    public async Task<IResult<UserDto?>> GetAsync(int id)
    {
        User? user = await userRepository.GetAsync(id);
        
        if (user is null)
        {
            return new ServiceResult<UserDto?>
            {
                Data = null,
                Success = false,
                Message = "User not found"
            };
        }
        
        return new ServiceResult<UserDto?>
        {
            Data = UserDto.FromModel(user),
            Success = true,
            Message = "User found"
        };
    }
    
    public async Task<IResult<UserDto?>> GetUserWithAccountByEmailAsync(string email)
    {
        User? user = await userRepository.GetUserWithAccountByEmailAsync(email);
        
        if (user is null)
        {
            return new ServiceResult<UserDto?>
            {
                Data = null,
                Success = false,
                Message = "User not found"
            };
        }
        
        return new ServiceResult<UserDto?>
        {
            Data = UserDto.FromModel(user),
            Success = true,
            Message = "User found"
        };
    }
    
    public async Task<IResult<IEnumerable<UserDto>>> GetAllAsync()
    {
        IEnumerable<User> users = await userRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<UserDto>>
        {
            Data = users.Select(UserDto.FromModel),
            Success = true,
            Message = "Users found"
        };
    }
    
    public async Task<IResult<UserDto>> AddAsync(UserDto user)
    {
        try
        {
            User addedUser = await userRepository.AddAsync(user.ToModel());

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<UserDto>
            {
                Data = UserDto.FromModel(addedUser),
                Success = true,
                Message = "User added successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<UserDto>
            {
                Data = null,
                Success = false,
                Message = ex.Message
            };
        }
    }
    
    public async Task<IResult<UserDto>> UpdateAsync(int id, UserDto user)
    {
        try
        {
            User? existingUser = await userRepository.GetAsync(id);

            if (existingUser is null)
            {
                return new ServiceResult<UserDto>
                {
                    Data = null,
                    Success = false,
                    Message = "User not found"
                };
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Image = user.Image;
            existingUser.Role = (Role) user.Role;
            existingUser.Accounts = user.Accounts?.Select(x => x.ToModel()).ToList() ?? [];

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<UserDto>
            {
                Data = UserDto.FromModel(existingUser),
                Success = true,
                Message = "User updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<UserDto>
            {
                Data = null,
                Success = false,
                Message = ex.Message
            };
        }
    }
    
    public async Task<IResult<UserDto>> DeleteAsync(int id)
    {
        try
        {
            User? existingUser = await userRepository.GetAsync(id);

            if (existingUser is null)
            {
                return new ServiceResult<UserDto>
                {
                    Data = null,
                    Success = false,
                    Message = "User not found"
                };
            }

            await userRepository.DeleteAsync(existingUser.Id);

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<UserDto>
            {
                Data = UserDto.FromModel(existingUser),
                Success = true,
                Message = "User deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<UserDto>
            {
                Data = null,
                Success = false,
                Message = ex.Message
            };
        }
    }
    
    public async Task<IResult<UserDto?>> GetUserByUserNameAsync(string username)
    {
        User? user = await userRepository.GetUserByUserNameAsync(username);
        
        if (user is null)
        {
            return new ServiceResult<UserDto?>
            {
                Data = null,
                Success = false,
                Message = "User not found"
            };
        }
        
        return new ServiceResult<UserDto?>
        {
            Data = UserDto.FromModel(user),
            Success = true,
            Message = "User found"
        };
    }
}