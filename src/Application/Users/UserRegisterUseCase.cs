using Application.Common.Services;
using Domain.UserEntity;
using SharedKernel;

namespace Application.Users;

public class UserRegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public UserRegisterUseCase(IUserRepository userRepo, IPasswordService passwordService, IJwtService jwtService)
    {
        _userRepository = userRepo;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }
    
    public async Task<Result<string>> Register(UserDto dto)
    {
        var usernameCreateResult = Username.Create(dto.Username);

        if (usernameCreateResult.Value is not { } username)
        {
            return new Error();
        }

        var foundUser = await _userRepository.SearchByName(username);

        if (foundUser is not null)
        {
            return new Error();
        }

        var password = _passwordService.Create(dto.Password);
        var newUser = new User(username, password);
        var result = await _userRepository.Insert(newUser);

        if (!result.IsSuccess)
        {
            return result.Error!;
        }

        return _jwtService.SignToken(result.Value!);
    }
}