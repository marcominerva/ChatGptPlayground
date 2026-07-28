namespace AgentPlayground.Settings;

public class AppSettings
{
    public TimeSpan MessageExpiration { get; init; }

    public int MessageLimit { get; set; } = 20;
}
