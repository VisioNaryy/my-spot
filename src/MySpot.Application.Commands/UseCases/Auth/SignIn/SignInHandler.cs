using MySpot.Data.EF.Repositories.Users.Interfaces;
using MySpot.Infrastructure.Services.UseCases.Security.Interfaces;
using MySpot.Services.Exceptions;

namespace MySpot.Services.UseCases.Auth.SignIn;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticator _authenticator;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenStorage _tokenStorage;

    public SignInHandler(
        IUserRepository userRepository, 
        IAuthenticator authenticator, 
        IPasswordManager passwordManager,
        ITokenStorage tokenStorage)
    {
        _userRepository = userRepository;
        _authenticator = authenticator;
        _passwordManager = passwordManager;
        _tokenStorage = tokenStorage;
    }
    
    public async Task HandleAsync(SignIn command)
    {
        var (email, password) = command;
        
        var user = await _userRepository.GetByEmailAsync(email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!_passwordManager.Validate(password, user.Password))
        {
            throw new InvalidCredentialsException();
        }

        var jwt = _authenticator.CreateToken(user.Id, user.Role);
        _tokenStorage.Set(jwt);
    }
}