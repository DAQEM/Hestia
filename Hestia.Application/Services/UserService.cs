using Hestia.Application.Dtos.User;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Domain.Result;

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
            user.Bio = "Hi, I'm new here!";
            user.Joined = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            user.LastActive = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            
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
    
    public async Task<IResult<bool>> UpdateUserLastActive(int userId, long lastActive)
    {
        try
        {
            User? existingUser = await userRepository.GetAsync(userId);

            if (existingUser is null)
            {
                return new ServiceResult<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found"
                };
            }

            existingUser.LastActive = lastActive;

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<bool>
            {
                Data = true,
                Success = true,
                Message = "User updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<bool>
            {
                Data = false,
                Success = false,
                Message = ex.Message
            };
        }
    }
    
    public async Task<IResult<UserDto?>> UpdateNameAndBioAsync(int userId, string name, string bio)
    {
        Dictionary<string, string[]> errors = new();
        
        if (name.Length is > 24 or < 3)
        {
            errors.Add(nameof(name), ["Name must be between 3 and 24 characters"]);
        }
        
        if (bio.Length is > 160 or < 5)
        {
            errors.Add(nameof(bio), ["Bio must be between 5 and 160 characters"]);
        }
        
        if (name.Contains(' '))
        {
            errors.Add(nameof(name), ["Name cannot contain spaces"]);
        }

        if (errors.Count > 0)
        {
            return new ServiceResult<UserDto?>
            {
                Data = null,
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };
        }
        
        try
        {
            User? existingUser = await userRepository.GetAsync(userId);

            if (existingUser is null)
            {
                return new ServiceResult<UserDto?>
                {
                    Data = null,
                    Success = false,
                    Message = "User not found"
                };
            }

            existingUser.Name = name;
            existingUser.Bio = bio;

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<UserDto?>
            {
                Data = UserDto.FromModel(existingUser),
                Success = true,
                Message = "User updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<UserDto?>
            {
                Data = null,
                Success = false,
                Message = ex.Message
            };
        }
    }
}