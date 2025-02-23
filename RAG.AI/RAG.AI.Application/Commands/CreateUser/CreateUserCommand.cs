using BookHouse.Identity.Application.Behaviors;
using RAG.AI.Infrastructure.Dtos.Users;
using System.Text.Json.Serialization;

namespace RAG.AI.Application.Commands.CreateUser;

public record CreateUserCommand(string Mobile, string NationalCode, DateTime Birthdate, int CityId, string? OtpCodeNumber, string? Description, bool CheckOtp = true) : ICommand<CreatedUserResponseDto>
{
    [JsonIgnore]
    public string UserName => Mobile;
    [JsonIgnore]
    public string? Email => $"{Mobile}@ketab.ir";
    [JsonIgnore]
    public string? Phone => Mobile;
    [JsonIgnore]
    public string Password => Guid.NewGuid().ToString("N").Substring(0, 12);
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("شماره موبایل الزامی است.")
            .Matches(@"^09\d{9}$").WithMessage("فرمت شماره موبایل صحیح نیست.");

        RuleFor(x => x.NationalCode)
            .NotEmpty().WithMessage("کد ملی الزامی است.")
            .Length(10).WithMessage("کد ملی باید 10 رقمی باشد.")
            .Matches(@"^\d{10}$").WithMessage("کد ملی باید فقط شامل اعداد باشد.");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("تاریخ تولد الزامی است.")
            .LessThan(DateTime.Now).WithMessage("تاریخ تولد نمی‌تواند در آینده باشد.");

        RuleFor(x => x.OtpCodeNumber)
            .NotEmpty().WithMessage("کد تایید الزامی است.")
            .Length(6).WithMessage("کد تایید باید 6 رقمی باشد.")
            .When(x => x.CheckOtp);

        //RuleFor(x => x.RequestToken)
        //    .NotEmpty().WithMessage("توکن درخواست نمی‌تواند خالی باشد.")
        //    .When(x => x.RequestToken != null);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .Equal(x => x.Mobile).WithMessage("نام کاربری باید با شماره موبایل یکسان باشد.");

    }
}


