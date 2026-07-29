using Microsoft.Extensions.AI;

namespace AgentPlayground.Models;

// Text is null in the final message, that only contains the token usage of the whole response.
public record class Response(Guid ConversationId, string? Text, StreamState StreamState, UsageDetails? TokenUsage = null);