using Microsoft.AspNetCore.Http;
using RAG.AI.Application.Queries.Users.GetUserDetail;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Application.Models;
public class UserDetailModel
{
    public UserDetailDto UserDetail { get; set; }

    private readonly IHttpContextAccessor? _httpContext;
    private readonly IMediator _mediator;
    private UserDetailModel()
    {
        UserDetail = new UserDetailDto();
    }

    public UserDetailModel(IMediator mediator, IHttpContextAccessor? httpContext) : this()
    {
        _mediator = mediator;
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
        UserDetail = await _mediator.Send(new GetUserDetailQuery(UserDetail?.Id ?? 0), new CancellationToken());
    }

}



