namespace RAG.AI.Infrastructure.Configurations;
public class BackgroundServicesConfig
{
    public const string Key = "BackgroundServices";

    public string CloseAnsweredTickets { get; set; }
    public long SystemUserId { get; set; }
}


