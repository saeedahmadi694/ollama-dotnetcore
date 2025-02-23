namespace RAG.AI.Infrastructure.Persistent.DataSeeding;
public class DataSeeder
{
    private readonly IUnitOfWork _uow;

    public DataSeeder(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task SeedData()
    {
        SeedUsers();
        SeedSettings();

        await _uow.SaveChangesAsync();
    }

    private void SeedSettings()
    {

        //if (!await _uow.SettingRepository.AnyAsync(r => true))
        //{
        //    var setting = new Setting();
        //    setting.SetBuySetting(true, 2500000, 0, 56000000);
        //    setting.SetCashBackSetting(true, 2500000, new());
        //    setting.SetInPersonSetting(true, 2500000, new());
        //    setting.SetReferralSetting(true, 2500000,ReferralGiftType.Both);
        //    setting.SetSellSetting(true, 2500000,0, 56000000);
        //    setting.SetSignUpSetting(true, 2500000);
        //    setting.SetWebServiceSetting(true, 2);
        //    await _uow.SettingRepository.InsertAsync(setting);
        //}
    }

    private void SeedUsers()
    {
        //if (!await _uow.UserRepository.AnyAsync(r => r.NationalCode == "123456789"))
        //{
        //    var user = new User(
        //    "admin",
        //    "admin",
        //    DateTime.Now,
        //    "123456789");

        //    var salt = PasswordHelper.GenerateSalt();
        //    var hashedPassword = PasswordHelper.GeneratePassword(salt, user.NationalCode);

        //    user.SetMobile("09309759014");
        //    user.SetPassword(salt, hashedPassword);
        //    user.SetDescription("");

        //    await _uow.UserRepository.InsertAsync(user);
        //    //await _uow.SaveChangesAsync();


        //    //var cashWallet = new CashWallet($"{user.Mobile}-CASH", 0, true, 1);
        //    //var goldWallet = new GoldWallet($"{user.Mobile}-GOLD", 0, true, 1);

        //    //await _uow.WalletRepository.InsertAsync(cashWallet);
        //    //await _uow.WalletRepository.InsertAsync(goldWallet);

        //}
    }

}

