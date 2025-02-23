using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RAG.AI.Domain.Aggregates.UserAggregate;

namespace RAG.AI.Infrastructure.Extentions.Attributes;

public class UserAuthorizationAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly UserType _userType;
    public UserAuthorizationAttribute()
    {

    }
    public UserAuthorizationAttribute(UserType userType)
    {
        _userType = userType;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        string? strUserId = user.Claims.FirstOrDefault(e => e.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(strUserId))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        // Resolve UserDetailModel from the service provider
        //var userInfo = context.HttpContext.RequestServices.GetService<UserDetailModel>();

        //if (userInfo == null)
        //{
        //    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    return;
        //}

        //userInfo.UserDetail.Id = int.Parse(strUserId);
        //await userInfo.GetInfo();


        //var hasPermission = userInfo.HasPermission(_permission, _level);
        //if (!hasPermission)
        //{
        //    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        //    return;
        //}
    }
}


