namespace AgentPlayground.Settings;

public class AzureOpenAISettings
{
    public required string Endpoint { get; init; }

    public required string Deployment { get; init; }

    public required string ApiKey { get; init; }
}