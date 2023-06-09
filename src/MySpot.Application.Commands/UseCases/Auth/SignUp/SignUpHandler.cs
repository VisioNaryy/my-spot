using MySpot.Data.EF.Repositories.Users.Interfaces;
using MySpot.Domain.Data.Entities;
using MySpot.Domain.Data.ValueObjects;
using MySpot.Domain.Services.UseCases.Date.Interfaces;
using MySpot.Infrastructure.Services.UseCases.Security.Interfaces;
using MySpot.Services.Exceptions;

namespace MySpot.Services.UseCases.Auth.SignUp;

internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IClock _clock;

    public SignUpHandler(
        IUserRepository userRepository, 
        IPasswordManager passwordManager,
        IClock clock)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _clock = clock;
    }

    public async Task HandleAsync(SignUp command)
    {
        var userId = new UserId(command.UserId);
        var email = new Email(command.Email);
        var username = new Username(command.Username);
        var password = new Password(command.Password);
        var fullName = new FullName(command.FullName);
        var userRole = string.IsNullOrWhiteSpace(command.Role) ? Role.User() : new Role(command.Role);
        
        if (await _userRepository.GetByEmailAsync(email) is not null)
        {
            throw new EmailAlreadyInUseException(email);
        }

        if (await _userRepository.GetByUsernameAsync(username) is not null)
        {
            throw new UsernameAlreadyInUseException(username);
        }

        var securedPassword = _passwordManager.Secure(password);
        var user = new User(userId, email, username, securedPassword, fullName, userRole, _clock.Current());
        await _userRepository.AddAsync(user);
    }
}