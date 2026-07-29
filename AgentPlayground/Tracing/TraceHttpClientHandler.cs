using System.Text.Encodings.Web;
using System.Text.Json;

namespace AgentPlayground.Tracing;

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

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        PrintText("Raw Response", ConsoleColor.Green);
        PrintText(FormatJson(responseString), ConsoleColor.DarkGray);
        PrintSeparator();

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
