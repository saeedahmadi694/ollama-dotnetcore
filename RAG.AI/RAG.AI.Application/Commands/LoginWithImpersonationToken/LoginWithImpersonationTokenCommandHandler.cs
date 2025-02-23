using Microsoft.Extensions.Options;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Dtos.Users;
using RAG.AI.Infrastructure.Exceptions.Users;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;

namespace RAG.AI.Application.Commands.LoginWithImpersonationToken;

public class LoginWithImpersonationTokenCommandHandler : IRequestHandler<LoginWithImpersonationTokenCommand, SignInResponseDto>
{
    private readonly IRedisHelper _redisHelper;
    private readonly IUserQueryService _userQueryService;
    private readonly ITokenService _tokenService;
    private readonly JwtConfig _settings;
    private readonly IUnitOfWork _unitOfWork;
    public LoginWithImpersonationTokenCommandHandler(IRedisHelper redisHelper, ITokenService tokenService, IOptions<JwtConfig> settings, IUserQueryService userQueryService, IUnitOfWork unitOfWork )
    {
        _redisHelper = redisHelper;
        _tokenService = tokenService;
        _settings = settings.Value;
        _userQueryService = userQueryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<SignInResponseDto> Handle(LoginWithImpersonationTokenCommand request, CancellationToken cancellationToken)
    {
        var item = await _redisHelper.Strings.GetAsync(request.ImpersonationToken);

        if (string.IsNullOrEmpty(item))
            throw new UnauthorizedAccessException();

        var userId = int.Parse(item);

        var user = await _userQueryService.GetAsync(r => r.Id == userId);
        if (user is null)
            throw new UserNotFoundException(userId);

        var userRoles = user?.Roles.Select(r => r.RoleId);
        var roles = await _roleQueryService.GetAllAsync(r => userRoles.Any(rr => rr == r.Id));

        var claims = new Dictionary<string, string>
        {
            { "Role", string.Join(",", roles.Select(r=>r.Title)) },
            { "UserId", user.Id.ToString() },
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var userToken = new UserToken((int)user.Id, accessToken, refreshToken, DateTime.Now.AddMinutes(int.Parse(_settings.ExpirationInMinutes)));
        await _unitOfWork.UserTokenRepository.InsertAsync(userToken);

        return new SignInResponseDto
        {
            IsSuccess = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

    }
}


