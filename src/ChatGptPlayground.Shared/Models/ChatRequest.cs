namespace ChatGptPlayground.Shared.Models;

public record class ChatRequest(Guid ConversationId, string Message);