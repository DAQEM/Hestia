using System.Text.RegularExpressions;
using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Result;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Users;

public partial class UserService(IUserRepository userRepository, IMapper mapper)
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
            Data = mapper.Map<UserDto>(user),
            Success = true,
            Message = "User found"
        };
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
            Data = mapper.Map<UserDto>(user),
            Success = true,
            Message = "User found"
        };
    }

    public async Task<IResult<UserDto?>> GetByOAuthIdAsync(OAuthProvider provider, string userId)
    {
        User? user = await userRepository.GetByOAuthIdAsync(provider, userId);
        
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
            Data = mapper.Map<UserDto>(user),
            Success = true,
            Message = "User found"
        };
    }

    public async Task<IResult<IEnumerable<UserDto>>> GetAllAsync()
    {
        IEnumerable<User> users = await userRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<UserDto>>
        {
            Data = users.Select(mapper.Map<UserDto>),
            Success = true,
            Message = "Users found"
        };
    }

    public async Task<IResult<UserDto>> AddAsync(UserDto user)
    {
        try
        {
            user.Joined = DateTime.UtcNow;
            user.LastActive = DateTime.UtcNow;
            
            User addedUser = await userRepository.AddAsync(mapper.Map<User>(user));

            await userRepository.SaveChangesAsync();
            
            return new ServiceResult<UserDto>
            {
                Data = mapper.Map<UserDto>(addedUser),
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
                Data = mapper.Map<UserDto>(existingUser),
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

    public async Task<IResult<bool>> UpdateUserLastActive(int userId, DateTime lastActive)
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

    public async Task<IResult<UserDto?>> UpdateNameAndBioAsync(int userId, string name, string? bio)
    {
        Dictionary<string, string[]> errors = new();
        
        if (name.Length is > 24 or < 3)
        {
            errors.Add(nameof(name), ["Name must be between 3 and 24 characters"]);
        }
        
        if (name.Contains(' '))
        {
            errors.Add(nameof(name), ["Name cannot contain spaces"]);
        }
        
        if (UsernameRegex().IsMatch(name))
        {
            errors.Add(nameof(name), ["Name can only contain letters and numbers"]);
        }
        
        if (bio?.Length is > 128)
        {
            errors.Add(nameof(bio), ["Bio must be less than 128 characters"]);
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
        
        bio = string.IsNullOrWhiteSpace(bio) ? null : bio;
        
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
                Data = mapper.Map<UserDto>(existingUser),
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

    public async Task UpdateOAuthIdAsync(int userId, OAuthProvider provider, string oAuthUserId)
    {
        await userRepository.UpdateOAuthIdAsync(userId, provider, oAuthUserId);
        await userRepository.SaveChangesAsync();
    }

    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex UsernameRegex();
}