using RAG.AI.Application.Models;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;
using RAG.AI.Infrastructure.ExternalServices;
using RAG.AI.Infrastructure.ExternalServices.KycService;
using RAG.AI.Infrastructure.ExternalServices.TokenService;
using RAG.AI.Infrastructure.Persistent.QueryServices;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;
using RAG.AI.Infrastructure.Persistent.Repositories;

namespace RAG.AI.Presenter.Web.DependencyInjection;

public static class AddRepositoriesInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddSingleton<IDbConnectionFactory, SqlServerDbConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IScheduledJobLogRepository, ScheduledJobLogRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();


        services.AddScoped<ISliderQueryService, SliderQueryService>();
        services.AddScoped<IWithdrawalRequestQueryService, WithdrawalRequestQueryService>();
        services.AddScoped<IFAQQueryService, FAQQueryService>();
        services.AddScoped<INotificationQueryService, NotificationQueryService>();
        services.AddScoped<IOperationQueryService, OperationQueryService>();
        services.AddScoped<IPaymentRequestQueryservice, PaymentRequestQueryservice>();
        services.AddScoped<ITicketCategoryQueryService, TicketCategoryQueryService>();
        services.AddScoped<ITicketDiscussionQueryService, TicketDiscussionQueryService>();
        services.AddScoped<ITicketFileQueryService, TicketFileQueryService>();
        services.AddScoped<ITicketQueryService, TicketQueryService>();
        services.AddScoped<IUserBankQueryService, UserBankQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IWalletChargeRequestQueryService, WalletChargeRequestQueryService>();
        services.AddScoped<IWalletQueryService, WalletQueryService>();
        services.AddScoped<IContactMessageQueryService, ContactMessageQueryService>();
        services.AddScoped<ISettingQueryService, SettingQueryService>();
        services.AddScoped<IGoldTradeRequestQueryService, GoldTradeRequestQueryService>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<ICommentQueryService, CommentQueryService>();
        services.AddScoped<UserDetailModel>();


        services.AddTransient<IKycService, KycService>();
        services.AddTransient<IJibitService, JibitService>();
        services.AddTransient<IJibitAuthenticator, JibitAuthenticator>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IKavenegarService, KavenegarService>();

        return services;
    }
}



