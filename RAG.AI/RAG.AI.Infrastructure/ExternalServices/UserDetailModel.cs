using BookHouse.Identity.Contracts.AspNetCore.Services;
using BookHouse.Identity.Contracts.ServiceBus.Models;
using BookHouse.Identity.Contracts.ServiceBus.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace BookHouse.Identity.Contracts.AspNetCore.Models;
public class UserDetailModel
{
    public UserDetailDto UserDetail { get; set; }

    private readonly IHttpContextAccessor? _httpContext;
    private readonly IUserDetailProvider _userProviderService;
    private UserDetailModel()
    {
        UserDetail = new UserDetailDto();
    }

    public UserDetailModel(IUserDetailProvider userProviderService, IHttpContextAccessor? httpContext) : this()
    {
        _userProviderService = userProviderService;
        _httpContext = httpContext;
        if (_httpContext?.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            var strUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "UserId").Value;
            if (!string.IsNullOrEmpty(strUserId))
            {
                UserDetail.Id = int.Parse(strUserId);
                GetInfo().Wait();
            }
        }
    }

    public async Task GetInfo()
    {
        if (UserDetail?.Id <= 0)
            return;
        UserDetail = await _userProviderService.GetUserDetailByIdAsync(UserDetail.Id);
    }

    public bool HasPermission(string permission, PermissionLevel level)
    {
        return UserDetail?.Permissions?.Any(e => e.Permission == permission && e.Level.HasFlag(level)) ?? false;
    }
}

