using BookHouse.Core.Bases;
using BookHouse.Core.Localization;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Application.Commands.CreateUser;

public class CreateUserCommandHandler : ApplicationBaseService, IRequestHandler<CreateUserCommand, CreatedUserResponseDto>
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommunicationService _communicationService;
    private readonly IShipmentService _shipmentService;
    private readonly ISsoRequestQueryService _ssoRequestQueryService;
    private readonly IIdentityMediatrIntegrationEventService _identityInternalIntegrationEventService;
    private readonly IIdentityIntegrationEventService _identityIntegrationEventService;
    private readonly IMediator _mediator;
    public CreateUserCommandHandler(ILogger logger,
        IUnitOfWork unitOfWork,
        IShipmentService shipmentService,
        ISsoRequestQueryService ssoRequestQueryService,
        ICommunicationService communicationService,
        ILocalizationSource source,
        IIdentityMediatrIntegrationEventService identityInternalIntegrationEventService,
        IIdentityIntegrationEventService identityIntegrationEventService,
        IMediator mediator) : base(source)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _shipmentService = shipmentService;
        _ssoRequestQueryService = ssoRequestQueryService;
        _communicationService = communicationService;
        _identityInternalIntegrationEventService = identityInternalIntegrationEventService;
        _identityIntegrationEventService = identityIntegrationEventService;
        _mediator = mediator;
    }

    public async Task<CreatedUserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.CheckOtp)
        {
            var isVerified = await _communicationService.VerifyOtpCode(request.Mobile, request.OtpCodeNumber, OtpSmsTypeBusEnum.Register);
            if (!isVerified)
            {
                return new CreatedUserResponseDto
                {
                    IsSuccess = false,
                    Error = L("OTPCodeIsWrong")
                };
            }
        }

        var isDuplicateUser = await _unitOfWork.UserRepository
            .AnyAsync(x =>
                    x.ContactInfo.Email == request.Email ||
                    x.ContactInfo.Mobile == request.Mobile ||
                    x.NationalCode == request.NationalCode ||
                    x.UserName == request.UserName);

        if (isDuplicateUser)
        {
            return new CreatedUserResponseDto
            {
                IsSuccess = false,
                Error = L("UserExist")
            };
        }


        bool isMobileVerified = false;
        //if (!request.IsUnderEighteen)
        //{
        isMobileVerified = await _mediator.Send(new VerifyMobileOwnershipQuery(request.NationalCode, request.Mobile), cancellationToken);
        if (!isMobileVerified)
        {
            _logger.Error("error while verify ownership of mobile number with message={@Message}", "Mobile does not matched");

            return new CreatedUserResponseDto { IsSuccess = false, Error = L("MobileOwnershipReject") };
        }
        //}

        var userIdentityInfo = await _mediator.Send(new GetIdentityInfoQuery(request.NationalCode, request.Birthdate));
        if (userIdentityInfo == null || userIdentityInfo == default)
        {
            _logger.Error("User identity info doesn't match.");
            return new CreatedUserResponseDto
            {
                IsSuccess = false,
                Error = L("UserInfoProblem")
            };
        }


        var salt = PasswordHelper.GenerateSalt();
        var hashedPassword = PasswordHelper.GeneratePassword(salt, request.Password);

        var city = await _shipmentService.GetGetCityForView(request.CityId);



        var user = new User(request.UserName, userIdentityInfo.FirstName, userIdentityInfo.LastName, userIdentityInfo.FatherName,
        userIdentityInfo.Gender, request.Birthdate, userIdentityInfo.NationalCode);

        user.SetCityInfo(request.CityId, city.CityName, city.ProvinceId, city.ProvinceName);
        user.SetContactInfo(request.Mobile, request.Phone, request.Email);
        user.SetPassword(salt, hashedPassword);
        user.SetDescription(request.Description);


        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync();


        _identityIntegrationEventService.AddAndSaveEventAsync(
            new NewUserCreatedBusEvent
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = (int)user.Id,
                NationalId = user.NationalCode,
                Mobile = user.ContactInfo.Mobile,
            }
        );

        return new CreatedUserResponseDto
        {
            Birthday = user.Birthdate,
            CityId = user.City.CityId,
            Description = user.Description,
            Email = user.ContactInfo.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            IsActive = user.IsActive,
            Mobile = user.ContactInfo.Mobile,
            NationalId = user.NationalCode,
            ProvinceId = user.City.ProvinceId,
            FatherName = user.FatherName,
            Id = user.Id,
            IsSuccess = true
        };
    }
}


