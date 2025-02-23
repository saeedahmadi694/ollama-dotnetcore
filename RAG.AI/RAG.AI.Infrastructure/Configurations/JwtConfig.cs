namespace RAG.AI.Infrastructure.Configurations;

public class JwtConfig
{
    public const string Key = "JwtConfig";
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
    public int ExpirationInMinutes { get; set; }
}


