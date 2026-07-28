using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Encodings.Web;
using System.Text.Json;
using AgentPlayground.Components;
using AgentPlayground.Services;
using AgentPlayground.Settings;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;
using TinyHelpers.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
var aiSettings = builder.Services.ConfigureAndGet<AzureOpenAISettings>(builder.Configuration, "AzureOpenAI")!;
var appSettings = builder.Services.ConfigureAndGet<AppSettings>(builder.Configuration, nameof(AppSettings))!;

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.ConfigureHttpClientDefaults(configure =>
{
    configure.AddStandardResilienceHandler(options =>
    {
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(2);
    });
});

builder.Services.AddChatClient(_ =>
{
    var chatClient = new OpenAIClient(new ApiKeyCredential(aiSettings.ApiKey), new()
    {
        Endpoint = new(aiSettings.Endpoint),
        Transport = new HttpClientPipelineTransport(new HttpClient(new TraceHttpClientHandler()))
    }).GetResponsesClient().AsIChatClientWithStoredOutputDisabled(aiSettings.Deployment);

    return chatClient;
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new()
    {
        LocalCacheExpiration = appSettings.MessageExpiration
    };
});

builder.Services.AddSingleton<HybridCacheSessionStoreService>();

builder.Services.AddAIAgent("PlaygroundAgent", (services, key) =>
{
    var chatClient = services.GetRequiredService<IChatClient>();

    return chatClient.AsAIAgent(new ChatClientAgentOptions
    {
        Id = key.ToLowerInvariant(),
        Name = key,
        ChatOptions = new()
        {
            Instructions = """
                You are a helpful assistant. Answer the user's questions in the same language as the question.
                """,
            Reasoning = new()
            {
                Effort = ReasoningEffort.Low,
                Output = ReasoningOutput.None
            },
            Tools = [new HostedWebSearchTool()]
        },
        ChatHistoryProvider = new InMemoryChatHistoryProvider(new()
        {
            ChatReducer = new MessageCountingChatReducer(appSettings.MessageLimit),
            ReducerTriggerEvent = InMemoryChatHistoryProviderOptions.ChatReducerTriggerEvent.AfterMessageAdded
        })
    },
    loggerFactory: services.GetRequiredService<ILoggerFactory>(),
    services: services);
}, ServiceLifetime.Scoped)
.WithSessionStore((services, _) => services.GetRequiredService<HybridCacheSessionStoreService>(), withIsolation: false);

builder.Services.AddScoped<AgentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);

    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/error?code={0}");

app.UseRouting();
app.UseRequestLocalization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public class TraceHttpClientHandler : HttpClientHandler
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestString = request.Content is null ? "(no request body)" : await request.Content.ReadAsStringAsync(cancellationToken);

        PrintText($"Raw Request ({request.RequestUri})", ConsoleColor.Green);
        PrintText(FormatJson(requestString), ConsoleColor.DarkGray);
        PrintSeparator();

        var response = await base.SendAsync(request, cancellationToken);

        return response;

        static void PrintText(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void PrintSeparator() => Console.WriteLine(new string('-', 50));
    }

    private static string FormatJson(string input)
    {
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(input);
            return JsonSerializer.Serialize(jsonElement, jsonSerializerOptions);
        }
        catch
        {
            return input;
        }
    }
}
