using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.FAQs.ChangeDisplayStatus;
using RAG.AI.Application.Commands.Users.ChangeUserStatus;
using RAG.AI.Application.Queries.Users.GetAllUsersForAdmin;
using RAG.AI.Infrastructure.Dtos.Users;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Queries.Users.ExportAllUsersForAdmin;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Application.Queries.Users.GetUserDetailForAdmin;
using RAG.AI.Application.Queries.UserBanks.GetAllUserBanksForAdmin;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllUsersForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new UserViewModel(query, items));
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDisplayStatus(ChangeUserStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserDetailForAdminQuery(id, null, null), cancellationToken);
        var userBank = await _mediator.Send(new GetAllUserBanksForAdminQuery(null, null, null, null, null, null, null, user.Id, null, null, 1, 100), cancellationToken);
        return PartialView(new UserDetailViewModel(user, userBank));
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportUsers([FromQuery] ExportAllUsersForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"users-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }

    //[HttpGet("get-current-user-info", Name = "GetUserInfo")]
    //public async Task<ActionResult<UserDetailForClientDto>> GetUserDetail()
    //{
    //    //if (_userInfo?.UserDetail?.Id == 0 || _userInfo?.UserDetail is null)
    //    //    return Unauthorized();

    //    GetUserDetailForClientQuery query = new(1);
    //    UserDetailForClientDto result = await _mediator.Send(query);

    //    return Ok(result);
    //}

    //public IActionResult Create()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditUserDto dto, CancellationToken cancellationToken)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Messages.InvalidState();
    //    }
    //    try
    //    {
    //        await _mediator.Send(new CreateUserCommand { CreateOrEditUserDto = dto }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}


    //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    //{
    //    var item = await _mediator.Send(new GetUserDetailForEditQuery { UserId = id }, cancellationToken);
    //    return View(item);
    //}

    //[HttpPut]
    //public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditUserDto dto, CancellationToken cancellationToken)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Messages.InvalidState();
    //    }
    //    try
    //    {
    //        await _mediator.Send(new EditUserCommand { CreateOrEditUserDto = dto }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}

    //[HttpDelete]
    //public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        await _mediator.Send(new DeleteUserCommand { UserId = id }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}

    //public async Task<IActionResult> ExportUsers(UserFilterInput filter, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(new ExportUserQuery { UserFilterInput = filter }, cancellationToken);
    //    return result != null && result.Length > 0 ? File(result, "text/csv", $"users.csv") : (IActionResult)BadRequest();
    //}

    //public async Task<ActionResult<ReturnedDto>> ActiveDeActiveUser(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        await _mediator.Send(new ActiveDeActiveUserCommand { UserId = id }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}

    //public IActionResult ResetPassword(int id)
    //{
    //    return View(new ResetPasswordByAdminDto { UserId = id });
    //}

    //[HttpPost]
    //public async Task<ActionResult<ReturnedDto>> ResetPassword(ResetPasswordByAdminDto command, CancellationToken cancellationToken)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Messages.InvalidState();
    //    }
    //    try
    //    {
    //        await _mediator.Send(new ResetPasswordByAdminCommand()
    //        {
    //            ResetPasswordByAdminDto = command
    //        }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }

    //}
}


