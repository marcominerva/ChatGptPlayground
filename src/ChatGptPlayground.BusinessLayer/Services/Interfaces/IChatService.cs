using ChatGptPlayground.Shared.Models;
using OperationResults;

namespace ChatGptPlayground.BusinessLayer.Services.Interfaces;

public interface IChatService
{
    Task<Result<ChatResponse>> AskAsync(ChatRequest request);

    IAsyncEnumerable<string> AskStreamAsync(ChatRequest request);

    Task<Result> DeleteAsync(Guid conversationId);
}