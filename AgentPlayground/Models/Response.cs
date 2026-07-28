using Microsoft.Extensions.AI;

namespace AgentPlayground.Models;

// Answer is null in the final message, that only contains the token usage of the whole response.
public record class Response(Guid ConversationId, string? Answer, StreamState StreamState, UsageDetails? TokenUsage = null);